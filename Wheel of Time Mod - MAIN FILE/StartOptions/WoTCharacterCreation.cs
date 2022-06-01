using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CharacterCreationContent
{
    
    public class WoTCharacterCreation : CharacterCreationContentBase
	{

		//Code copied from taleworld's binarys, strings, locations etc. can be changed

		public override TextObject ReviewPageDescription
		{
			get
			{
				return new TextObject("{=W6pKpEoT}You prepare to set off for a grand adventure! Here is your character. Continue if you are ready, or go back to make changes.", null);
			}
		}

		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x000958F6 File Offset: 0x00093AF6
		public override IEnumerable<Type> CharacterCreationStages
		{
			get
			{
				//yield return typeof(CharacterCreationCultureStage);
				yield return typeof(CharacterCreationFaceGeneratorStage);
				yield return typeof(CharacterCreationGenericStage);
				yield return typeof(CharacterCreationBannerEditorStage);
				yield return typeof(CharacterCreationClanNamingStage);
				yield return typeof(CharacterCreationReviewStage);
				yield return typeof(CharacterCreationOptionsStage);
				yield break;
			}
		}

		protected override void OnCultureSelected()
		{
			base.SelectedTitleType = 1;
			base.SelectedParentType = 0;
			Clan.PlayerClan.ChangeClanName(FactionHelper.GenerateClanNameforPlayer());
		}

		
		public override int GetSelectedParentType()
		{
			return base.SelectedParentType;
		}

		
		public override void OnCharacterCreationFinalized()
		{
			CultureObject culture = CharacterObject.PlayerCharacter.Culture;
			Vec2 position2D;
			if (this._startingPoints.TryGetValue(culture.StringId, out position2D))
			{
				MobileParty.MainParty.Position2D = position2D;
			}
			else
			{
				MobileParty.MainParty.Position2D = Campaign.Current.DefaultStartingPosition;
				Debug.FailedAssert("Selected culture is not in the dictionary!", "C:\\Develop\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\WoTCharacterCreation.cs", "OnCharacterCreationFinalized", 103);
			}
			MapState mapState;
			if ((mapState = (GameStateManager.Current.ActiveState as MapState)) != null)
			{
				mapState.Handler.ResetCamera();
				mapState.Handler.TeleportCameraToMainParty();
			}
			this.SetHeroAge((float)this._startingAge);
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000959C2 File Offset: 0x00093BC2
		protected override void OnInitialized(CharacterCreation characterCreation)
		{
			this.AddTestMenu(characterCreation);
			//this.AddOriginMenu(characterCreation);
			//this.AddLifeMenu(characterCreation);
			//this.AddAgeSelectionMenu(characterCreation);
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000959EE File Offset: 0x00093BEE
		protected override void OnApplyCulture()
		{
		}

		protected void AddTestMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Origin", null), new TextObject("You were...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);

			characterCreationCategory.AddCategoryOption(new TextObject("test", null), new List<SkillObject>
				{
				DefaultSkills.Athletics,
				DefaultSkills.TwoHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodYourLeadershipSkillsOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodLeadingOnApply), new TextObject("testtesttest", null), null, 0, 0, 0, 0, 0);
			characterCreation.AddNewMenu(characterCreationMenu);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x00096A18 File Offset: 0x00094C18
		protected void AddOriginMenu(CharacterCreation characterCreation)
		{
			CultureObject culture = null;

			CultureObject[] cultures = base.GetCultures().ToArray();
			foreach(CultureObject culture1 in cultures)
            {
				if(culture1.Name.ToString() == "sturgia")
                {
					culture = culture1;
                }
            }

			base.SetSelectedCulture(culture, characterCreation);

			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Origin", null), new TextObject("You were...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);

			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			//if (base.GetSelectedCulture().Id.ToString() == "sturgia")
            //{
			//	
			//	
			//	characterCreationCategory.AddCategoryOption(new TextObject("kidnapped as a child by the shadowspawn.", null), new List<SkillObject>
			//	{
			//	DefaultSkills.Athletics,
			//	DefaultSkills.TwoHanded
			//	}, 
			//	DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodYourLeadershipSkillsOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodLeadingOnApply), new TextObject("As a young child your village was raided by a hord of trollocs led by a ruthless myrdaal, but they were not there to kill. At least not the children. They took you and all who couldn't hold a spear and defend themselves to the nearest city of the shadow. Those who survived the long journey and your hard life there became involuntary darkfriends, forced to kill in the name of the Dark One against their will.", null), null, 0, 0, 0, 0, 0);
			//
			//	//---------------------------
			//	characterCreationCategory.AddCategoryOption(new TextObject(" a child of darkfriends.", null), new List<SkillObject>
			//	{
			//	DefaultSkills.Roguery,
			//	DefaultSkills.Riding
			//	}, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodYourBrawnOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodAthleticsOnApply), new TextObject("You were born to a family long in the shadows service. Nowing nothing but the Dark One as your master you became like all your kin, a darkfriend.", null), null, 0, 0, 0, 0, 0);
			//
			//	//--------------------------
			//
			//	characterCreationCategory.AddCategoryOption(new TextObject(" a result of human and animals breeding, a trolloc.", null), new List<SkillObject>
			//	{
			//	DefaultSkills.Athletics,
			//	DefaultSkills.Polearm
			//	}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodAttentionToDetailOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodMemoryOnApply), new TextObject("Your mind is weak, but your fury and muscle great, a trolloc, a beast designed to kill and ravage in the Dark Ones name.", null), null, 0, 0, 0, 0, 0);
			//
			//	//_____________________
			//
			//	
			//	characterCreationCategory.AddCategoryOption(new TextObject("{=Y3UcaX74}your aptitude for numbers.", null), new List<SkillObject>
			//	{
			//	DefaultSkills.Engineering,
			//	DefaultSkills.Trade
			//	}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodAptitudeForNumbersOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodMathOnApply), new TextObject("{=DFidSjIf}Most children around you had only the most rudimentary education, but you lingered after class to study letters and mathematics. You were fascinated by the marketplace - weights and measures, tallies and accounts, the chatter about profits and losses.", null), null, 0, 0, 0, 0, 0);
			//	characterCreationCategory.AddCategoryOption(new TextObject("{=GEYzLuwb}your way with people.", null), new List<SkillObject>
			//	{
			//	DefaultSkills.Charm,
			//	DefaultSkills.Leadership
			//	}, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodWayWithPeopleOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodMannersOnApply), new TextObject("{=w2TEQq26}You were always attentive to other people, good at guessing their motivations. You studied how individuals were swayed, and tried out what you learned from adults on your friends.", null), null, 0, 0, 0, 0, 0);
			//	characterCreationCategory.AddCategoryOption(new TextObject("{=MEgLE2kj}your skill with horses.", null), new List<SkillObject>
			//	{
			//	DefaultSkills.Riding,
			//	DefaultSkills.Medicine
			//	}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodSkillsWithHorsesOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodAffinityWithAnimalsOnApply), new TextObject("{=ngazFofr}You were always drawn to animals, and spent as much time as possible hanging out in the village stables. You could calm horses, and were sometimes called upon to break in new colts. You learned the basics of veterinary arts, much of which is applicable to humans as well.", null), null, 0, 0, 0, 0, 0);
			//}
			
			characterCreationCategory.AddCategoryOption(new TextObject("kidnapped as a child by the shadowspawn.", null), new List<SkillObject>
				{
				DefaultSkills.Athletics,
				DefaultSkills.TwoHanded
				},
			DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodYourLeadershipSkillsOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodLeadingOnApply), new TextObject("As a young child your village was raided by a hord of trollocs led by a ruthless myrdaal, but they were not there to kill. At least not the children. They took you and all who couldn't hold a spear and defend themselves to the nearest city of the shadow. Those who survived the long journey and your hard life there became involuntary darkfriends, forced to kill in the name of the Dark One against their will.", null), null, 0, 0, 0, 0, 0);

			//---------------------------
			characterCreationCategory.AddCategoryOption(new TextObject(" a child of darkfriends.", null), new List<SkillObject>
				{
				DefaultSkills.Roguery,
				DefaultSkills.Riding
				}, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodYourBrawnOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodAthleticsOnApply), new TextObject("You were born to a family long in the shadows service. Nowing nothing but the Dark One as your master you became like all your kin, a darkfriend.", null), null, 0, 0, 0, 0, 0);

			//--------------------------

			characterCreationCategory.AddCategoryOption(new TextObject(" a result of human and animals breeding, a trolloc.", null), new List<SkillObject>
				{
				DefaultSkills.Athletics,
				DefaultSkills.Polearm
				}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodAttentionToDetailOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodMemoryOnApply), new TextObject("Your mind is weak, but your fury and muscle great, a trolloc, a beast designed to kill and ravage in the Dark Ones name.", null), null, 0, 0, 0, 0, 0);

			//_____________________


			characterCreationCategory.AddCategoryOption(new TextObject("{=Y3UcaX74}your aptitude for numbers.", null), new List<SkillObject>
				{
				DefaultSkills.Engineering,
				DefaultSkills.Trade
				}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodAptitudeForNumbersOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodMathOnApply), new TextObject("{=DFidSjIf}Most children around you had only the most rudimentary education, but you lingered after class to study letters and mathematics. You were fascinated by the marketplace - weights and measures, tallies and accounts, the chatter about profits and losses.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=GEYzLuwb}your way with people.", null), new List<SkillObject>
				{
				DefaultSkills.Charm,
				DefaultSkills.Leadership
				}, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodWayWithPeopleOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodMannersOnApply), new TextObject("{=w2TEQq26}You were always attentive to other people, good at guessing their motivations. You studied how individuals were swayed, and tried out what you learned from adults on your friends.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=MEgLE2kj}your skill with horses.", null), new List<SkillObject>
				{
				DefaultSkills.Riding,
				DefaultSkills.Medicine
				}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodSkillsWithHorsesOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodAffinityWithAnimalsOnApply), new TextObject("{=ngazFofr}You were always drawn to animals, and spent as much time as possible hanging out in the village stables. You could calm horses, and were sometimes called upon to break in new colts. You learned the basics of veterinary arts, much of which is applicable to humans as well.", null), null, 0, 0, 0, 0, 0);


			characterCreation.AddNewMenu(characterCreationMenu);
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x00096CEC File Offset: 0x00094EEC
		

		// Token: 0x0600257E RID: 9598 RVA: 0x00097A80 File Offset: 0x00095C80
		protected void AddLifeMenu(CharacterCreation characterCreation)
		{
			MBTextManager.SetTextVariable("EXP_VALUE", this.SkillLevelToAdd);
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=MafIe9yI}Young Adulthood", null), new TextObject("{=4WYY0X59}Before you set out for a life of adventure, your biggest achievement was...", null), new CharacterCreationOnInit(this.AccomplishmentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			characterCreationCategory.AddCategoryOption(new TextObject("{=8bwpVpgy}you defeated an enemy in battle.", null), new List<SkillObject>
			{
				DefaultSkills.OneHanded,
				DefaultSkills.TwoHanded
			}, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentDefeatedEnemyOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentDefeatedEnemyOnApply), new TextObject("{=1IEroJKs}Not everyone who musters for the levy marches to war, and not everyone who goes on campaign sees action. You did both, and you also took down an enemy warrior in direct one-to-one combat, in the full view of your comrades.", null), new List<TraitObject>
			{
				DefaultTraits.Valor
			}, 1, 20, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=mP3uFbcq}you led a successful manhunt.", null), new List<SkillObject>
			{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentPosseOnConditions), new CharacterCreationOnSelect(this.AccomplishmentExpeditionOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=4f5xwzX0}When your community needed to organize a posse to pursue horse thieves, you were the obvious choice. You hunted down the raiders, surrounded them and forced their surrender, and took back your stolen property.", null), new List<TraitObject>
			{
				DefaultTraits.Calculating
			}, 1, 10, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=wfbtS71d}you led a caravan.", null), new List<SkillObject>
			{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentMerchantOnCondition), new CharacterCreationOnSelect(this.AccomplishmentMerchantOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=joRHKCkm}Your family needed someone trustworthy to take a caravan to a neighboring town. You organized supplies, ensured a constant watch to keep away bandits, and brought it safely to its destination.", null), new List<TraitObject>
			{
				DefaultTraits.Calculating
			}, 1, 10, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=x1HTX5hq}you saved your village from a flood.", null), new List<SkillObject>
			{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentSavedVillageOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSavedVillageOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=bWlmGDf3}When a sudden storm caused the local stream to rise suddenly, your neighbors needed quick-thinking leadership. You provided it, directing them to build levees to save their homes.", null), new List<TraitObject>
			{
				DefaultTraits.Calculating
			}, 1, 10, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=s8PNllPN}you saved your city quarter from a fire.", null), new List<SkillObject>
			{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentSavedStreetOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSavedStreetOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentExpeditionOnApply), new TextObject("{=ZAGR6PYc}When a sudden blaze broke out in a back alley, your neighbors needed quick-thinking leadership and you provided it. You organized a bucket line to the nearest well, putting the fire out before any homes were lost.", null), new List<TraitObject>
			{
				DefaultTraits.Calculating
			}, 1, 10, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=xORjDTal}you invested some money in a workshop.", null), new List<SkillObject>
			{
				DefaultSkills.Trade,
				DefaultSkills.Crafting
			}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(this.AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=PyVqDLBu}Your parents didn't give you much money, but they did leave just enough for you to secure a loan against a larger amount to build a small workshop. You paid back what you borrowed, and sold your enterprise for a profit.", null), new List<TraitObject>
			{
				DefaultTraits.Calculating
			}, 1, 10, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=xKXcqRJI}you invested some money in land.", null), new List<SkillObject>
			{
				DefaultSkills.Trade,
				DefaultSkills.Crafting
			}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(this.AccomplishmentWorkshopOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentWorkshopOnApply), new TextObject("{=cbF9jdQo}Your parents didn't give you much money, but they did leave just enough for you to purchase a plot of unused land at the edge of the village. You cleared away rocks and dug an irrigation ditch, raised a few seasons of crops, than sold it for a considerable profit.", null), new List<TraitObject>
			{
				DefaultTraits.Calculating
			}, 1, 10, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=TbNRtUjb}you hunted a dangerous animal.", null), new List<SkillObject>
			{
				DefaultSkills.Polearm,
				DefaultSkills.Crossbow
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=I3PcdaaL}Wolves, bears are a constant menace to the flocks of northern Calradia, while hyenas and leopards trouble the south. You went with a group of your fellow villagers and fired the missile that brought down the beast.", null), null, 0, 5, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=WbHfGCbd}you survived a siege.", null), new List<SkillObject>
			{
				DefaultSkills.Bow,
				DefaultSkills.Crossbow
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(this.AccomplishmentSiegeHunterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentSiegeHunterOnApply), new TextObject("{=FhZPjhli}Your hometown was briefly placed under siege, and you were called to defend the walls. Everyone did their part to repulse the enemy assault, and everyone is justly proud of what they endured.", null), null, 0, 5, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=kNXet6Um}you had a famous escapade in town.", null), new List<SkillObject>
			{
				DefaultSkills.Athletics,
				DefaultSkills.Roguery
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentRuralOnCondition), new CharacterCreationOnSelect(this.AccomplishmentEscapadeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentEscapadeOnApply), new TextObject("{=DjeAJtix}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, on one of your trips into town you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new List<TraitObject>
			{
				DefaultTraits.Valor
			}, 1, 5, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=qlOuiKXj}you had a famous escapade.", null), new List<SkillObject>
			{
				DefaultSkills.Athletics,
				DefaultSkills.Roguery
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.AccomplishmentUrbanOnCondition), new CharacterCreationOnSelect(this.AccomplishmentEscapadeOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentEscapadeOnApply), new TextObject("{=lD5Ob3R4}Maybe it was a love affair, or maybe you cheated at dice, or maybe you just chose your words poorly when drinking with a dangerous crowd. Anyway, you got into the kind of trouble from which only a quick tongue or quick feet get you out alive.", null), new List<TraitObject>
			{
				DefaultTraits.Valor
			}, 1, 5, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=Yqm0Dics}you treated people well.", null), new List<SkillObject>
			{
				DefaultSkills.Charm,
				DefaultSkills.Steward
			}, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.AccomplishmentTreaterOnConsequence), new CharacterCreationApplyFinalEffects(this.AccomplishmentTreaterOnApply), new TextObject("{=dDmcqTzb}Yours wasn't the kind of reputation that local legends are made of, but it was the kind that wins you respect among those around you. You were consistently fair and honest in your business dealings and helpful to those in trouble. In doing so, you got a sense of what made people tick.", null), new List<TraitObject>
			{
				DefaultTraits.Mercy,
				DefaultTraits.Generosity,
				DefaultTraits.Honor
			}, 1, 5, 0, 0, 0);
			characterCreation.AddNewMenu(characterCreationMenu);
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x00098110 File Offset: 0x00096310
		protected void AddAgeSelectionMenu(CharacterCreation characterCreation)
		{
			MBTextManager.SetTextVariable("EXP_VALUE", this.SkillLevelToAdd);
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=HDFEAYDk}Starting Age", null), new TextObject("{=VlOGrGSn}Your character started off on the adventuring path at the age of...", null), new CharacterCreationOnInit(this.StartingAgeOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			characterCreationCategory.AddCategoryOption(new TextObject("{=!}20", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeYoungOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeYoungOnApply), new TextObject("{=2k7adlh7}While lacking experience a bit, you are full with youthful energy, you are fully eager, for the long years of adventuring ahead.", null), null, 0, 0, 0, 2, 1);
			characterCreationCategory.AddCategoryOption(new TextObject("{=!}30", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeAdultOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeAdultOnApply), new TextObject("{=NUlVFRtK}You are at your prime, You still have some youthful energy but also have a substantial amount of experience under your belt. ", null), null, 0, 0, 0, 4, 2);
			characterCreationCategory.AddCategoryOption(new TextObject("{=!}40", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeMiddleAgedOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeMiddleAgedOnApply), new TextObject("{=5MxTYApM}This is the right age for starting off, you have years of experience, and you are old enough for people to respect you and gather under your banner.", null), null, 0, 0, 0, 6, 3);
			characterCreationCategory.AddCategoryOption(new TextObject("{=!}50", null), new List<SkillObject>(), null, 0, 0, 0, null, new CharacterCreationOnSelect(this.StartingAgeElderlyOnConsequence), new CharacterCreationApplyFinalEffects(this.StartingAgeElderlyOnApply), new TextObject("{=ePD5Afvy}While you are past your prime, there is still enough time to go on that last big adventure for you. And you have all the experience you need to overcome anything!", null), null, 0, 0, 0, 8, 4);
			characterCreation.AddNewMenu(characterCreationMenu);
		}

		

		protected void TestOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this._startingAge, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			this.RefreshPlayerAppearance(characterCreation);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x00098274 File Offset: 0x00096474
		protected void ParentsOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = false;
			characterCreation.HasSecondaryCharacter = false;
			WoTCharacterCreation.ClearMountEntity(characterCreation);
			characterCreation.ClearFaceGenPrefab();
			if (base.PlayerBodyProperties != CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1))
			{
				base.PlayerBodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
				BodyProperties playerBodyProperties = base.PlayerBodyProperties;
				BodyProperties playerBodyProperties2 = base.PlayerBodyProperties;
				FaceGen.GenerateParentKey(base.PlayerBodyProperties, ref playerBodyProperties, ref playerBodyProperties2);
				playerBodyProperties = new BodyProperties(new DynamicBodyProperties(33f, 0.3f, 0.2f), playerBodyProperties.StaticProperties);
				playerBodyProperties2 = new BodyProperties(new DynamicBodyProperties(33f, 0.5f, 0.5f), playerBodyProperties2.StaticProperties);
				base.MotherFacegenCharacter = new FaceGenChar(playerBodyProperties, new Equipment(), true, "anim_mother_1");
				base.FatherFacegenCharacter = new FaceGenChar(playerBodyProperties2, new Equipment(), false, "anim_father_1");
			}
			characterCreation.ChangeFaceGenChars(new List<FaceGenChar>
			{
				base.MotherFacegenCharacter,
				base.FatherFacegenCharacter
			});
			this.ChangeParentsOutfit(characterCreation, "", "", true, true);
			this.ChangeParentsAnimation(characterCreation);
		}

		// Token: 0x06002581 RID: 9601 RVA: 0x000983A8 File Offset: 0x000965A8
		protected void ChangeParentsOutfit(CharacterCreation characterCreation, string fatherItemId = "", string motherItemId = "", bool isLeftHandItemForFather = true, bool isLeftHandItemForMother = true)
		{
			characterCreation.ClearFaceGenPrefab();
			List<Equipment> list = new List<Equipment>();
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new object[]
			{
				"mother_char_creation_",
				base.SelectedParentType,
				"_",
				base.GetSelectedCulture().StringId
			}));
			Equipment equipment = ((@object != null) ? @object.DefaultEquipment : null) ?? MBEquipmentRoster.EmptyEquipment;
			MBEquipmentRoster object2 = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(string.Concat(new object[]
			{
				"father_char_creation_",
				base.SelectedParentType,
				"_",
				base.GetSelectedCulture().StringId
			}));
			Equipment equipment2 = ((object2 != null) ? object2.DefaultEquipment : null) ?? MBEquipmentRoster.EmptyEquipment;
			if (motherItemId != "")
			{
				ItemObject object3 = Game.Current.ObjectManager.GetObject<ItemObject>(motherItemId);
				if (object3 != null)
				{
					equipment.AddEquipmentToSlotWithoutAgent(isLeftHandItemForMother ? EquipmentIndex.WeaponItemBeginSlot : EquipmentIndex.Weapon1, new EquipmentElement(object3, null, null, false));
				}
				else
				{
					characterCreation.ChangeCharacterPrefab(motherItemId, isLeftHandItemForMother ? Game.Current.HumanMonster.MainHandItemBoneIndex : Game.Current.HumanMonster.OffHandItemBoneIndex);
				}
			}
			if (fatherItemId != "")
			{
				ItemObject object4 = Game.Current.ObjectManager.GetObject<ItemObject>(fatherItemId);
				if (object4 != null)
				{
					equipment2.AddEquipmentToSlotWithoutAgent(isLeftHandItemForFather ? EquipmentIndex.WeaponItemBeginSlot : EquipmentIndex.Weapon1, new EquipmentElement(object4, null, null, false));
				}
			}
			list.Add(equipment);
			list.Add(equipment2);
			characterCreation.ChangeCharactersEquipment(list);
		}

		// Token: 0x06002582 RID: 9602 RVA: 0x00098530 File Offset: 0x00096730
		protected void ChangeParentsAnimation(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"anim_mother_" + base.SelectedParentType,
				"anim_father_" + base.SelectedParentType
			});
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x00098580 File Offset: 0x00096780
		protected void SetParentAndOccupationType(CharacterCreation characterCreation, int parentType, WoTCharacterCreation.OccupationTypes occupationType, string fatherItemId = "", string motherItemId = "", bool isLeftHandItemForFather = true, bool isLeftHandItemForMother = true)
		{
			base.SelectedParentType = parentType;
			this._familyOccupationType = occupationType;
			characterCreation.ChangeFaceGenChars(new List<FaceGenChar>
			{
				base.MotherFacegenCharacter,
				base.FatherFacegenCharacter
			});
			this.ChangeParentsAnimation(characterCreation);
			this.ChangeParentsOutfit(characterCreation, fatherItemId, motherItemId, isLeftHandItemForFather, isLeftHandItemForMother);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000985D4 File Offset: 0x000967D4
		protected void EmpireLandlordsRetainerOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 1, WoTCharacterCreation.OccupationTypes.Retainer, "", "", true, true);
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000985EB File Offset: 0x000967EB
		protected void EmpireMerchantOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 2, WoTCharacterCreation.OccupationTypes.Merchant, "", "", true, true);
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x00098602 File Offset: 0x00096802
		protected void EmpireFreeholderOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 3, WoTCharacterCreation.OccupationTypes.Farmer, "", "", true, true);
		}

		// Token: 0x06002587 RID: 9607 RVA: 0x00098619 File Offset: 0x00096819
		protected void EmpireArtisanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 4, WoTCharacterCreation.OccupationTypes.Artisan, "", "", true, true);
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x00098630 File Offset: 0x00096830
		protected void EmpireWoodsmanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 5, WoTCharacterCreation.OccupationTypes.Hunter, "", "", true, true);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x00098647 File Offset: 0x00096847
		protected void EmpireVagabondOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 6, WoTCharacterCreation.OccupationTypes.Vagabond, "", "", true, true);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x0009865E File Offset: 0x0009685E
		protected void EmpireLandlordsRetainerOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x00098666 File Offset: 0x00096866
		protected void EmpireMerchantOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600258C RID: 9612 RVA: 0x0009866E File Offset: 0x0009686E
		protected void EmpireFreeholderOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600258D RID: 9613 RVA: 0x00098676 File Offset: 0x00096876
		protected void EmpireArtisanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600258E RID: 9614 RVA: 0x0009867E File Offset: 0x0009687E
		protected void EmpireWoodsmanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600258F RID: 9615 RVA: 0x00098686 File Offset: 0x00096886
		protected void EmpireVagabondOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x06002590 RID: 9616 RVA: 0x0009868E File Offset: 0x0009688E
		protected void VlandiaBaronsRetainerOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 1, WoTCharacterCreation.OccupationTypes.Retainer, "", "", true, true);
		}

		// Token: 0x06002591 RID: 9617 RVA: 0x000986A5 File Offset: 0x000968A5
		protected void VlandiaMerchantOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 2, WoTCharacterCreation.OccupationTypes.Merchant, "", "", true, true);
		}

		// Token: 0x06002592 RID: 9618 RVA: 0x000986BC File Offset: 0x000968BC
		protected void VlandiaYeomanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 3, WoTCharacterCreation.OccupationTypes.Farmer, "", "", true, true);
		}

		// Token: 0x06002593 RID: 9619 RVA: 0x000986D3 File Offset: 0x000968D3
		protected void VlandiaBlacksmithOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 4, WoTCharacterCreation.OccupationTypes.Artisan, "", "", true, true);
		}

		// Token: 0x06002594 RID: 9620 RVA: 0x000986EA File Offset: 0x000968EA
		protected void VlandiaHunterOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 5, WoTCharacterCreation.OccupationTypes.Hunter, "", "", true, true);
		}

		// Token: 0x06002595 RID: 9621 RVA: 0x00098701 File Offset: 0x00096901
		protected void VlandiaMercenaryOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 6, WoTCharacterCreation.OccupationTypes.Mercenary, "", "", true, true);
		}

		// Token: 0x06002596 RID: 9622 RVA: 0x00098718 File Offset: 0x00096918
		protected void VlandiaBaronsRetainerOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x06002597 RID: 9623 RVA: 0x00098720 File Offset: 0x00096920
		protected void VlandiaMerchantOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x06002598 RID: 9624 RVA: 0x00098728 File Offset: 0x00096928
		protected void VlandiaYeomanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x06002599 RID: 9625 RVA: 0x00098730 File Offset: 0x00096930
		protected void VlandiaBlacksmithOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600259A RID: 9626 RVA: 0x00098738 File Offset: 0x00096938
		protected void VlandiaHunterOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600259B RID: 9627 RVA: 0x00098740 File Offset: 0x00096940
		protected void VlandiaMercenaryOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x0600259C RID: 9628 RVA: 0x00098748 File Offset: 0x00096948
		protected void SturgiaBoyarsCompanionOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 1, WoTCharacterCreation.OccupationTypes.Retainer, "", "", true, true);
		}

		// Token: 0x0600259D RID: 9629 RVA: 0x0009875F File Offset: 0x0009695F
		protected void SturgiaTraderOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 2, WoTCharacterCreation.OccupationTypes.Merchant, "", "", true, true);
		}

		// Token: 0x0600259E RID: 9630 RVA: 0x00098776 File Offset: 0x00096976
		protected void SturgiaFreemanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 3, WoTCharacterCreation.OccupationTypes.Farmer, "", "", true, true);
		}

		// Token: 0x0600259F RID: 9631 RVA: 0x0009878D File Offset: 0x0009698D
		protected void SturgiaArtisanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 4, WoTCharacterCreation.OccupationTypes.Artisan, "", "", true, true);
		}

		// Token: 0x060025A0 RID: 9632 RVA: 0x000987A4 File Offset: 0x000969A4
		protected void SturgiaHunterOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 5, WoTCharacterCreation.OccupationTypes.Hunter, "", "", true, true);
		}

		// Token: 0x060025A1 RID: 9633 RVA: 0x000987BB File Offset: 0x000969BB
		protected void SturgiaVagabondOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 6, WoTCharacterCreation.OccupationTypes.Vagabond, "", "", true, true);
		}

		// Token: 0x060025A2 RID: 9634 RVA: 0x000987D2 File Offset: 0x000969D2
		protected void SturgiaBoyarsCompanionOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025A3 RID: 9635 RVA: 0x000987DA File Offset: 0x000969DA
		protected void SturgiaTraderOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025A4 RID: 9636 RVA: 0x000987E2 File Offset: 0x000969E2
		protected void SturgiaFreemanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025A5 RID: 9637 RVA: 0x000987EA File Offset: 0x000969EA
		protected void SturgiaArtisanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025A6 RID: 9638 RVA: 0x000987F2 File Offset: 0x000969F2
		protected void SturgiaHunterOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025A7 RID: 9639 RVA: 0x000987FA File Offset: 0x000969FA
		protected void SturgiaVagabondOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x00098802 File Offset: 0x00096A02
		protected void AseraiTribesmanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 1, WoTCharacterCreation.OccupationTypes.Retainer, "", "", true, true);
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00098819 File Offset: 0x00096A19
		protected void AseraiWariorSlaveOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 2, WoTCharacterCreation.OccupationTypes.Mercenary, "", "", true, true);
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x00098830 File Offset: 0x00096A30
		protected void AseraiMerchantOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 3, WoTCharacterCreation.OccupationTypes.Merchant, "", "", true, true);
		}

		// Token: 0x060025AB RID: 9643 RVA: 0x00098847 File Offset: 0x00096A47
		protected void AseraiOasisFarmerOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 4, WoTCharacterCreation.OccupationTypes.Farmer, "", "", true, true);
		}

		// Token: 0x060025AC RID: 9644 RVA: 0x0009885E File Offset: 0x00096A5E
		protected void AseraiBedouinOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 5, WoTCharacterCreation.OccupationTypes.Herder, "", "", true, true);
		}

		// Token: 0x060025AD RID: 9645 RVA: 0x00098875 File Offset: 0x00096A75
		protected void AseraiBackAlleyThugOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 6, WoTCharacterCreation.OccupationTypes.Artisan, "", "", true, true);
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x0009888C File Offset: 0x00096A8C
		protected void AseraiTribesmanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00098894 File Offset: 0x00096A94
		protected void AseraiWariorSlaveOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x0009889C File Offset: 0x00096A9C
		protected void AseraiMerchantOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025B1 RID: 9649 RVA: 0x000988A4 File Offset: 0x00096AA4
		protected void AseraiOasisFarmerOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025B2 RID: 9650 RVA: 0x000988AC File Offset: 0x00096AAC
		protected void AseraiBedouinOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025B3 RID: 9651 RVA: 0x000988B4 File Offset: 0x00096AB4
		protected void AseraiBackAlleyThugOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025B4 RID: 9652 RVA: 0x000988BC File Offset: 0x00096ABC
		protected void BattaniaChieftainsHearthguardOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 1, WoTCharacterCreation.OccupationTypes.Retainer, "", "", true, true);
		}

		// Token: 0x060025B5 RID: 9653 RVA: 0x000988D3 File Offset: 0x00096AD3
		protected void BattaniaHealerOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 2, WoTCharacterCreation.OccupationTypes.Healer, "", "", true, true);
		}

		// Token: 0x060025B6 RID: 9654 RVA: 0x000988EB File Offset: 0x00096AEB
		protected void BattaniaTribesmanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 3, WoTCharacterCreation.OccupationTypes.Farmer, "", "", true, true);
		}

		// Token: 0x060025B7 RID: 9655 RVA: 0x00098902 File Offset: 0x00096B02
		protected void BattaniaSmithOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 4, WoTCharacterCreation.OccupationTypes.Artisan, "", "", true, true);
		}

		// Token: 0x060025B8 RID: 9656 RVA: 0x00098919 File Offset: 0x00096B19
		protected void BattaniaWoodsmanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 5, WoTCharacterCreation.OccupationTypes.Hunter, "", "", true, true);
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x00098930 File Offset: 0x00096B30
		protected void BattaniaBardOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 6, WoTCharacterCreation.OccupationTypes.Bard, "", "", true, true);
		}

		// Token: 0x060025BA RID: 9658 RVA: 0x00098947 File Offset: 0x00096B47
		protected void BattaniaChieftainsHearthguardOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x0009894F File Offset: 0x00096B4F
		protected void BattaniaHealerOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x00098957 File Offset: 0x00096B57
		protected void BattaniaTribesmanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x0009895F File Offset: 0x00096B5F
		protected void BattaniaSmithOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x00098967 File Offset: 0x00096B67
		protected void BattaniaWoodsmanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x0009896F File Offset: 0x00096B6F
		protected void BattaniaBardOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025C0 RID: 9664 RVA: 0x00098977 File Offset: 0x00096B77
		protected void KhuzaitNoyansKinsmanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 1, WoTCharacterCreation.OccupationTypes.Retainer, "", "", true, true);
		}

		// Token: 0x060025C1 RID: 9665 RVA: 0x0009898E File Offset: 0x00096B8E
		protected void KhuzaitMerchantOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 2, WoTCharacterCreation.OccupationTypes.Merchant, "", "", true, true);
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x000989A5 File Offset: 0x00096BA5
		protected void KhuzaitTribesmanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 3, WoTCharacterCreation.OccupationTypes.Herder, "", "", true, true);
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x000989BC File Offset: 0x00096BBC
		protected void KhuzaitFarmerOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 4, WoTCharacterCreation.OccupationTypes.Farmer, "", "", true, true);
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x000989D3 File Offset: 0x00096BD3
		protected void KhuzaitShamanOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 5, WoTCharacterCreation.OccupationTypes.Healer, "", "", true, true);
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x000989EB File Offset: 0x00096BEB
		protected void KhuzaitNomadOnConsequence(CharacterCreation characterCreation)
		{
			this.SetParentAndOccupationType(characterCreation, 6, WoTCharacterCreation.OccupationTypes.Herder, "", "", true, true);
		}

		// Token: 0x060025C6 RID: 9670 RVA: 0x00098A02 File Offset: 0x00096C02
		protected void KhuzaitNoyansKinsmanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025C7 RID: 9671 RVA: 0x00098A0A File Offset: 0x00096C0A
		protected void KhuzaitMerchantOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025C8 RID: 9672 RVA: 0x00098A12 File Offset: 0x00096C12
		protected void KhuzaitTribesmanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x00098A1A File Offset: 0x00096C1A
		protected void KhuzaitFarmerOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x00098A22 File Offset: 0x00096C22
		protected void KhuzaitShamanOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x00098A2A File Offset: 0x00096C2A
		protected void KhuzaitNomadOnApply(CharacterCreation characterCreation)
		{
			this.FinalizeParents();
		}

		// Token: 0x060025CC RID: 9676 RVA: 0x00098A32 File Offset: 0x00096C32
		protected bool EmpireParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire";
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00098A49 File Offset: 0x00096C49
		protected bool VlandianParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "vlandia";
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00098A60 File Offset: 0x00096C60
		protected bool SturgianParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "sturgia";
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00098A77 File Offset: 0x00096C77
		protected bool AseraiParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "aserai";
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00098A8E File Offset: 0x00096C8E
		protected bool BattanianParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "battania";
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00098AA5 File Offset: 0x00096CA5
		protected bool KhuzaitParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "khuzait";
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00098ABC File Offset: 0x00096CBC
		protected void FinalizeParents()
		{
			CharacterObject @object = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_mother");
			CharacterObject object2 = Game.Current.ObjectManager.GetObject<CharacterObject>("main_hero_father");
			@object.HeroObject.ModifyPlayersFamilyAppearance(base.MotherFacegenCharacter.BodyProperties.StaticProperties);
			object2.HeroObject.ModifyPlayersFamilyAppearance(base.FatherFacegenCharacter.BodyProperties.StaticProperties);
			@object.HeroObject.Weight = base.MotherFacegenCharacter.BodyProperties.Weight;
			@object.HeroObject.Build = base.MotherFacegenCharacter.BodyProperties.Build;
			object2.HeroObject.Weight = base.FatherFacegenCharacter.BodyProperties.Weight;
			object2.HeroObject.Build = base.FatherFacegenCharacter.BodyProperties.Build;
			EquipmentHelper.AssignHeroEquipmentFromEquipment(@object.HeroObject, base.MotherFacegenCharacter.Equipment);
			EquipmentHelper.AssignHeroEquipmentFromEquipment(object2.HeroObject, base.FatherFacegenCharacter.Equipment);
			@object.Culture = Hero.MainHero.Culture;
			object2.Culture = Hero.MainHero.Culture;
			StringHelpers.SetCharacterProperties("PLAYER", CharacterObject.PlayerCharacter, null, false);
			TextObject textObject = GameTexts.FindText("str_player_father_name", Hero.MainHero.Culture.StringId);
			object2.HeroObject.SetName(textObject, textObject);
			TextObject textObject2 = new TextObject("{=XmvaRfLM}{PLAYER_FATHER.NAME} was the father of {PLAYER.LINK}. He was slain when raiders attacked the inn at which his family was staying.", null);
			StringHelpers.SetCharacterProperties("PLAYER_FATHER", object2, textObject2, false);
			object2.HeroObject.EncyclopediaText = textObject2;
			TextObject textObject3 = GameTexts.FindText("str_player_mother_name", Hero.MainHero.Culture.StringId);
			@object.HeroObject.SetName(textObject3, textObject3);
			TextObject textObject4 = new TextObject("{=hrhvEWP8}{PLAYER_MOTHER.NAME} was the mother of {PLAYER.LINK}. She was slain when raiders attacked the inn at which her family was staying.", null);
			StringHelpers.SetCharacterProperties("PLAYER_MOTHER", @object, textObject4, false);
			@object.HeroObject.EncyclopediaText = textObject4;
			@object.HeroObject.UpdateHomeSettlement();
			object2.HeroObject.UpdateHomeSettlement();
			@object.HeroObject.HasMet = true;
			object2.HeroObject.HasMet = true;
		}

		// Token: 0x060025D3 RID: 9683 RVA: 0x00098CDC File Offset: 0x00096EDC
		protected static List<FaceGenChar> ChangePlayerFaceWithAge(float age, string actionName = "act_childhood_schooled")
		{
			List<FaceGenChar> list = new List<FaceGenChar>();
			BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
			bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, age);
			list.Add(new FaceGenChar(bodyProperties, new Equipment(), CharacterObject.PlayerCharacter.IsFemale, actionName));
			return list;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00098D2C File Offset: 0x00096F2C
		protected Equipment ChangePlayerOutfit(CharacterCreation characterCreation, string outfit)
		{
			List<Equipment> list = new List<Equipment>();
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(outfit);
			Equipment equipment = (@object != null) ? @object.DefaultEquipment : null;
			if (equipment == null)
			{
				Debug.FailedAssert("item shouldn't be null! Check this!", "C:\\Develop\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CharacterCreationContent\\WoTCharacterCreation.cs", "ChangePlayerOutfit", 1035);
				equipment = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>("player_char_creation_default").DefaultEquipment;
			}
			list.Add(equipment);
			characterCreation.ChangeCharactersEquipment(list);
			return equipment;
		}

		// Token: 0x060025D5 RID: 9685 RVA: 0x00098DA4 File Offset: 0x00096FA4
		protected static void ChangePlayerMount(CharacterCreation characterCreation, Hero hero)
		{
			List<FaceGenMount> list = new List<FaceGenMount>();
			if (hero.CharacterObject.HasMount())
			{
				ItemObject item = hero.CharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item;
				list.Add(new FaceGenMount(MountCreationKey.GetRandomMountKey(item, hero.CharacterObject.GetMountKeySeed()), hero.CharacterObject.Equipment[EquipmentIndex.ArmorItemEndSlot].Item, hero.CharacterObject.Equipment[EquipmentIndex.HorseHarness].Item, "act_horse_stand_1"));
				characterCreation.ChangeFaceGenMounts(list);
			}
		}

		// Token: 0x060025D6 RID: 9686 RVA: 0x00098E3B File Offset: 0x0009703B
		protected static void ClearMountEntity(CharacterCreation characterCreation)
		{
			characterCreation.ClearFaceGenMounts();
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00098E44 File Offset: 0x00097044
		protected void ChildhoodOnInit(CharacterCreation characterCreation)
		{
			CultureObject[] cultures = CharacterCreationContentBase.Instance.GetCultures().ToArray();
			foreach (CultureObject culture in cultures)
			{
				if (culture.Id.ToString() == "sturgia")
				{
					base.SetSelectedCulture(culture, characterCreation);
				}
			}
			
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this.ChildhoodAge, "act_childhood_schooled"));
			string text = string.Concat(new object[]
			{
				"player_char_creation_childhood_age_",
				"sturgia",
				"_",
				base.SelectedParentType
			});
			text += (Hero.MainHero.IsFemale ? "_f" : "_m");
			this.ChangePlayerOutfit(characterCreation, text);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			WoTCharacterCreation.ClearMountEntity(characterCreation);
		}

		// Token: 0x060025D8 RID: 9688 RVA: 0x00098EF8 File Offset: 0x000970F8
		protected static void ChildhoodYourLeadershipSkillsOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_leader"
			});
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00098F10 File Offset: 0x00097110
		protected static void ChildhoodYourBrawnOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00098F28 File Offset: 0x00097128
		protected static void ChildhoodAttentionToDetailOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_memory"
			});
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00098F40 File Offset: 0x00097140
		protected static void ChildhoodAptitudeForNumbersOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_numbers"
			});
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00098F58 File Offset: 0x00097158
		protected static void ChildhoodWayWithPeopleOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_manners"
			});
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x00098F70 File Offset: 0x00097170
		protected static void ChildhoodSkillsWithHorsesOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_animals"
			});
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x00098F88 File Offset: 0x00097188
		protected static void ChildhoodGoodLeadingOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x00098F8A File Offset: 0x0009718A
		protected static void ChildhoodGoodAthleticsOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x00098F8C File Offset: 0x0009718C
		protected static void ChildhoodGoodMemoryOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x00098F8E File Offset: 0x0009718E
		protected static void ChildhoodGoodMathOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x00098F90 File Offset: 0x00097190
		protected static void ChildhoodGoodMannersOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x00098F92 File Offset: 0x00097192
		protected static void ChildhoodAffinityWithAnimalsOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x00098F94 File Offset: 0x00097194
		protected void EducationOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			characterCreation.ClearFaceGenPrefab();
			TextObject textObject = new TextObject("{=WYvnWcXQ}Like all village children you helped out in the fields. You also...", null);
			TextObject textObject2 = new TextObject("{=DsCkf6Pb}Growing up, you spent most of your time...", null);
			this._educationIntroductoryText.SetTextVariable("EDUCATION_INTRO", this.RuralType() ? textObject : textObject2);
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this.EducationAge, "act_childhood_schooled"));
			string text = string.Concat(new object[]
			{
				"player_char_creation_education_age_",
				base.GetSelectedCulture().StringId,
				"_",
				base.SelectedParentType
			});
			text += (Hero.MainHero.IsFemale ? "_f" : "_m");
			this.ChangePlayerOutfit(characterCreation, text);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			WoTCharacterCreation.ClearMountEntity(characterCreation);
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x00099080 File Offset: 0x00097280
		protected bool RuralType()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Farmer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Hunter || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Bard || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Herder || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Vagabond || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Healer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Artisan;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x000990D8 File Offset: 0x000972D8
		protected bool RichParents()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Merchant;
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000990EE File Offset: 0x000972EE
		protected bool RuralAdolescenceOnCondition()
		{
			return this.RuralType();
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000990F6 File Offset: 0x000972F6
		protected bool UrbanAdolescenceOnCondition()
		{
			return !this.RuralType();
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00099101 File Offset: 0x00097301
		protected bool UrbanRichAdolescenceOnCondition()
		{
			return !this.RuralType() && this.RichParents();
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x00099113 File Offset: 0x00097313
		protected bool UrbanPoorAdolescenceOnCondition()
		{
			return !this.RuralType() && !this.RichParents();
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x00099128 File Offset: 0x00097328
		protected void RefreshPropsAndClothing(CharacterCreation characterCreation, bool isChildhoodStage, string itemId, bool isLeftHand, string secondItemId = "")
		{
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ClearCharactersEquipment();
			string text = isChildhoodStage ? string.Concat(new object[]
			{
				"player_char_creation_childhood_age_",
				base.GetSelectedCulture().StringId,
				"_",
				base.SelectedParentType
			}) : string.Concat(new object[]
			{
				"player_char_creation_education_age_",
				base.GetSelectedCulture().StringId,
				"_",
				base.SelectedParentType
			});
			text += (Hero.MainHero.IsFemale ? "_f" : "_m");
			Equipment equipment = this.ChangePlayerOutfit(characterCreation, text).Clone(false);
			if (Game.Current.ObjectManager.GetObject<ItemObject>(itemId) != null)
			{
				ItemObject @object = Game.Current.ObjectManager.GetObject<ItemObject>(itemId);
				equipment.AddEquipmentToSlotWithoutAgent(isLeftHand ? EquipmentIndex.WeaponItemBeginSlot : EquipmentIndex.Weapon1, new EquipmentElement(@object, null, null, false));
				if (secondItemId != "")
				{
					@object = Game.Current.ObjectManager.GetObject<ItemObject>(secondItemId);
					equipment.AddEquipmentToSlotWithoutAgent(isLeftHand ? EquipmentIndex.Weapon1 : EquipmentIndex.WeaponItemBeginSlot, new EquipmentElement(@object, null, null, false));
				}
			}
			else
			{
				characterCreation.ChangeCharacterPrefab(itemId, isLeftHand ? Game.Current.HumanMonster.MainHandItemBoneIndex : Game.Current.HumanMonster.OffHandItemBoneIndex);
			}
			characterCreation.ChangeCharactersEquipment(new List<Equipment>
			{
				equipment
			});
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x00099295 File Offset: 0x00097495
		protected void RuralAdolescenceHerderOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_streets"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000992C0 File Offset: 0x000974C0
		protected void RuralAdolescenceSmithyOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_militia"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x000992EB File Offset: 0x000974EB
		protected void RuralAdolescenceRepairmanOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_grit"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "carry_hammer", true, "");
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00099316 File Offset: 0x00097516
		protected void RuralAdolescenceGathererOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x00099341 File Offset: 0x00097541
		protected void RuralAdolescenceHunterOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_sharp"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "composite_bow", true, "");
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x0009936C File Offset: 0x0009756C
		protected void RuralAdolescenceHelperOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers_2"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x00099397 File Offset: 0x00097597
		protected void UrbanAdolescenceWatcherOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_fox"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000993C2 File Offset: 0x000975C2
		protected void UrbanAdolescenceMarketerOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_manners"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000993ED File Offset: 0x000975ED
		protected void UrbanAdolescenceGangerOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x00099418 File Offset: 0x00097618
		protected void UrbanAdolescenceDockerOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x00099443 File Offset: 0x00097643
		protected void UrbanAdolescenceHorserOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers_2"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0009946E File Offset: 0x0009766E
		protected void UrbanAdolescenceTutorOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_book"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "character_creation_notebook", false, "");
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x00099499 File Offset: 0x00097699
		protected static void RuralAdolescenceHerderOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x0009949B File Offset: 0x0009769B
		protected static void RuralAdolescenceSmithyOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x0009949D File Offset: 0x0009769D
		protected static void RuralAdolescenceRepairmanOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x0009949F File Offset: 0x0009769F
		protected static void RuralAdolescenceGathererOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000994A1 File Offset: 0x000976A1
		protected static void RuralAdolescenceHunterOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000994A3 File Offset: 0x000976A3
		protected static void RuralAdolescenceHelperOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000994A5 File Offset: 0x000976A5
		protected static void UrbanAdolescenceWatcherOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000994A7 File Offset: 0x000976A7
		protected static void UrbanAdolescenceMarketerOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000994A9 File Offset: 0x000976A9
		protected static void UrbanAdolescenceGangerOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000994AB File Offset: 0x000976AB
		protected static void UrbanAdolescenceDockerOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000994B0 File Offset: 0x000976B0
		protected void YouthOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			characterCreation.ClearFaceGenPrefab();
			TextObject textObject = new TextObject("{=F7OO5SAa}As a youngster growing up in Calradia, war was never too far away. You...", null);
			TextObject textObject2 = new TextObject("{=5kbeAC7k}In wartorn Calradia, especially in frontier or tribal areas, some women as well as men learn to fight from an early age. You...", null);
			this._youthIntroductoryText.SetTextVariable("YOUTH_INTRO", CharacterObject.PlayerCharacter.IsFemale ? textObject2 : textObject);
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this.YouthAge, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			if (base.SelectedTitleType < 1 || base.SelectedTitleType > 10)
			{
				base.SelectedTitleType = 1;
			}
			this.RefreshPlayerAppearance(characterCreation);
		}

		// Token: 0x06002603 RID: 9731 RVA: 0x00099558 File Offset: 0x00097758
		protected void RefreshPlayerAppearance(CharacterCreation characterCreation)
		{
			string text = string.Concat(new object[]
			{
				"player_char_creation_",
				base.GetSelectedCulture().StringId,
				"_",
				base.SelectedTitleType
			});
			text += (Hero.MainHero.IsFemale ? "_f" : "_m");
			this.ChangePlayerOutfit(characterCreation, text);
			this.ApplyEquipments(characterCreation);
		}

		// Token: 0x06002604 RID: 9732 RVA: 0x000995CC File Offset: 0x000977CC
		protected bool YouthCommanderOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" && this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x000995F0 File Offset: 0x000977F0
		protected void YouthCommanderOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x000995F2 File Offset: 0x000977F2
		protected bool YouthGroomOnCondition()
		{
			return base.GetSelectedCulture().StringId == "vlandia" && this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00099616 File Offset: 0x00097816
		protected void YouthCommanderOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 10;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_decisive"
			});
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0009963D File Offset: 0x0009783D
		protected void YouthGroomOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 10;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_sharp"
			});
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x00099664 File Offset: 0x00097864
		protected void YouthChieftainOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 10;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_ready"
			});
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x0009968B File Offset: 0x0009788B
		protected void YouthCavalryOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 9;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_apprentice"
			});
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000996B2 File Offset: 0x000978B2
		protected void YouthHearthGuardOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 9;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000996D9 File Offset: 0x000978D9
		protected void YouthOutridersOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 2;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_gracious"
			});
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000996FF File Offset: 0x000978FF
		protected void YouthOtherOutridersOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 2;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_gracious"
			});
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x00099725 File Offset: 0x00097925
		protected void YouthInfantryOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 3;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_fierce"
			});
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0009974B File Offset: 0x0009794B
		protected void YouthSkirmisherOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 4;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_fox"
			});
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x00099771 File Offset: 0x00097971
		protected void YouthGarrisonOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 1;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_vibrant"
			});
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x00099797 File Offset: 0x00097997
		protected void YouthOtherGarrisonOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 1;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_sharp"
			});
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000997BD File Offset: 0x000979BD
		protected void YouthKernOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 8;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_apprentice"
			});
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000997E3 File Offset: 0x000979E3
		protected void YouthCamperOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 5;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_militia"
			});
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x00099809 File Offset: 0x00097A09
		protected void YouthGroomOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x0009980B File Offset: 0x00097A0B
		protected bool YouthChieftainOnCondition()
		{
			return (base.GetSelectedCulture().StringId == "battania" || base.GetSelectedCulture().StringId == "khuzait") && this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x00099846 File Offset: 0x00097A46
		protected void YouthChieftainOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x00099848 File Offset: 0x00097A48
		protected bool YouthCavalryOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait" || base.GetSelectedCulture().StringId == "aserai" || base.GetSelectedCulture().StringId == "vlandia";
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000998B1 File Offset: 0x00097AB1
		protected void YouthCavalryOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000998B3 File Offset: 0x00097AB3
		protected bool YouthHearthGuardOnCondition()
		{
			return base.GetSelectedCulture().StringId == "sturgia" || base.GetSelectedCulture().StringId == "battania";
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000998E3 File Offset: 0x00097AE3
		protected void YouthHearthGuardOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x000998E5 File Offset: 0x00097AE5
		protected bool YouthOutridersOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait";
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x00099915 File Offset: 0x00097B15
		protected void YouthOutridersOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x00099917 File Offset: 0x00097B17
		protected bool YouthOtherOutridersOnCondition()
		{
			return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "khuzait";
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x00099947 File Offset: 0x00097B47
		protected void YouthOtherOutridersOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x00099949 File Offset: 0x00097B49
		protected void YouthInfantryOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x0009994B File Offset: 0x00097B4B
		protected void YouthSkirmisherOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x0009994D File Offset: 0x00097B4D
		protected bool YouthGarrisonOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "vlandia";
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x0009997D File Offset: 0x00097B7D
		protected void YouthGarrisonOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x0009997F File Offset: 0x00097B7F
		protected bool YouthOtherGarrisonOnCondition()
		{
			return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "vlandia";
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000999AF File Offset: 0x00097BAF
		protected void YouthOtherGarrisonOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x000999B1 File Offset: 0x00097BB1
		protected bool YouthSkirmisherOnCondition()
		{
			return base.GetSelectedCulture().StringId != "battania";
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000999C8 File Offset: 0x00097BC8
		protected bool YouthKernOnCondition()
		{
			return base.GetSelectedCulture().StringId == "battania";
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000999DF File Offset: 0x00097BDF
		protected void YouthKernOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000999E1 File Offset: 0x00097BE1
		protected bool YouthCamperOnCondition()
		{
			return this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000999EF File Offset: 0x00097BEF
		protected void YouthCamperOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000999F4 File Offset: 0x00097BF4
		protected void AccomplishmentOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this.AccomplishmentAge, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			this.RefreshPlayerAppearance(characterCreation);
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x00099A49 File Offset: 0x00097C49
		protected void AccomplishmentDefeatedEnemyOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x00099A4B File Offset: 0x00097C4B
		protected void AccomplishmentExpeditionOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x00099A4D File Offset: 0x00097C4D
		protected bool AccomplishmentRuralOnCondition()
		{
			return this.RuralType();
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00099A55 File Offset: 0x00097C55
		protected bool AccomplishmentMerchantOnCondition()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Merchant;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x00099A60 File Offset: 0x00097C60
		protected bool AccomplishmentPosseOnConditions()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Herder || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Mercenary;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x00099A7F File Offset: 0x00097C7F
		protected bool AccomplishmentSavedVillageOnCondition()
		{
			return this.RuralType() && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Retainer && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Herder;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x00099AA0 File Offset: 0x00097CA0
		protected bool AccomplishmentSavedStreetOnCondition()
		{
			return !this.RuralType() && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Merchant && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Mercenary;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x00099AC1 File Offset: 0x00097CC1
		protected bool AccomplishmentUrbanOnCondition()
		{
			return !this.RuralType();
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x00099ACC File Offset: 0x00097CCC
		protected void AccomplishmentWorkshopOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x00099ACE File Offset: 0x00097CCE
		protected void AccomplishmentSiegeHunterOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x00099AD0 File Offset: 0x00097CD0
		protected void AccomplishmentEscapadeOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x00099AD2 File Offset: 0x00097CD2
		protected void AccomplishmentTreaterOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x00099AD4 File Offset: 0x00097CD4
		protected void AccomplishmentDefeatedEnemyOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x00099AEC File Offset: 0x00097CEC
		protected void AccomplishmentExpeditionOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_gracious"
			});
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x00099B04 File Offset: 0x00097D04
		protected void AccomplishmentMerchantOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_ready"
			});
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x00099B1C File Offset: 0x00097D1C
		protected void AccomplishmentSavedVillageOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_vibrant"
			});
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x00099B34 File Offset: 0x00097D34
		protected void AccomplishmentSavedStreetOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_vibrant"
			});
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x00099B4C File Offset: 0x00097D4C
		protected void AccomplishmentWorkshopOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_decisive"
			});
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x00099B64 File Offset: 0x00097D64
		protected void AccomplishmentSiegeHunterOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_tough"
			});
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x00099B7C File Offset: 0x00097D7C
		protected void AccomplishmentEscapadeOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_clever"
			});
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x00099B94 File Offset: 0x00097D94
		protected void AccomplishmentTreaterOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_manners"
			});
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x00099BAC File Offset: 0x00097DAC
		protected void StartingAgeOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this._startingAge, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			this.RefreshPlayerAppearance(characterCreation);
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x00099C04 File Offset: 0x00097E04
		protected void StartingAgeYoungOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge(20f, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_focus"
			});
			this.RefreshPlayerAppearance(characterCreation);
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.YoungAdult;
			this.SetHeroAge(20f);
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x00099C5C File Offset: 0x00097E5C
		protected void StartingAgeAdultOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge(30f, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_ready"
			});
			this.RefreshPlayerAppearance(characterCreation);
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.Adult;
			this.SetHeroAge(30f);
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x00099CB4 File Offset: 0x00097EB4
		protected void StartingAgeMiddleAgedOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge(40f, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_sharp"
			});
			this.RefreshPlayerAppearance(characterCreation);
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.MiddleAged;
			this.SetHeroAge(40f);
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x00099D0C File Offset: 0x00097F0C
		protected void StartingAgeElderlyOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge(50f, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_tough"
			});
			this.RefreshPlayerAppearance(characterCreation);
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.Elder;
			this.SetHeroAge(50f);
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x00099D64 File Offset: 0x00097F64
		protected void StartingAgeYoungOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.YoungAdult;
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x00099D6E File Offset: 0x00097F6E
		protected void StartingAgeAdultOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.Adult;
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x00099D78 File Offset: 0x00097F78
		protected void StartingAgeMiddleAgedOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.MiddleAged;
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x00099D82 File Offset: 0x00097F82
		protected void StartingAgeElderlyOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.Elder;
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x00099D8C File Offset: 0x00097F8C
		protected void ApplyEquipments(CharacterCreation characterCreation)
		{
			WoTCharacterCreation.ClearMountEntity(characterCreation);
			string text = string.Concat(new object[]
			{
				"player_char_creation_",
				base.GetSelectedCulture().StringId,
				"_",
				base.SelectedTitleType
			});
			text += (Hero.MainHero.IsFemale ? "_f" : "_m");
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(text);
			base.PlayerStartEquipment = (((@object != null) ? @object.DefaultEquipment : null) ?? MBEquipmentRoster.EmptyEquipment);
			base.PlayerCivilianEquipment = (((@object != null) ? @object.GetCivilianEquipments().FirstOrDefault<Equipment>() : null) ?? MBEquipmentRoster.EmptyEquipment);
			if (base.PlayerStartEquipment != null && base.PlayerCivilianEquipment != null)
			{
				CharacterObject.PlayerCharacter.Equipment.FillFrom(base.PlayerStartEquipment, true);
				CharacterObject.PlayerCharacter.FirstCivilianEquipment.FillFrom(base.PlayerCivilianEquipment, true);
			}
			WoTCharacterCreation.ChangePlayerMount(characterCreation, Hero.MainHero);
		}

		// Token: 0x0600264A RID: 9802 RVA: 0x00099E89 File Offset: 0x00098089
		protected void SetHeroAge(float age)
		{
			Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-age));
		}

		// Token: 0x04000D43 RID: 3395
		protected const int FocusToAddYouthStart = 2;

		// Token: 0x04000D44 RID: 3396
		protected const int FocusToAddAdultStart = 4;

		// Token: 0x04000D45 RID: 3397
		protected const int FocusToAddMiddleAgedStart = 6;

		// Token: 0x04000D46 RID: 3398
		protected const int FocusToAddElderlyStart = 8;

		// Token: 0x04000D47 RID: 3399
		protected const int AttributeToAddYouthStart = 1;

		// Token: 0x04000D48 RID: 3400
		protected const int AttributeToAddAdultStart = 2;

		// Token: 0x04000D49 RID: 3401
		protected const int AttributeToAddMiddleAgedStart = 3;

		// Token: 0x04000D4A RID: 3402
		protected const int AttributeToAddElderlyStart = 4;

		// Token: 0x04000D4B RID: 3403
		protected readonly Dictionary<string, Vec2> _startingPoints = new Dictionary<string, Vec2>
		{
			{
				"empire",
				new Vec2(657.95f, 279.08f)
			},
			{
				"sturgia",
				new Vec2(356.75f, 551.52f)
			},
			{
				"aserai",
				new Vec2(300.78f, 259.99f)
			},
			{
				"battania",
				new Vec2(293.64f, 446.39f)
			},
			{
				"khuzait",
				new Vec2(680.73f, 480.8f)
			},
			{
				"vlandia",
				new Vec2(207.04f, 389.04f)
			}
		};

		// Token: 0x04000D4C RID: 3404
		protected WoTCharacterCreation.SandboxAgeOptions _startingAge = WoTCharacterCreation.SandboxAgeOptions.YoungAdult;

		// Token: 0x04000D4D RID: 3405
		protected WoTCharacterCreation.OccupationTypes _familyOccupationType;

		// Token: 0x04000D4E RID: 3406
		protected TextObject _educationIntroductoryText = new TextObject("{=!}{EDUCATION_INTRO}", null);

		// Token: 0x04000D4F RID: 3407
		protected TextObject _youthIntroductoryText = new TextObject("{=!}{YOUTH_INTRO}", null);

		// Token: 0x020005BA RID: 1466
		protected enum SandboxAgeOptions
		{
			// Token: 0x0400177C RID: 6012
			YoungAdult = 20,
			// Token: 0x0400177D RID: 6013
			Adult = 30,
			// Token: 0x0400177E RID: 6014
			MiddleAged = 40,
			// Token: 0x0400177F RID: 6015
			Elder = 50
		}

		// Token: 0x020005BB RID: 1467
		protected enum OccupationTypes
		{
			// Token: 0x04001781 RID: 6017
			Artisan,
			// Token: 0x04001782 RID: 6018
			Bard,
			// Token: 0x04001783 RID: 6019
			Retainer,
			// Token: 0x04001784 RID: 6020
			Merchant,
			// Token: 0x04001785 RID: 6021
			Farmer,
			// Token: 0x04001786 RID: 6022
			Hunter,
			// Token: 0x04001787 RID: 6023
			Vagabond,
			// Token: 0x04001788 RID: 6024
			Mercenary,
			// Token: 0x04001789 RID: 6025
			Herder,
			// Token: 0x0400178A RID: 6026
			Healer,
			// Token: 0x0400178B RID: 6027
			NumberOfTypes
		}
	}
}
