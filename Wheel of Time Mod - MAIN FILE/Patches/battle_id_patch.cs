using HarmonyLib;
using Helpers;
using Newtonsoft.Json.Linq;
using SandBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.BarterSystem;
using TaleWorlds.CampaignSystem.BarterSystem.Barterables;
using TaleWorlds.CampaignSystem.CampaignBehaviors;
using TaleWorlds.CampaignSystem.CampaignBehaviors.BarterBehaviors;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using WoT_Main.Support;
using Path = System.IO.Path;
using SandBox.View.Map;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.ModuleManager;
using TaleWorlds.ObjectSystem;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Localization;

namespace WoT_Main.Patches
{
    
	[HarmonyPatch]
    public static class GetBattleSceneForMapPatchPatch
	{
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerEncounter), "GetBattleSceneForMapPatch")]
        public static bool PreFix(ref MapPatchData mapPatch, ref string __result)
        {
			PathFaceRecord faceIndex = Campaign.Current.MapSceneWrapper.GetFaceIndex(MobileParty.MainParty.Position2D);

			
			int id = faceIndex.FaceGroupIndex;

			string sceneID = "n";


				try
				{
					JObject config = JObject.Parse(File.ReadAllText(Path.GetFullPath(BasePath.Name + "Modules/Wheel of Time Mod/config.json")));
					
					string[] sceneIds = null;
					
					
					switch (id)
                    {
						case 1: sceneIds = config.GetValue("faceIndex1").ToObject<string[]>(); break;
						case 2: sceneIds = config.GetValue("faceIndex2").ToObject<string[]>(); break;
						case 4: sceneIds = config.GetValue("faceIndex4").ToObject<string[]>(); break;
						case 5: sceneIds = config.GetValue("faceIndex5").ToObject<string[]>(); break;
						case 7: sceneIds = config.GetValue("faceIndex7").ToObject<string[]>(); break;
						case 10: sceneIds = config.GetValue("faceIndex10").ToObject<string[]>(); break;


						case 50: sceneIds = config.GetValue("faceIndex50").ToObject<string[]>(); break;
						case 51: sceneIds = config.GetValue("faceIndex51").ToObject<string[]>(); break;
						case 52: sceneIds = config.GetValue("faceIndex52").ToObject<string[]>(); break;
						case 53: sceneIds = config.GetValue("faceIndex53").ToObject<string[]>(); break;
						case 54: sceneIds = config.GetValue("faceIndex54").ToObject<string[]>(); break;
						case 55: sceneIds = config.GetValue("faceIndex55").ToObject<string[]>(); break;
						case 56: sceneIds = config.GetValue("faceIndex56").ToObject<string[]>(); break;
						case 57: sceneIds = config.GetValue("faceIndex57").ToObject<string[]>(); break;
						case 58: sceneIds = config.GetValue("faceIndex58").ToObject<string[]>(); break;
						case 59: sceneIds = config.GetValue("faceIndex59").ToObject<string[]>(); break;
						default: config.GetValue("faceIndex50").ToObject<string[]>(); break;
					}
				

						
					if (sceneID == "n" && sceneIds != null)
                    {
						sceneID = sceneIds.GetRandomElement();
					}
					
				}
				catch (Exception)
				{
					campaignSupport.displayMessageInChat("config wasn't loaded", Colors.Red);
				}
			
			
			//__result = "scn_test";
			__result = sceneID;

			return false;
        }
    }

	//[HarmonyPatch]
	//public static class GetDistancePatch
	//{
	//
	//	[HarmonyPrefix]
	//	[HarmonyPatch(typeof(DefaultMapDistanceModel), "GetDistance")]
	//	public static bool PreFix(ref Settlement fromSettlement, ref Settlement toSettlement, ref float maximumDistance, ref float distance, ref bool __result)
	//	{
	//		if(fromSettlement == null || toSettlement == null)
	//        {
	//			__result = false;
	//			distance = 20;
	//			return false;
	//        }
	//		distance = 1;
	//
	//		return true;
	//	}
	//}
	[HarmonyPatch]
	public static class ChildGenPatch
	{
	
		[HarmonyPrefix]
		[HarmonyPatch(typeof(InitialChildGenerationCampaignBehavior), "OnNewGameCreatedPartialFollowUp")]
		public static bool PreFix(ref CampaignGameStarter starter, ref int index)
		{
			if (index == 0)
			{
				
				
				using (List<Clan>.Enumerator enumerator = Clan.All.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Clan clan = enumerator.Current;
						if (!clan.IsBanditFaction && !clan.IsMinorFaction && !clan.IsNeutralClan && !clan.IsEliminated && clan != Clan.PlayerClan)
						{
							List<Hero> list = new List<Hero>();
							List<Hero> list2 = new List<Hero>();
							List<Hero> list3 = new List<Hero>();
							foreach (Hero hero in clan.Lords)
							{
								if (hero.IsAlive)
								{
									if (hero.IsChild)
									{
										list.Add(hero);
									}
									else if (hero.IsFemale)
									{
										list2.Add(hero);
									}
									else
									{
										list3.Add(hero);
									}
								}
							}
							int num = MathF.Ceiling((float)(list3.Count + list2.Count) / 2f) - list.Count<Hero>();
							float num2 = 0.49f;
							if (list3.Count == 0)
							{
								num2 = -1f;
							}
							
                            
							for (int i = 0; i < num; i++)
							{
								bool isFemale = MBRandom.RandomFloat <= num2;
								Hero hero2 = isFemale ? list2.GetRandomElement<Hero>() : list3.GetRandomElement<Hero>();
								if (hero2 == null)
								{
									List<Clan> clans = new List<Clan>();
									Clan c2 = null;
                                    foreach (Clan c1 in Clan.NonBanditFactions.ToList())
                                    {
										if(c1.Culture == clan.Culture)
                                        {
											clans.Add(c1);
                                        }
                                    }
									if(clans.Count != 0)
                                    {
										c2 = clans.GetRandomElement<Clan>();
                                    }
                                    else
                                    {
										c2 = Clan.NonBanditFactions.ToList().GetRandomElement<Clan>();
                                    }
									hero2 = c2.Lords.GetRandomElement<Hero>();
								}
								if (hero2 != null)
								{
									int age = MBRandom.RandomInt(2, 18);
									Hero hero3 = HeroCreator.CreateSpecialHero(hero2.CharacterObject, clan.HomeSettlement, clan, null, age);
									hero3.UpdateHomeSettlement();
									hero3.HeroDeveloper.DeriveSkillsFromTraits(true, null);
									bool flag = hero3.Age < (float)Campaign.Current.Models.AgeModel.BecomeTeenagerAge;
									EquipmentFlags customFlags = EquipmentFlags.IsNobleTemplate | (flag ? EquipmentFlags.IsChildEquipmentTemplate : EquipmentFlags.IsTeenagerEquipmentTemplate);
									MBEquipmentRoster randomElementInefficiently = MBEquipmentRosterExtensions.GetAppropriateEquipmentRostersForHero(hero3, customFlags, true).GetRandomElementInefficiently<MBEquipmentRoster>();
									if (randomElementInefficiently != null)
									{
										Equipment randomCivilianEquipment = randomElementInefficiently.GetRandomCivilianEquipment();
										EquipmentHelper.AssignHeroEquipmentFromEquipment(hero3, randomCivilianEquipment);
										Equipment equipment = new Equipment(false);
										equipment.FillFrom(randomCivilianEquipment, false);
										EquipmentHelper.AssignHeroEquipmentFromEquipment(hero3, equipment);
									}
								}
								if (num2 <= 0f)
								{
									num2 = 0.49f;
								}
							}
							Debug.Print("Generated a child in " + clan.Name, 0, Debug.DebugColor.White, 17592186044416UL);
						}
					}
				}
			}

			return false;
		}
	}
	[HarmonyPatch]
	public static class HeroAgePatch
	{
	
		[HarmonyPrefix]
		[HarmonyPatch(typeof(AgingCampaignBehavior), "OnHeroComesOfAge")]
		public static bool PreFix(ref Hero hero)
		{
			if (hero.HeroState != Hero.CharacterStates.Active)
			{
				bool flag = !hero.IsFemale || hero.Clan == Hero.MainHero.Clan || (hero.Mother != null && !hero.Mother.IsNoncombatant) || (hero.RandomIntWithSeed(17U, 0, 1) == 0 && hero.GetTraitLevel(DefaultTraits.Valor) == 1);
				if (hero.Clan != Clan.PlayerClan)
				{
					foreach (TraitObject trait in DefaultTraits.SkillCategories)
					{
						hero.SetTraitLevel(trait, 0);
					}
					if (flag)
					{
						hero.SetTraitLevel(DefaultTraits.CavalryFightingSkills, 5);
						int value = MathF.Max(DefaultTraits.Commander.MinValue, 3 + hero.GetTraitLevel(DefaultTraits.Valor) + hero.GetTraitLevel(DefaultTraits.Generosity) + hero.RandomIntWithSeed(18U, -1, 2));
						hero.SetTraitLevel(DefaultTraits.Commander, value);
					}
					int value2 = MathF.Max(DefaultTraits.Manager.MinValue, 3 + hero.GetTraitLevel(DefaultTraits.Honor) + hero.RandomIntWithSeed(19U, -1, 2));
					hero.SetTraitLevel(DefaultTraits.Manager, value2);
					int value3 = MathF.Max(DefaultTraits.Politician.MinValue, 3 + hero.GetTraitLevel(DefaultTraits.Calculating) + hero.RandomIntWithSeed(20U, -1, 2));
					hero.SetTraitLevel(DefaultTraits.Politician, value3);
					hero.HeroDeveloper.DeriveSkillsFromTraits(true, null);
				}
				List<MBEquipmentRoster> list = new List<MBEquipmentRoster>();
				List<MBEquipmentRoster> list2 = new List<MBEquipmentRoster>();
				foreach (MBEquipmentRoster mbequipmentRoster in MBEquipmentRosterExtensions.All)
				{
					if (mbequipmentRoster.IsRosterAppropriateForHeroAsTemplate(hero, EquipmentFlags.IsNobleTemplate, false))
					{
						if (flag)
						{
							if (mbequipmentRoster.HasEquipmentFlags(EquipmentFlags.IsMediumTemplate))
							{
								list.Add(mbequipmentRoster);
							}
							if (mbequipmentRoster.HasEquipmentFlags(EquipmentFlags.IsCombatantTemplate | EquipmentFlags.IsCivilianTemplate))
							{
								list2.Add(mbequipmentRoster);
							}
						}
						else
						{
							if (mbequipmentRoster.HasEquipmentFlags(EquipmentFlags.IsMediumTemplate))
							{
								list.Add(mbequipmentRoster);
							}
							if (mbequipmentRoster.HasEquipmentFlags(EquipmentFlags.IsNoncombatantTemplate))
							{
								list2.Add(mbequipmentRoster);
							}
						}
					}
				}

				if(list.Count != 0 && list2.Count != 0)
                {
					MBEquipmentRoster randomElement = list.GetRandomElement<MBEquipmentRoster>();
					MBEquipmentRoster randomElement2 = list2.GetRandomElement<MBEquipmentRoster>();

					Equipment randomElement3 = null;
					Equipment randomElement4 = null;
					int buf = 100;
					while (randomElement3 == null && randomElement4 == null)
					{
						if (buf == 0) throw new Exception("buf exceeded");
						buf--;
						randomElement3 = randomElement.AllEquipments.GetRandomElement<Equipment>();
						randomElement4 = randomElement2.AllEquipments.GetRandomElement<Equipment>();
					}

					EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, randomElement3);
					EquipmentHelper.AssignHeroEquipmentFromEquipment(hero, randomElement4);
				}
				

			}

			return false;
		}
	}


	[HarmonyPatch]
	public static class ItemBarterBehaviorPatch
	{
	
		[HarmonyPrefix]
		[HarmonyPatch(typeof(ItemBarterBehavior), "CheckForBarters")]
		public static bool PreFix(ref BarterData args)
		{
			
			Vec2 asVec;
			if (args.OffererHero != null)
			{
				asVec = args.OffererHero.GetPosition().AsVec2;
			}
			else if (args.OffererParty != null)
			{
				asVec = args.OffererParty.MobileParty.GetPosition().AsVec2;
			}
			else
			{
				asVec = args.OtherHero.GetPosition().AsVec2;
			}


			List<Settlement> closestSettlements = new List<Settlement>();

			foreach(Settlement s in Settlement.All)
            {
				if(closestSettlements.Count != 3)
                {
					closestSettlements.Add(s);
                }
				else if(s.Position2D.Distance(asVec) < closestSettlements[0].Position2D.Distance(asVec))
                {
					closestSettlements[0] = s;
                }
				else if (s.Position2D.Distance(asVec) < closestSettlements[1].Position2D.Distance(asVec))
				{
					closestSettlements[1] = s;
				}
				else if (s.Position2D.Distance(asVec) < closestSettlements[2].Position2D.Distance(asVec))
				{
					closestSettlements[2] = s;
				}
			}


			if (args.OffererParty != null && args.OtherParty != null)
			{
				for (int i = 0; i < args.OffererParty.ItemRoster.Count; i++)
				{
					ItemRosterElement elementCopyAtIndex = args.OffererParty.ItemRoster.GetElementCopyAtIndex(i);
					if (elementCopyAtIndex.Amount > 0 && elementCopyAtIndex.EquipmentElement.GetBaseValue() > 100)
					{
						

						int num = 0;
						if (!closestSettlements.IsEmpty<Settlement>())
						{
							foreach (Settlement settlement in closestSettlements)
							{
								num += settlement.Town.GetItemPrice(elementCopyAtIndex.EquipmentElement, args.OffererParty.MobileParty, true);
							}
							num /= closestSettlements.Count;
						}
						int averageValueOfItemInNearbySettlements = num;

						Barterable barterable = new ItemBarterable(args.OffererHero, args.OtherHero, args.OffererParty, args.OtherParty, elementCopyAtIndex, averageValueOfItemInNearbySettlements);
						args.AddBarterable<ItemBarterGroup>(barterable, false);
					}
				}
				for (int j = 0; j < args.OtherParty.ItemRoster.Count; j++)
				{
					ItemRosterElement elementCopyAtIndex2 = args.OtherParty.ItemRoster.GetElementCopyAtIndex(j);
					if (elementCopyAtIndex2.Amount > 0 && elementCopyAtIndex2.EquipmentElement.GetBaseValue() > 100)
					{
						


						int num = 0;
						if (!closestSettlements.IsEmpty<Settlement>())
						{
							foreach (Settlement settlement in closestSettlements)
							{
								num += settlement.Town.GetItemPrice(elementCopyAtIndex2.EquipmentElement, args.OffererParty.MobileParty, true);
							}
							num /= closestSettlements.Count;
						}
						int averageValueOfItemInNearbySettlements2 = num;

						Barterable barterable2 = new ItemBarterable(args.OtherHero, args.OffererHero, args.OtherParty, args.OffererParty, elementCopyAtIndex2, averageValueOfItemInNearbySettlements2);
						args.AddBarterable<ItemBarterGroup>(barterable2, false);
					}
				}
			}

			return false;
		}
	}

	[HarmonyPatch]
	public static class CustomModuleHelper
	{
		
		[HarmonyPrefix]
		[HarmonyPatch(typeof(ModuleHelper), "GetXmlPath")]
		public static bool LoadGetMergedXmlForManaged(ref string moduleId, ref string xmlName, ref string __result)
		{
			bool flag = moduleId == "Native" && xmlName == "module_strings";
			bool result;
			if (flag)
			{
				__result = ModuleHelper.GetModuleFullPath("Wheel of Time Mod") + "ModuleData/module_strings_native.xml";
				result = false;
			}
			else
			{
				bool flag2 = moduleId == "Sandbox" && xmlName == "module_strings";
				if (flag2)
				{
					__result = ModuleHelper.GetModuleFullPath("Wheel of Time Mod") + "ModuleData/module_string_sandbox.xml";
					result = false;
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
	}

	[HarmonyPatch]
	public class SandboxXmlFix
    {
		[HarmonyPrefix]
		[HarmonyPatch(typeof(SandBoxManager), "InitializeSandboxXMLs")]
		public static bool LoadXml_Postfix(ref bool isSavedCampaign)
		{
			MBObjectManager.Instance.LoadXML("NPCCharacters", false);
			if (!isSavedCampaign)
			{
				string Heroespath = Path.Combine(ModuleHelper.GetModuleFullPath("Wheel of Time Mod"), "ModuleData/heroes.xml");
				string HeroesXSD = Path.Combine(ModuleHelper.GetModuleFullPath("Wheel of Time Mod"), "ModuleData/XSD/Heroes.xsd");
				MBObjectManager.Instance.LoadOneXmlFromFile(Heroespath, HeroesXSD, false);
			}
			if (Campaign.Current.GameMode == CampaignGameMode.Tutorial)
			{
				MBObjectManager.Instance.LoadXML("MPCharacters", false);
			}
			if (!isSavedCampaign)
			{
				MBObjectManager.Instance.LoadXML("Kingdoms", false);
				MBObjectManager.Instance.LoadXML("Factions", false);
			}
			MBObjectManager.Instance.LoadXML("WorkshopTypes", false);
			MBObjectManager.Instance.LoadXML("LocationComplexTemplates", false);
			if (Campaign.Current.GameMode == CampaignGameMode.Campaign && !Game.Current.IsEditModeOn)
			{
				string Heroespath = Path.Combine(ModuleHelper.GetModuleFullPath("Wheel of Time Mod"), "ModuleData/settlements.xml");
				string HeroesXSD = Path.Combine(ModuleHelper.GetModuleFullPath("Wheel of Time Mod"), "ModuleData/XSD/Settlements.xsd");
				MBObjectManager.Instance.LoadOneXmlFromFile(Heroespath, HeroesXSD, false);
			}

			
		
			return false;
		}
	}
	[HarmonyPatch(typeof(SettlementPositionScript), "SettlementsDistanceCacheFilePath", MethodType.Getter)]
	public class HarmonyOnSceneSave
	{
		// Token: 0x06000025 RID: 37 RVA: 0x000034FC File Offset: 0x000016FC
		public static bool Prefix(ref string __result, SettlementPositionScript __instance)
		{
			__result = ModuleHelper.GetModuleFullPath("Wheel of Time Mod") + "ModuleData/settlements_distance_cache.bin";
			return false;
		}
	}


	


}

	//Please look away, it's awful


	//[HarmonyPatch]
	//public static class PartyIconPatch
	//{
	//
	//	[HarmonyPrefix]
	//	[HarmonyPatch(typeof(PartyVisual), "RefreshPartyIcon")]
	//	public static bool PreFix(ref PartyBase party, ref PartyVisual __instance)
	//	{
	//
	//		bool _isDirty = (bool)__instance.GetType().GetField("_isDirty", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
	//		if (_isDirty)
	//		{
	//			_isDirty = false;
	//			bool flag = true;
	//			bool flag2 = true;
	//			if (!party.IsSettlement)
	//			{
	//				if (__instance.StrategicEntity != null)
	//				{
	//					
	//					if (__instance.GetType().GetField("_contourMaskMesh", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance) != null)
	//					{
	//						__instance.MapScreen.ContourMaskEntity.RemoveComponentWithMesh((Mesh)__instance.GetType().GetField("_contourMaskMesh", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance));
	//						__instance.GetType().GetField("_contourMaskMesh", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, null);
	//					}
	//				}
	//				if (__instance.HumanAgentVisuals != null)
	//				{
	//					__instance.HumanAgentVisuals.Reset();
	//
	//					__instance.GetType().GetField("HumanAgentVisuals").SetValue(__instance, null);
	//
	//					
	//				}
	//				if (__instance.MountAgentVisuals != null)
	//				{
	//					__instance.MountAgentVisuals.Reset();
	//					
	//					__instance.GetType().GetField("MountAgentVisuals").SetValue(__instance, null);
	//				}
	//				if (__instance.CaravanMountAgentVisuals != null)
	//				{
	//					__instance.CaravanMountAgentVisuals.Reset();
	//					
	//					__instance.GetType().GetField("CaravanMountAgentVisuals").SetValue(__instance, null);
	//				}
	//				if (__instance.StrategicEntity != null)
	//				{
	//					if ((__instance.StrategicEntity.EntityFlags & EntityFlags.Ignore) != (EntityFlags)0U)
	//					{
	//						__instance.StrategicEntity.RemoveFromPredisplayEntity();
	//					}
	//					__instance.StrategicEntity.ClearComponents();
	//				}
	//				for (int i = 0; i < 4; i++)
	//				{
	//					GameEntity[] siteEnteties = null;
	//					
	//					siteEnteties = (GameEntity[])__instance.GetType().GetField("_siteEntities", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
	//					siteEnteties[i] = null;
	//					__instance.GetType().GetField("_siteEntities", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, siteEnteties);
	//				}
	//				Campaign.Current.UnregisterFadingVisual(__instance);
	//
	//
	//
	//				MatrixFrame circleLocalFrame = __instance.CircleLocalFrame;
	//				circleLocalFrame.origin = Vec3.Zero;
	//				
	//				__instance.GetType().GetField("CircleLocalFrame").SetValue(__instance, circleLocalFrame);
	//			}
	//			else
	//			{
	//				//this.RemoveSiege();
	//				
	//				foreach (ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame> valueTuple in (List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame>>)__instance.GetType().GetField//("_siegeRangedMachineEntities", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance))
	//				{
	//					__instance.StrategicEntity.RemoveChild(valueTuple.Item1, false, false, true, 36);
	//				}
	//				
	//				foreach (ValueTuple<GameEntity, BattleSideEnum, int> valueTuple2 in (List<ValueTuple<GameEntity, BattleSideEnum, int>>)__instance.GetType().GetField("_siegeMissileEntities", BindingFlags.Instance /|/ BindingFlags.NonPublic).GetValue(__instance))
	//				{
	//					__instance.StrategicEntity.RemoveChild(valueTuple2.Item1, false, false, true, 37);
	//				}
	//				
	//				foreach (ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame> valueTuple3 in (List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame>>)__instance.GetType().GetField//("_siegeMeleeMachineEntities", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance))
	//				{
	//					__instance.StrategicEntity.RemoveChild(valueTuple3.Item1, false, false, true, 38);
	//				}
	//				List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame>> ahhhh = (List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame>>)__instance.GetType().GetField("_siegeRangedMachineEntities", //BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
	//				ahhhh.Clear();
	//				__instance.GetType().GetField("_siegeRangedMachineEntities", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, ahhhh);
	//
	//				List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame>> ahhhhh = (List<ValueTuple<GameEntity, BattleSideEnum, int, MatrixFrame>>)__instance.GetType().GetField("_siegeMeleeMachineEntities", //BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
	//				ahhhhh.Clear();
	//				__instance.GetType().GetField("_siegeMeleeMachineEntities", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, ahhhhh);
	//
	//				List<ValueTuple<GameEntity, BattleSideEnum, int>> ahhhhhg = (List<ValueTuple<GameEntity, BattleSideEnum, int>>)__instance.GetType().GetField("_siegeMissileEntities", BindingFlags.Instance | //BindingFlags.NonPublic).GetValue(__instance);
	//				ahhhhhg.Clear();
	//				__instance.GetType().GetField("_siegeMissileEntities", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, ahhhhhg);
	//
	//
	//
	//
	//				__instance.StrategicEntity.RemoveAllParticleSystems();
	//				__instance.StrategicEntity.EntityFlags |= EntityFlags.DoNotTick;
	//			}
	//			MobileParty mobileParty = party.MobileParty;
	//			if (((mobileParty != null) ? mobileParty.CurrentSettlement : null) != null)
	//			{
	//				//Dictionary<int, List<GameEntity>> gateBannerEntitiesWithLevels = ((PartyVisual)party.MobileParty.CurrentSettlement.Party.Visuals._gateBannerEntitiesWithLevels;
	//				Dictionary<int, List<GameEntity>> gateBannerEntitiesWithLevels = (Dictionary<int, List<GameEntity>>)__instance.GetType().GetField("_gateBannerEntitiesWithLevels", BindingFlags.Instance | //BindingFlags.NonPublic).GetValue(__instance);
	//				if (!party.MobileParty.MapFaction.IsAtWarWith(party.MobileParty.CurrentSettlement.MapFaction) && gateBannerEntitiesWithLevels != null && !gateBannerEntitiesWithLevels.IsEmpty<KeyValuePair<int, //List<GameEntity>>>())
	//				{
	//					Hero leaderHero = party.LeaderHero;
	//					if (((leaderHero != null) ? leaderHero.ClanBanner : null) != null)
	//					{
	//						string text = party.LeaderHero.ClanBanner.Serialize();
	//						if (string.IsNullOrEmpty(text))
	//						{
	//							
	//						}
	//						int num = 0;
	//						foreach (MobileParty mobileParty2 in party.MobileParty.CurrentSettlement.Parties)
	//						{
	//							if (mobileParty2 == party.MobileParty)
	//							{
	//								break;
	//							}
	//							Hero leaderHero2 = mobileParty2.LeaderHero;
	//							if (((leaderHero2 != null) ? leaderHero2.ClanBanner : null) != null)
	//							{
	//								num++;
	//							}
	//						}
	//						MatrixFrame matrixFrame = MatrixFrame.Identity;
	//						int wallLevel = party.MobileParty.CurrentSettlement.Town.GetWallLevel();
	//						int count = gateBannerEntitiesWithLevels[wallLevel].Count;
	//						if (count == 0)
	//						{
	//							Debug.FailedAssert(string.Format("{0} - has no Banner Entities at level {1}.", party.MobileParty.CurrentSettlement.Name, wallLevel), "C:\\Develop\\MB3\\Source\\Bannerlord\\SandBox.View/\/\Map\\PartyVisual.cs", "RefreshPartyIcon", 1045);
	//						}
	//						GameEntity gameEntity = gateBannerEntitiesWithLevels[wallLevel][num % count];
	//						GameEntity child = gameEntity.GetChild(0);
	//						MatrixFrame matrixFrame2 = (child != null) ? child.GetGlobalFrame() : gameEntity.GetGlobalFrame();
	//						num /= count;
	//						int num2 = party.MobileParty.CurrentSettlement.Parties.Count(delegate (MobileParty p)
	//						{
	//							Hero leaderHero3 = p.LeaderHero;
	//							return ((leaderHero3 != null) ? leaderHero3.ClanBanner : null) != null;
	//						});
	//						float f = 0.75f / (float)MathF.Max(1, num2 / (count * 2));
	//						int num3 = (num % 2 == 0) ? -1 : 1;
	//						Vec3 v = matrixFrame2.rotation.f / 2f * (float)num3;
	//						if (v.Length < matrixFrame2.rotation.s.Length)
	//						{
	//							v = matrixFrame2.rotation.s / 2f * (float)num3;
	//						}
	//						matrixFrame.origin = matrixFrame2.origin + v * (float)((num + 1) / 2) * (float)(num % 2 * 2 - 1) * f * (float)num3;
	//						MatrixFrame globalFrame = __instance.StrategicEntity.GetGlobalFrame();
	//						matrixFrame.origin = globalFrame.TransformToLocal(matrixFrame.origin);
	//						float num4 = MBMath.Map((float)party.NumberOfAllMembers / 400f * ((party.MobileParty.Army != null && party.MobileParty.Army.LeaderParty == party.MobileParty) ? 1.25f : 1f), 0f, 1f, /0.2f, /0.5f);
	//						matrixFrame = matrixFrame.Elevate(-num4);
	//						matrixFrame.rotation.ApplyScaleLocal(num4);
	//						globalFrame = __instance.StrategicEntity.GetGlobalFrame();
	//						matrixFrame.rotation = globalFrame.rotation.TransformToLocal(matrixFrame.rotation);
	//						__instance.StrategicEntity.AddSphereAsBody(matrixFrame.origin + Vec3.Up * 0.3f, 0.15f, BodyFlags.None);
	//						flag = false;
	//						string text2 = "campaign_flag";
	//						ValueTuple<string, GameEntityComponent> cacheBannerComponent = (ValueTuple<string, GameEntityComponent>)__instance.GetType().GetField("_cachedBannerComponent", BindingFlags.Instance | //BindingFlags.NonPublic).GetValue(__instance);
	//						if (cacheBannerComponent.Item1 == text + text2)
	//						{
	//							cacheBannerComponent.Item2.GetFirstMetaMesh().Frame = matrixFrame;
	//							__instance.StrategicEntity.AddComponent(cacheBannerComponent.Item2);
	//							
	//						}
	//						//mehtod start
	//						MetaMesh bannerOfCharacter = null;
	//
	//						MetaMesh copy = MetaMesh.GetCopy(text2, true, false);
	//						for (int i = 0; i < copy.MeshCount; i++)
	//						{
	//							Mesh meshAtIndex = copy.GetMeshAtIndex(i);
	//							if (!meshAtIndex.HasTag("dont_use_tableau"))
	//							{
	//								Material material = meshAtIndex.GetMaterial();
	//								Material tableauMaterial = null;
	//								Tuple<Material, BannerCode> key = new Tuple<Material, BannerCode>(material, BannerCode.CreateFrom(text));
	//								if (MapScreen.Instance._characterBannerMaterialCache.ContainsKey(key))
	//								{
	//									tableauMaterial = MapScreen.Instance._characterBannerMaterialCache[key];
	//								}
	//								else
	//								{
	//									tableauMaterial = material.CreateCopy();
	//									Action<Texture> setAction = delegate (Texture tex)
	//									{
	//										tableauMaterial.SetTexture(Material.MBTextureType.DiffuseMap2, tex);
	//										uint num1= (uint)tableauMaterial.GetShader().GetMaterialShaderFlagMask("use_tableau_blending", true);
	//										ulong shaderFlags = tableauMaterial.GetShaderFlags();
	//										tableauMaterial.SetShaderFlags(shaderFlags | (ulong)num1);
	//									};
	//									Banner b = new Banner(text);
	//									b.GetTableauTextureLarge(setAction);
	//									MapScreen.Instance._characterBannerMaterialCache[key] = tableauMaterial;
	//								}
	//								meshAtIndex.SetMaterial(tableauMaterial);
	//							}
	//						}
	//						bannerOfCharacter = copy;
	//						//method end
	//
	//
	//						bannerOfCharacter.Frame = matrixFrame;
	//						int componentCount = __instance.StrategicEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator);
	//						__instance.StrategicEntity.AddMultiMesh(bannerOfCharacter, true);
	//						if (__instance.StrategicEntity.GetComponentCount(GameEntity.ComponentType.ClothSimulator) > componentCount)
	//						{
	//							cacheBannerComponent.Item1 = text + text2;
	//							cacheBannerComponent.Item2 = __instance.StrategicEntity.GetComponentAtIndex(componentCount, GameEntity.ComponentType.ClothSimulator);
	//							
	//						}
	//						
	//					}
	//				}
	//				__instance.StrategicEntity.RemovePhysics(false);
	//			}
	//			else
	//			{
	//
	//			
	//
	//
	//				
	//
	//				if (__instance.StrategicEntity != null && party.IsMobile)
	//				{
	//					__instance.StrategicEntity.AddSphereAsBody(new Vec3(0f, 0f, 0f, -1f), 0.5f, BodyFlags.Moveable | BodyFlags.OnlyCollideWithRaycast);
	//				}
	//
	//				if (party.IsSettlement && party.Settlement != null)
	//				{
	//					if (party.Settlement.IsFortification)
	//					{
	//						GameEntity.UpgradeLevelMask eee = (GameEntity.UpgradeLevelMask)__instance.GetType().GetField("_currentSettlementUpgradeLevelMask", BindingFlags.Instance | BindingFlags.NonPublic).GetValue//(__instance);
	//
	//						__instance.GetType().GetField("_currentLevelBreachableWallEntities", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, (
	//							from e in (GameEntity[])__instance.GetType().GetField("_breachableWallEntities", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)
	//							where (e.GetUpgradeLevelMask() & eee) == eee
	//							select e).ToArray<GameEntity>());
	//
	//						
	//					}
	//					object[] o1 = new object[1];
	//					object[] o2 = new object[0];
	//					o1[0] = party;
	//					__instance.GetType().GetMethod("AddSiegeIconComponents", BindingFlags.NonPublic).Invoke(__instance, o1);
	//					__instance.GetType().GetMethod("SetSettlementLevelVisibility", BindingFlags.NonPublic).Invoke(__instance, o2);
	//					__instance.GetType().GetMethod("RefreshWallState", BindingFlags.NonPublic).Invoke(__instance, o1);
	//					__instance.GetType().GetMethod("RefreshTownPhysicalEntitiesState", BindingFlags.NonPublic).Invoke(__instance, o1);
	//					__instance.GetType().GetMethod("RefreshSiegePreperations", BindingFlags.NonPublic).Invoke(__instance, o1);
	//
	//					
	//					if (party.Settlement.IsVillage)
	//					{
	//						MapEvent mapEvent = party.MapEvent;
	//						if (mapEvent != null && mapEvent.IsRaid)
	//						{
	//							__instance.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
	//							__instance.StrategicEntity.AddParticleSystemComponent("psys_fire_smoke_env_point");
	//						}
	//						else if (party.Settlement.IsRaided)
	//						{
	//							__instance.StrategicEntity.EntityFlags &= ~EntityFlags.DoNotTick;
	//							__instance.StrategicEntity.AddParticleSystemComponent("map_icon_village_plunder_fx");
	//						}
	//						SoundEvent seve = (SoundEvent)__instance.GetType().GetField("_raidedSoundEvent", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
	//						if (party.Settlement.IsRaided && seve != null)
	//						{
	//							__instance.GetType().GetField("_raidedSoundEvent", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance, SoundEvent.CreateEventFromString("event:/map/ambient/node///burning_village", (Scene)__instance.GetType().GetField("MapScene", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance)));
	//							
	//							seve.SetParameter("battle_size", 4f);
	//							seve.SetPosition(party.Settlement.GetPosition());
	//							seve.Play();
	//						}
	//						else if (!party.Settlement.IsRaided && seve != null)
	//						{
	//							seve.Stop();
	//							__instance.GetType().GetField("_raidedSoundEvent", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(__instance,null);
	//						}
	//					}
	//				}
	//				else
	//				{
	//					object[] o3 = new object[3];
	//					
	//				}
	//			}
	//
	//			__instance.StrategicEntity.CheckResources(true, false);
	//		}
	//
	//		return false;
	//	}
	//}




