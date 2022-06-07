using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.SandBox;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ModuleManager;
using TaleWorlds.MountAndBlade;
using TaleWorlds.SaveSystem.Load;
using WoT_Main;

namespace SandBox
{
	
	public class WoTCampaignManager : MBGameManager
	{
		//Code copied from Taleworlds
		public WoTCampaignManager()
		{
			this._loadingSavedGame = false;
			this._seed = ((int)DateTime.Now.Ticks & 65535);
		}

		
		public override void OnGameEnd(Game game)
		{
			MBDebug.SetErrorReportScene(null);
			base.OnGameEnd(game);
		}

		
		protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
		{
			nextStep = GameManagerLoadingSteps.None;
			switch (gameManagerLoadingStep)
			{
				case GameManagerLoadingSteps.PreInitializeZerothStep:
					nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
					return;
				case GameManagerLoadingSteps.FirstInitializeFirstStep:
					MBGameManager.LoadModuleData(this._loadingSavedGame);
					nextStep = GameManagerLoadingSteps.WaitSecondStep;
					return;
				case GameManagerLoadingSteps.WaitSecondStep:
					if (!this._loadingSavedGame)
					{
						MBGameManager.StartNewGame();
					}
					nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
					return;
				case GameManagerLoadingSteps.SecondInitializeThirdState:
					MBGlobals.InitializeReferences();
					if (!this._loadingSavedGame)
					{
						MBDebug.Print("Initializing new game begin...", 0, Debug.DebugColor.White, 17592186044416UL);
						Campaign campaign = new Campaign(CampaignGameMode.Campaign);
						Game.CreateGame(campaign, this);
						campaign.SetLoadingParameters(Campaign.GameLoadingType.NewCampaign, this._seed);
						MBDebug.Print("Initializing new game end...", 0, Debug.DebugColor.White, 17592186044416UL);
					}
					else
					{
						MBDebug.Print("Initializing saved game begin...", 0, Debug.DebugColor.White, 17592186044416UL);
						((Campaign)Game.LoadSaveGame(this._loadedGameResult, this).GameType).SetLoadingParameters(Campaign.GameLoadingType.SavedCampaign, this._seed);
						this._loadedGameResult = null;
						Common.MemoryCleanupGC(false);
						MBDebug.Print("Initializing saved game end...", 0, Debug.DebugColor.White, 17592186044416UL);
					}
					Game.Current.DoLoading();
					nextStep = GameManagerLoadingSteps.PostInitializeFourthState;
					return;
				case GameManagerLoadingSteps.PostInitializeFourthState:
					{
						bool flag = true;
						foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.SubModules)
						{
							flag = (flag && mbsubModuleBase.DoLoading(Game.Current));
						}
						nextStep = (flag ? GameManagerLoadingSteps.FinishLoadingFifthStep : GameManagerLoadingSteps.PostInitializeFourthState);
						return;
					}
				case GameManagerLoadingSteps.FinishLoadingFifthStep:
					nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.None : GameManagerLoadingSteps.FinishLoadingFifthStep);
					return;
				default:
					return;
			}
		}

		
		public override void OnLoadFinished()
		{
			if (!this._loadingSavedGame)
			{
				MBDebug.Print("Switching to menu window...", 0, Debug.DebugColor.White, 17592186044416UL);
				
				this.LaunchSandboxCharacterCreation();
				
			}
			else
			{
				if (CampaignSiegeTestStatic.IsSiegeTestBuild)
				{
					CampaignSiegeTestStatic.DisableSiegeTest();
				}
				Game.Current.GameStateManager.OnSavedGameLoadFinished();
				Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<MapState>(), 0);
				MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
				string text = (mapState != null) ? mapState.GameMenuId : null;
				if (!string.IsNullOrEmpty(text))
				{
					PlayerEncounter playerEncounter = PlayerEncounter.Current;
					if (playerEncounter != null)
					{
						playerEncounter.OnLoad();
					}
					Campaign.Current.GameMenuManager.SetNextMenu(text);
				}
				IPartyVisual visuals = PartyBase.MainParty.Visuals;
				if (visuals != null)
				{
					visuals.SetMapIconAsDirty();
				}
				Campaign.Current.CampaignInformationManager.OnGameLoaded();
				foreach (Settlement settlement in Settlement.All)
				{
					settlement.Party.Visuals.RefreshLevelMask(settlement.Party);
				}
				CampaignEventDispatcher.Instance.OnGameLoadFinished();
				if (mapState != null)
				{
					mapState.OnLoadingFinished();
				}
			}
			base.IsLoaded = true;
		}

		
		private void LaunchSandboxCharacterCreation()
		{
			CharacterCreationState gameState = Game.Current.GameStateManager.CreateState<CharacterCreationState>(new object[]
			{
				new WoTCharacterCreation()
			});
			Game.Current.GameStateManager.CleanAndPushState(gameState, 0);
		}

		private bool _loadingSavedGame;

		private LoadResult _loadedGameResult;

		private int _seed = 1234;
	}
}