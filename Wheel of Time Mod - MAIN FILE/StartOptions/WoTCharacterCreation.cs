using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace WoT_Main
{
	// Token: 0x02000222 RID: 546
	public class WoTCharacterCreation : CharacterCreationContentBase
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x00095B19 File Offset: 0x00093D19
		public override TextObject ReviewPageDescription
		{
			get
			{
				return new TextObject("You prepare to set off for a grand adventure! Here is your character. Continue if you are ready, or go back to make changes.", null);
			}
		}

		// Token: 0x17000A23 RID: 2595
		// (get) Token: 0x06002574 RID: 9588 RVA: 0x00095B26 File Offset: 0x00093D26
		public override IEnumerable<Type> CharacterCreationStages
		{
			get
			{
				yield return typeof(CharacterCreationCultureStage);
				yield return typeof(CharacterCreationFaceGeneratorStage);
				yield return typeof(CharacterCreationGenericStage);
				yield return typeof(CharacterCreationBannerEditorStage);
				yield return typeof(CharacterCreationClanNamingStage);
				yield return typeof(CharacterCreationReviewStage);
				yield return typeof(CharacterCreationOptionsStage);
				yield break;
			}
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x00095B2F File Offset: 0x00093D2F
		protected override void OnCultureSelected()
		{
			base.SelectedTitleType = 1;
			base.SelectedParentType = 0;
			Clan.PlayerClan.ChangeClanName(FactionHelper.GenerateClanNameforPlayer());
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x00095B4E File Offset: 0x00093D4E
		public override int GetSelectedParentType()
		{
			return base.SelectedParentType;
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x00095B58 File Offset: 0x00093D58
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

		// Token: 0x06002578 RID: 9592 RVA: 0x00095BF2 File Offset: 0x00093DF2
		protected override void OnInitialized(CharacterCreation characterCreation)
		{
			this.AddOriginMenu(characterCreation);
			this.AddProfessionMenu(characterCreation);
			this.AddAchievementMenu(characterCreation);
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x00095C1E File Offset: 0x00093E1E
		protected override void OnApplyCulture()
		{
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x00095C20 File Offset: 0x00093E20
		protected void AddOriginMenu(CharacterCreation characterCreation)
        {

			
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Origin", null), new TextObject("You were...", null), new CharacterCreationOnInit(this.OriginOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			
			//Shadowspawn Origin

			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			List<SkillObject> effectedSkills1 = new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			CharacterAttribute effectedAttribute = DefaultCharacterAttributes.Vigor;
			characterCreationCategory.AddCategoryOption(new TextObject("kidnapped as a child", null), effectedSkills1, effectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Kidnapped_Child_Origin_On_Apply, null, new TextObject("As a child you were draged from your home village by the forces of the shadow. You have spent most of your life in the company of others, involuntarely working for the shadow.", null), null, 0, 0, 0, 0, 0);

			List<SkillObject> effectedSkills2 = new List<SkillObject>
			{
				DefaultSkills.OneHanded,
				DefaultSkills.Athletics
			};
			CharacterAttribute effectedAttribute2 = DefaultCharacterAttributes.Vigor;
			characterCreationCategory.AddCategoryOption(new TextObject("born of trollocs", null), effectedSkills2, effectedAttribute2, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Trolloc_Origin_On_Apply, null, new TextObject("In a dirty trolloc-breeding pit you first saw light. As one of many siblings you began your life as the cheap, worthless monsters the shadow used as their main force, yet you were better than others, you survived when most died.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new TextObject("born in a family of darkfriends", null), new List<SkillObject>{
				DefaultSkills.Roguery,
				DefaultSkills.Bow
			},
			DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Darkfriends_Origin_On_Apply, null, new TextObject("Your family stood in the service of the dark one for a long time. So do you, a young darkfriend, convinced of the ideals of the shadow since childhood, ready to give his life for the dark one.", null), null, 0, 0, 0, 0, 0);


           
				characterCreationCategory.AddCategoryOption(new TextObject("a member of the black Ajah / a male Aiel channeler", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Medicine
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Channeler_Origin_On_Apply, null, new TextObject("As soon as you obtained the shal, you joined the black Ajah. The reasons were many: the normal sisters simply didn't understand you, you despised the laws forbidding research in dark parts of channeling and either way the lord of chaos was your prefered master, not some mortal Amyrlin. / Involuntarily or out of your free will you landed in the ranks of the Samma N'Sei, channeling the one power for the dark one.", null), null, 0, 0, 0, 0, 0);
			
			
			

				characterCreationCategory.AddCategoryOption(new TextObject("a daughter/son of a dreadlord", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Dreadlord_Origin_On_Apply, null, new TextObject("You were part of the ruling class of the shadowspawn. Your father, a powerful dreadlord insured an excellent childhood, away from slaves, trollocs and filth. You were tought in many disciplines, mainly how to command and organise troops.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("an offspring of slaves", null), new List<SkillObject>{
				DefaultSkills.Roguery,
				DefaultSkills.Polearm
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Slaves_Origin_On_Apply, null, new TextObject("Your life was hard, working from morning till dawn. As an offspring of slaves you had no saying in what was done to you, your darkfriend-owner decided everything about you.", null), null, 0, 0, 0, 0, 0);


			characterCreation.AddNewMenu(characterCreationMenu);
		}
		protected void AddProfessionMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Profession", null), new TextObject("You earned your living as a...", null), new CharacterCreationOnInit(this.OriginOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);


			characterCreationCategory.AddCategoryOption(new TextObject("as a farmer", null), new List<SkillObject>{
				DefaultSkills.Scouting,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, null, new TextObject("Tilling, sowing and harvesting, this was the rythm of your life. An honest, though boring job.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a blacksmith's apprentice", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.Engineering
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, null, new TextObject("Your village's blacksmith took you under his hood, teaching you everaything you need.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a hunter in the woods", null), new List<SkillObject>{
				DefaultSkills.Scouting,
				DefaultSkills.Bow
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, null, new TextObject("Hunting all kinds of animals was your main profession, laying traps, sniffing out gain, a tedious yet rewarding job.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a merchant's assistant", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, null, new TextObject("Travelling from town to town, buying and selling, this is how you pased your days, a job which gave you a lot of profit, yet involved the risks of the dangerous roads.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a fisher", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Medicine
				},
				DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_fisher_condition, null, null, new TextObject("On a boat you cought fishes, making a living out of it.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a sheep herder", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Bow
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_sheep_condition, null, null, new TextObject("In the wide planes you herded sheep, defending them from wolfs with the help of your trusty bow and dog.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a trolloc breeder", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Bow
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_trolloc_condition, null, null, new TextObject("You watched over the trolloc breeding pits, an ugly job which always left you quesy afterwards.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a engineer on fortification", null), new List<SkillObject>{
				DefaultSkills.Engineering,
				DefaultSkills.Tactics
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_fortify_condition, null, null, new TextObject("You fortified towns, villages and camps with your crew, getting a good feal of how to defend your fortifications and how to attack them.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a raider and bandit", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_raider_condition, null, null, new TextObject("As a bandit you scowerd the lands, raping, robing and killing. 'Take everything and leave nothing', this was your moto.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a pirate", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Crossbow
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_pirate_condition, null, null, new TextObject("On a boat under the service of your captain you raided and boarded smaller traiding vehicles, even plundering a village on accasion.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a stable hand", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Throwing
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_horse_condition, null, null, new TextObject("You worked at stables, grooming, riding and caring horses. A great job your employers said to you, until realised everything was about clearing away shit.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a healer", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_healer_condition, null, null, new TextObject("You were your villages hedge doctor's/wisdom's apprentice, learning the use of herbs to heal wounds and sicknesses.", null), null, 0, 0, 0, 0, 0);

			characterCreation.AddNewMenu(characterCreationMenu);
		}
		protected void AddAchievementMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Origin", null), new TextObject("You were...", null), new CharacterCreationOnInit(this.OriginOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			List<SkillObject> effectedSkills = new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			CharacterAttribute effectedAttribute = DefaultCharacterAttributes.Vigor;
			characterCreationCategory.AddCategoryOption(new TextObject("{=InN5ZZt3}A landlord's retainers", null), effectedSkills, effectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, null, new TextObject("{=ivKl4mV2}Your father was a trusted lieutenant of the local landowning aristocrat. He rode with the lord's cavalry, fighting as an armored lancer.", null), null, 0, 0, 0, 0, 0);
			effectedSkills = new List<SkillObject>
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			};

			characterCreation.AddNewMenu(characterCreationMenu);
		}
		protected void AddParentsMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Origin", null), new TextObject("You were...", null), new CharacterCreationOnInit(this.OriginOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			List<SkillObject> effectedSkills = new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			CharacterAttribute effectedAttribute = DefaultCharacterAttributes.Vigor;
			characterCreationCategory.AddCategoryOption(new TextObject("{=InN5ZZt3}A landlord's retainers", null), effectedSkills, effectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null ,null, new TextObject("{=ivKl4mV2}Your father was a trusted lieutenant of the local landowning aristocrat. He rode with the lord's cavalry, fighting as an armored lancer.", null), null, 0, 0, 0, 0, 0);
			effectedSkills = new List<SkillObject>
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			};
			
			characterCreation.AddNewMenu(characterCreationMenu);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x00096C48 File Offset: 0x00094E48
		protected void AddChildhoodMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=8Yiwt1z6}Early Childhood", null), new TextObject("{=character_creation_content_16}As a child you were noted for...", null), new CharacterCreationOnInit(this.ChildhoodOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			characterCreationCategory.AddCategoryOption(new TextObject("{=kmM68Qx4}your leadership skills.", null), new List<SkillObject>
			{
				DefaultSkills.Leadership,
				DefaultSkills.Tactics
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodYourLeadershipSkillsOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodLeadingOnApply), new TextObject("{=FfNwXtii}If the wolf pup gang of your early childhood had an alpha, it was definitely you. All the other kids followed your lead as you decided what to play and where to play, and led them in games and mischief.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=5HXS8HEY}your brawn.", null), new List<SkillObject>
			{
				DefaultSkills.TwoHanded,
				DefaultSkills.Throwing
			}, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodYourBrawnOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodAthleticsOnApply), new TextObject("{=YKzuGc54}You were big, and other children looked to have you around in any scrap with children from a neighboring village. You pushed a plough and throw an axe like an adult.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=QrYjPUEf}your attention to detail.", null), new List<SkillObject>
			{
				DefaultSkills.Athletics,
				DefaultSkills.Bow
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(WoTCharacterCreation.ChildhoodAttentionToDetailOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.ChildhoodGoodMemoryOnApply), new TextObject("{=JUSHAPnu}You were quick on your feet and attentive to what was going on around you. Usually you could run away from trouble, though you could give a good account of yourself in a fight with other children if cornered.", null), null, 0, 0, 0, 0, 0);
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

		// Token: 0x0600257C RID: 9596 RVA: 0x00096F1C File Offset: 0x0009511C
		protected void AddEducationMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=rcoueCmk}Adolescence", null), this._educationIntroductoryText, new CharacterCreationOnInit(this.EducationOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			characterCreationCategory.AddCategoryOption(new TextObject("{=RKVNvimC}herded the sheep.", null), new List<SkillObject>
			{
				DefaultSkills.Athletics,
				DefaultSkills.Throwing
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceHerderOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.RuralAdolescenceHerderOnApply), new TextObject("{=KfaqPpbK}You went with other fleet-footed youths to take the villages' sheep, goats or cattle to graze in pastures near the village. You were in charge of chasing down stray beasts, and always kept a big stone on hand to be hurled at lurking predators if necessary.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=bTKiN0hr}worked in the village smithy.", null), new List<SkillObject>
			{
				DefaultSkills.TwoHanded,
				DefaultSkills.Crafting
			}, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceSmithyOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.RuralAdolescenceSmithyOnApply), new TextObject("{=y6j1bJTH}You were apprenticed to the local smith. You learned how to heat and forge metal, hammering for hours at a time until your muscles ached.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=tI8ZLtoA}repaired projects.", null), new List<SkillObject>
			{
				DefaultSkills.Crafting,
				DefaultSkills.Engineering
			}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceRepairmanOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.RuralAdolescenceRepairmanOnApply), new TextObject("{=6LFj919J}You helped dig wells, rethatch houses, and fix broken plows. You learned about the basics of construction, as well as what it takes to keep a farming community prosperous.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=TRwgSLD2}gathered herbs in the wild.", null), new List<SkillObject>
			{
				DefaultSkills.Medicine,
				DefaultSkills.Scouting
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceGathererOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.RuralAdolescenceGathererOnApply), new TextObject("{=9ks4u5cH}You were sent by the village healer up into the hills to look for useful medicinal plants. You learned which herbs healed wounds or brought down a fever, and how to find them.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=T7m7ReTq}hunted small game.", null), new List<SkillObject>
			{
				DefaultSkills.Bow,
				DefaultSkills.Tactics
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceHunterOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.RuralAdolescenceHunterOnApply), new TextObject("{=RuvSk3QT}You accompanied a local hunter as he went into the wilderness, helping him set up traps and catch small animals.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=qAbMagWq}sold produce at the market.", null), new List<SkillObject>
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			}, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.RuralAdolescenceOnCondition), new CharacterCreationOnSelect(this.RuralAdolescenceHelperOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.RuralAdolescenceHelperOnApply), new TextObject("{=DIgsfYfz}You took your family's goods to the nearest town to sell your produce and buy supplies. It was hard work, but you enjoyed the hubbub of the marketplace.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=nOfSqRnI}at the town watch's training ground.", null), new List<SkillObject>
			{
				DefaultSkills.Crossbow,
				DefaultSkills.Tactics
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceWatcherOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceWatcherOnApply), new TextObject("{=qnqdEJOv}You watched the town's watch practice shooting and perfect their plans to defend the walls in case of a siege.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=8a6dnLd2}with the alley gangs.", null), new List<SkillObject>
			{
				DefaultSkills.Roguery,
				DefaultSkills.OneHanded
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceGangerOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceGangerOnApply), new TextObject("{=1SUTcF0J}The gang leaders who kept watch over the slums of Calradian cities were always in need of poor youth to run messages and back them up in turf wars, while thrill-seeking merchants' sons and daughters sometimes slummed it in their company as well.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=7Hv984Sf}at docks and building sites.", null), new List<SkillObject>
			{
				DefaultSkills.Athletics,
				DefaultSkills.Crafting
			}, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceDockerOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceDockerOnApply), new TextObject("{=bhdkegZ4}All towns had their share of projects that were constantly in need of both skilled and unskilled labor. You learned how hoists and scaffolds were constructed, how planks and stones were hewn and fitted, and other skills.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=kbcwb5TH}in the markets and caravanserais.", null), new List<SkillObject>
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			}, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceMarketerOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceMarketerOnApply), new TextObject("{=lLJh7WAT}You worked in the marketplace, selling trinkets and drinks to busy shoppers.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=kbcwb5TH}in the markets and caravanserais.", null), new List<SkillObject>
			{
				DefaultSkills.Trade,
				DefaultSkills.Charm
			}, DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceMarketerOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceMarketerOnApply), new TextObject("{=rmMcwSn8}You helped your family handle their business affairs, going down to the marketplace to make purchases and oversee the arrival of caravans.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=mfRbx5KE}reading and studying.", null), new List<SkillObject>
			{
				DefaultSkills.Engineering,
				DefaultSkills.Leadership
			}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceTutorOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceDockerOnApply), new TextObject("{=elQnygal}Your family scraped up the money for a rudimentary schooling and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=etG87fB7}with your tutor.", null), new List<SkillObject>
			{
				DefaultSkills.Engineering,
				DefaultSkills.Leadership
			}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceTutorOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceDockerOnApply), new TextObject("{=hXl25avg}Your family arranged for a private tutor and you took full advantage, reading voraciously on history, mathematics, and philosophy and discussing what you read with your tutor and classmates.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=FKpLEamz}caring for horses.", null), new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Steward
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanRichAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceDockerOnApply), new TextObject("{=Ghz90npw}Your family owned a few horses at the town stables and you took charge of their care. Many evenings you would take them out beyond the walls and gallup through the fields, racing other youth.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=vH7GtuuK}working at the stables.", null), new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Steward
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.UrbanPoorAdolescenceOnCondition), new CharacterCreationOnSelect(this.UrbanAdolescenceHorserOnConsequence), new CharacterCreationApplyFinalEffects(WoTCharacterCreation.UrbanAdolescenceDockerOnApply), new TextObject("{=csUq1RCC}You were employed as a hired hand at the town's stables. The overseers recognized that you had a knack for horses, and you were allowed to exercise them and sometimes even break in new steeds.", null), null, 0, 0, 0, 0, 0);
			characterCreation.AddNewMenu(characterCreationMenu);
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x00097664 File Offset: 0x00095864
		protected void AddYouthMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("{=ok8lSW6M}Youth", null), this._youthIntroductoryText, new CharacterCreationOnInit(this.YouthOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			characterCreationCategory.AddCategoryOption(new TextObject("{=CITG915d}joined a commander's staff.", null), new List<SkillObject>
			{
				DefaultSkills.Steward,
				DefaultSkills.Tactics
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthCommanderOnCondition), new CharacterCreationOnSelect(this.YouthCommanderOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCommanderOnApply), new TextObject("{=Ay0G3f7I}Your family arranged for you to be part of the staff of an imperial strategos. You were not given major responsibilities - mostly carrying messages and tending to his horse -- but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=bhE2i6OU}served as a baron's groom.", null), new List<SkillObject>
			{
				DefaultSkills.Steward,
				DefaultSkills.Tactics
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthGroomOnCondition), new CharacterCreationOnSelect(this.YouthGroomOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGroomOnApply), new TextObject("{=iZKtGI6Y}Your family arranged for you to accompany a minor baron of the Vlandian kingdom. You were not given major responsibilities - mostly carrying messages and tending to his horse -- but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=F2bgujPo}were a chieftain's servant.", null), new List<SkillObject>
			{
				DefaultSkills.Steward,
				DefaultSkills.Tactics
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthChieftainOnCondition), new CharacterCreationOnSelect(this.YouthChieftainOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthChieftainOnApply), new TextObject("{=7AYJ3SjK}Your family arranged for you to accompany a chieftain of your people. You were not given major responsibilities - mostly carrying messages and tending to his horse -- but it did give you a chance to see how campaigns were planned and men were deployed in battle.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=h2KnarLL}trained with the cavalry.", null), new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthCavalryOnCondition), new CharacterCreationOnSelect(this.YouthCavalryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCavalryOnApply), new TextObject("{=7cHsIMLP}You could never have bought the equipment on your own but you were a good enough rider so that the local lord lent you a horse and equipment. You joined the armored cavalry, training with the lance.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=zsC2t5Hb}trained with the hearth guard.", null), new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthHearthGuardOnCondition), new CharacterCreationOnSelect(this.YouthHearthGuardOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthHearthGuardOnApply), new TextObject("{=RmbWW6Bm}You were a big and imposing enough youth that the chief's guard allowed you to train alongside them, in preparation to join them some day.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new List<SkillObject>
			{
				DefaultSkills.Crossbow,
				DefaultSkills.Engineering
			}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthGarrisonOnCondition), new CharacterCreationOnSelect(this.YouthGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthGarrisonOnApply), new TextObject("{=63TAYbkx}Urban troops spend much of their time guarding the town walls. Most of their training was in missile weapons, especially useful during sieges.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=aTncHUfL}stood guard with the garrisons.", null), new List<SkillObject>
			{
				DefaultSkills.Bow,
				DefaultSkills.Engineering
			}, DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthOtherGarrisonOnCondition), new CharacterCreationOnSelect(this.YouthOtherGarrisonOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherGarrisonOnApply), new TextObject("{=1EkEElZd}Urban troops spend much of their time guarding the town walls. Most of their training was in missile.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=VlXOgIX6}rode with the scouts.", null), new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Bow
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthOutridersOnCondition), new CharacterCreationOnSelect(this.YouthOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOutridersOnApply), new TextObject("{=888lmJqs}All of Calradia's kingdoms recognize the value of good light cavalry and horse archers, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a good bow so that you could fulfill their levy obligations.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=VlXOgIX6}rode with the scouts.", null), new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Bow
			}, DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthOtherOutridersOnCondition), new CharacterCreationOnSelect(this.YouthOtherOutridersOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthOtherOutridersOnApply), new TextObject("{=sYuN6hPD}All of Calradia's kingdoms recognize the value of good light cavalry, and are sure to recruit nomads and borderers with the skills to fulfill those duties. You were a good enough rider that your neighbors pitched in to buy you a small pony and a sheaf of javelins so that you could fulfill their levy obligations.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=a8arFSra}trained with the infantry.", null), new List<SkillObject>
			{
				DefaultSkills.Polearm,
				DefaultSkills.OneHanded
			}, DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, new CharacterCreationOnSelect(this.YouthInfantryOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthInfantryOnApply), new TextObject("{=afH90aNs}Levy armed with spear and shield, drawn from smallholding farmers, have always been the backbone of most armies of Calradia.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=oMbOIPc9}joined the skirmishers.", null), new List<SkillObject>
			{
				DefaultSkills.Throwing,
				DefaultSkills.OneHanded
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthSkirmisherOnCondition), new CharacterCreationOnSelect(this.YouthSkirmisherOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthSkirmisherOnApply), new TextObject("{=bXAg5w19}Younger recruits, or those of a slighter build, or those too poor to buy shield and armor tend to join the skirmishers. Fighting with bow and javelin, they try to stay out of reach of the main enemy forces.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=cDWbwBwI}joined the kern.", null), new List<SkillObject>
			{
				DefaultSkills.Throwing,
				DefaultSkills.OneHanded
			}, DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthKernOnCondition), new CharacterCreationOnSelect(this.YouthKernOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthKernOnApply), new TextObject("{=tTb28jyU}Many Battanians fight as kern, versatile troops who could both harass the enemy line with their javelins or join in the final screaming charge once it weakened.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("{=GFUggps8}marched with the camp followers.", null), new List<SkillObject>
			{
				DefaultSkills.Roguery,
				DefaultSkills.Throwing
			}, DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, new CharacterCreationOnCondition(this.YouthCamperOnCondition), new CharacterCreationOnSelect(this.YouthCamperOnConsequence), new CharacterCreationApplyFinalEffects(this.YouthCamperOnApply), new TextObject("{=64rWqBLN}You avoided service with one of the main forces of your realm's armies, but followed instead in the train - the troops' wives, lovers and servants, and those who make their living by caring for, entertaining, or cheating the soldiery.", null), null, 0, 0, 0, 0, 0);
			characterCreation.AddNewMenu(characterCreationMenu);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x00097CB0 File Offset: 0x00095EB0
		protected void AddAdulthoodMenu(CharacterCreation characterCreation)
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

		// Token: 0x0600257F RID: 9599 RVA: 0x00098340 File Offset: 0x00096540
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

		protected void Shadowspawn_Origin_Kidnapped_Child_Origin_On_Apply(CharacterCreation characterCreation)
        {
			ChangePlayerOutfit(characterCreation, "poor_CC_1");
        }
		
		protected void Shadowspawn_Origin_Trolloc_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "trolloc_CC_1");
		}
		
		protected void Shadowspawn_Origin_Darkfriends_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "middle_CC_1");
		}
		
		protected void Shadowspawn_Origin_Channeler_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "channeler_CC_1");
		}
		protected void Shadowspawn_Origin_Dreadlord_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "rich_CC_1");
		}
		protected void Shadowspawn_Origin_Slaves_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "poor_CC_2");
		}
		
		protected void Shadowspawn_Origin_Kidnapped_Child_Origin_On_Consequence(CharacterCreation characterCreation)
		{

		}


		protected bool Profession_fisher_condition()
		{
			return base.GetSelectedCulture().StringId == "battania" || base.GetSelectedCulture().StringId == "vlandia" || base.GetSelectedCulture().StringId == "Tarabon" || base.GetSelectedCulture().StringId == "Altara"  || base.GetSelectedCulture().StringId == "Tear" || base.GetSelectedCulture().StringId == "Mayene" || base.GetSelectedCulture().StringId == "Seanchan";
		}

		protected bool Profession_sheep_condition()
		{
			return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "Murandy" || base.GetSelectedCulture().StringId == "Amamdicia" || base.GetSelectedCulture().StringId == "Ghealdan" || base.GetSelectedCulture().StringId == "Cairhien" || base.GetSelectedCulture().StringId == "Cairhien" || base.GetSelectedCulture().StringId == "Arafel" || base.GetSelectedCulture().StringId == "Shienar" || base.GetSelectedCulture().StringId == "WhiteTower" || base.GetSelectedCulture().StringId == "BlackTower" || base.GetSelectedCulture().StringId == "khuzait" || base.GetSelectedCulture().StringId == "Dragonsworn";
		}
		protected bool Profession_trolloc_condition()
		{
			return base.GetSelectedCulture().StringId == "sturgia";
		}
		protected bool Profession_fortify_condition()
		{
			return base.GetSelectedCulture().StringId == "battania" || base.GetSelectedCulture().StringId == "Kandor" || base.GetSelectedCulture().StringId == "Arafel" || base.GetSelectedCulture().StringId == "Shienar" || base.GetSelectedCulture().StringId == "sturgia" || base.GetSelectedCulture().StringId == "Dragonsworn";
		}

		protected bool Profession_raider_condition()
		{
			return base.GetSelectedCulture().StringId == "khuzait" || base.GetSelectedCulture().StringId == "Cairhien" || base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "WhiteTower" || base.GetSelectedCulture().StringId == "BlackTower" || base.GetSelectedCulture().StringId == "FarMadding";
		}
		protected bool Profession_pirate_condition()
		{
			return base.GetSelectedCulture().StringId == "aserai" || base.GetSelectedCulture().StringId == "Tear" || base.GetSelectedCulture().StringId == "Mayene";
		}
		protected bool Profession_horse_condition()
		{
			return base.GetSelectedCulture().StringId == "vlandia" || base.GetSelectedCulture().StringId == "Tarabon" || base.GetSelectedCulture().StringId == "FarMadding";
		}
		protected bool Profession_healer_condition()
		{
			return base.GetSelectedCulture().StringId == "Ghealdan" || base.GetSelectedCulture().StringId == "Amamdicia" || base.GetSelectedCulture().StringId == "Altara" || base.GetSelectedCulture().StringId == "Murandy";
		}


		// Token: 0x060025CC RID: 9676 RVA: 0x00098C62 File Offset: 0x00096E62
		protected bool EmpireParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire";
		}

		// Token: 0x060025CD RID: 9677 RVA: 0x00098C79 File Offset: 0x00096E79
		protected bool VlandianParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "vlandia";
		}

		// Token: 0x060025CE RID: 9678 RVA: 0x00098C90 File Offset: 0x00096E90
		protected bool SturgianParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "sturgia";
		}

		// Token: 0x060025CF RID: 9679 RVA: 0x00098CA7 File Offset: 0x00096EA7
		protected bool AseraiParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "aserai";
		}

		// Token: 0x060025D0 RID: 9680 RVA: 0x00098CBE File Offset: 0x00096EBE
		protected bool BattanianParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "battania";
		}

		// Token: 0x060025D1 RID: 9681 RVA: 0x00098CD5 File Offset: 0x00096ED5
		protected bool KhuzaitParentsOnCondition()
		{
			return base.GetSelectedCulture().StringId == "khuzait";
		}

		// Token: 0x060025D2 RID: 9682 RVA: 0x00098CEC File Offset: 0x00096EEC
	

		// Token: 0x060025D3 RID: 9683 RVA: 0x00098F0C File Offset: 0x0009710C
		protected static List<FaceGenChar> ChangePlayerFaceWithAge(float age, string actionName = "act_childhood_schooled")
		{
			List<FaceGenChar> list = new List<FaceGenChar>();
			BodyProperties bodyProperties = CharacterObject.PlayerCharacter.GetBodyProperties(CharacterObject.PlayerCharacter.Equipment, -1);
			bodyProperties = FaceGen.GetBodyPropertiesWithAge(ref bodyProperties, age);
			list.Add(new FaceGenChar(bodyProperties, new Equipment(), CharacterObject.PlayerCharacter.IsFemale, actionName));
			return list;
		}

		// Token: 0x060025D4 RID: 9684 RVA: 0x00098F5C File Offset: 0x0009715C
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

		// Token: 0x060025D5 RID: 9685 RVA: 0x00098FD4 File Offset: 0x000971D4
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

		// Token: 0x060025D6 RID: 9686 RVA: 0x0009906B File Offset: 0x0009726B
		protected static void ClearMountEntity(CharacterCreation characterCreation)
		{
			characterCreation.ClearFaceGenMounts();
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00099074 File Offset: 0x00097274
		protected void ChildhoodOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this.ChildhoodAge, "act_childhood_schooled"));
			string text = string.Concat(new object[]
			{
				"player_char_creation_childhood_age_",
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

		// Token: 0x060025D8 RID: 9688 RVA: 0x00099128 File Offset: 0x00097328
		protected static void ChildhoodYourLeadershipSkillsOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_leader"
			});
		}

		// Token: 0x060025D9 RID: 9689 RVA: 0x00099140 File Offset: 0x00097340
		protected static void ChildhoodYourBrawnOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x00099158 File Offset: 0x00097358
		protected static void ChildhoodAttentionToDetailOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_memory"
			});
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x00099170 File Offset: 0x00097370
		protected static void ChildhoodAptitudeForNumbersOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_numbers"
			});
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00099188 File Offset: 0x00097388
		protected static void ChildhoodWayWithPeopleOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_manners"
			});
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x000991A0 File Offset: 0x000973A0
		protected static void ChildhoodSkillsWithHorsesOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_animals"
			});
		}

		// Token: 0x060025DE RID: 9694 RVA: 0x000991B8 File Offset: 0x000973B8
		protected static void ChildhoodGoodLeadingOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025DF RID: 9695 RVA: 0x000991BA File Offset: 0x000973BA
		protected static void ChildhoodGoodAthleticsOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E0 RID: 9696 RVA: 0x000991BC File Offset: 0x000973BC
		protected static void ChildhoodGoodMemoryOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E1 RID: 9697 RVA: 0x000991BE File Offset: 0x000973BE
		protected static void ChildhoodGoodMathOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E2 RID: 9698 RVA: 0x000991C0 File Offset: 0x000973C0
		protected static void ChildhoodGoodMannersOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E3 RID: 9699 RVA: 0x000991C2 File Offset: 0x000973C2
		protected static void ChildhoodAffinityWithAnimalsOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025E4 RID: 9700 RVA: 0x000991C4 File Offset: 0x000973C4
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

		// Token: 0x060025E5 RID: 9701 RVA: 0x000992B0 File Offset: 0x000974B0
		protected bool RuralType()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Farmer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Hunter || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Bard || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Herder || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Vagabond || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Healer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Artisan;
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x00099308 File Offset: 0x00097508
		protected bool RichParents()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Merchant;
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x0009931E File Offset: 0x0009751E
		protected bool RuralAdolescenceOnCondition()
		{
			return this.RuralType();
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x00099326 File Offset: 0x00097526
		protected bool UrbanAdolescenceOnCondition()
		{
			return !this.RuralType();
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x00099331 File Offset: 0x00097531
		protected bool UrbanRichAdolescenceOnCondition()
		{
			return !this.RuralType() && this.RichParents();
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x00099343 File Offset: 0x00097543
		protected bool UrbanPoorAdolescenceOnCondition()
		{
			return !this.RuralType() && !this.RichParents();
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x00099358 File Offset: 0x00097558
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

		// Token: 0x060025EC RID: 9708 RVA: 0x000994C5 File Offset: 0x000976C5
		protected void RuralAdolescenceHerderOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_streets"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000994F0 File Offset: 0x000976F0
		protected void RuralAdolescenceSmithyOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_militia"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
		}

		// Token: 0x060025EE RID: 9710 RVA: 0x0009951B File Offset: 0x0009771B
		protected void RuralAdolescenceRepairmanOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_grit"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "carry_hammer", true, "");
		}

		// Token: 0x060025EF RID: 9711 RVA: 0x00099546 File Offset: 0x00097746
		protected void RuralAdolescenceGathererOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x00099571 File Offset: 0x00097771
		protected void RuralAdolescenceHunterOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_sharp"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "composite_bow", true, "");
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x0009959C File Offset: 0x0009779C
		protected void RuralAdolescenceHelperOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers_2"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000995C7 File Offset: 0x000977C7
		protected void UrbanAdolescenceWatcherOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_fox"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000995F2 File Offset: 0x000977F2
		protected void UrbanAdolescenceMarketerOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_manners"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x0009961D File Offset: 0x0009781D
		protected void UrbanAdolescenceGangerOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "", true, "");
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x00099648 File Offset: 0x00097848
		protected void UrbanAdolescenceDockerOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_basket_a", true, "");
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x00099673 File Offset: 0x00097873
		protected void UrbanAdolescenceHorserOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_peddlers_2"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "_to_carry_bd_fabric_c", true, "");
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x0009969E File Offset: 0x0009789E
		protected void UrbanAdolescenceTutorOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_book"
			});
			this.RefreshPropsAndClothing(characterCreation, false, "character_creation_notebook", false, "");
		}

		// Token: 0x060025F8 RID: 9720 RVA: 0x000996C9 File Offset: 0x000978C9
		protected static void RuralAdolescenceHerderOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025F9 RID: 9721 RVA: 0x000996CB File Offset: 0x000978CB
		protected static void RuralAdolescenceSmithyOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FA RID: 9722 RVA: 0x000996CD File Offset: 0x000978CD
		protected static void RuralAdolescenceRepairmanOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FB RID: 9723 RVA: 0x000996CF File Offset: 0x000978CF
		protected static void RuralAdolescenceGathererOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FC RID: 9724 RVA: 0x000996D1 File Offset: 0x000978D1
		protected static void RuralAdolescenceHunterOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FD RID: 9725 RVA: 0x000996D3 File Offset: 0x000978D3
		protected static void RuralAdolescenceHelperOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FE RID: 9726 RVA: 0x000996D5 File Offset: 0x000978D5
		protected static void UrbanAdolescenceWatcherOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000996D7 File Offset: 0x000978D7
		protected static void UrbanAdolescenceMarketerOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000996D9 File Offset: 0x000978D9
		protected static void UrbanAdolescenceGangerOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000996DB File Offset: 0x000978DB
		protected static void UrbanAdolescenceDockerOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002602 RID: 9730 RVA: 0x000996E0 File Offset: 0x000978E0
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

		// Token: 0x06002603 RID: 9731 RVA: 0x00099788 File Offset: 0x00097988
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

		// Token: 0x06002604 RID: 9732 RVA: 0x000997FC File Offset: 0x000979FC
		protected bool YouthCommanderOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" && this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002605 RID: 9733 RVA: 0x00099820 File Offset: 0x00097A20
		protected void YouthCommanderOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002606 RID: 9734 RVA: 0x00099822 File Offset: 0x00097A22
		protected bool YouthGroomOnCondition()
		{
			return base.GetSelectedCulture().StringId == "vlandia" && this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x00099846 File Offset: 0x00097A46
		protected void YouthCommanderOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 10;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_decisive"
			});
		}

		// Token: 0x06002608 RID: 9736 RVA: 0x0009986D File Offset: 0x00097A6D
		protected void YouthGroomOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 10;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_sharp"
			});
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x00099894 File Offset: 0x00097A94
		protected void YouthChieftainOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 10;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_ready"
			});
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000998BB File Offset: 0x00097ABB
		protected void YouthCavalryOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 9;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_apprentice"
			});
		}

		// Token: 0x0600260B RID: 9739 RVA: 0x000998E2 File Offset: 0x00097AE2
		protected void YouthHearthGuardOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 9;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x00099909 File Offset: 0x00097B09
		protected void YouthOutridersOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 2;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_gracious"
			});
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x0009992F File Offset: 0x00097B2F
		protected void YouthOtherOutridersOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 2;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_gracious"
			});
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x00099955 File Offset: 0x00097B55
		protected void YouthInfantryOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 3;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_fierce"
			});
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x0009997B File Offset: 0x00097B7B
		protected void YouthSkirmisherOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 4;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_fox"
			});
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000999A1 File Offset: 0x00097BA1
		protected void YouthGarrisonOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 1;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_vibrant"
			});
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000999C7 File Offset: 0x00097BC7
		protected void YouthOtherGarrisonOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 1;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_sharp"
			});
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000999ED File Offset: 0x00097BED
		protected void YouthKernOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 8;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_apprentice"
			});
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x00099A13 File Offset: 0x00097C13
		protected void YouthCamperOnConsequence(CharacterCreation characterCreation)
		{
			base.SelectedTitleType = 5;
			this.RefreshPlayerAppearance(characterCreation);
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_militia"
			});
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x00099A39 File Offset: 0x00097C39
		protected void YouthGroomOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x00099A3B File Offset: 0x00097C3B
		protected bool YouthChieftainOnCondition()
		{
			return (base.GetSelectedCulture().StringId == "battania" || base.GetSelectedCulture().StringId == "khuzait") && this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x00099A76 File Offset: 0x00097C76
		protected void YouthChieftainOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x00099A78 File Offset: 0x00097C78
		protected bool YouthCavalryOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait" || base.GetSelectedCulture().StringId == "aserai" || base.GetSelectedCulture().StringId == "vlandia";
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x00099AE1 File Offset: 0x00097CE1
		protected void YouthCavalryOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x00099AE3 File Offset: 0x00097CE3
		protected bool YouthHearthGuardOnCondition()
		{
			return base.GetSelectedCulture().StringId == "sturgia" || base.GetSelectedCulture().StringId == "battania";
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x00099B13 File Offset: 0x00097D13
		protected void YouthHearthGuardOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600261B RID: 9755 RVA: 0x00099B15 File Offset: 0x00097D15
		protected bool YouthOutridersOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "khuzait";
		}

		// Token: 0x0600261C RID: 9756 RVA: 0x00099B45 File Offset: 0x00097D45
		protected void YouthOutridersOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600261D RID: 9757 RVA: 0x00099B47 File Offset: 0x00097D47
		protected bool YouthOtherOutridersOnCondition()
		{
			return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "khuzait";
		}

		// Token: 0x0600261E RID: 9758 RVA: 0x00099B77 File Offset: 0x00097D77
		protected void YouthOtherOutridersOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600261F RID: 9759 RVA: 0x00099B79 File Offset: 0x00097D79
		protected void YouthInfantryOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002620 RID: 9760 RVA: 0x00099B7B File Offset: 0x00097D7B
		protected void YouthSkirmisherOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002621 RID: 9761 RVA: 0x00099B7D File Offset: 0x00097D7D
		protected bool YouthGarrisonOnCondition()
		{
			return base.GetSelectedCulture().StringId == "empire" || base.GetSelectedCulture().StringId == "vlandia";
		}

		// Token: 0x06002622 RID: 9762 RVA: 0x00099BAD File Offset: 0x00097DAD
		protected void YouthGarrisonOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x00099BAF File Offset: 0x00097DAF
		protected bool YouthOtherGarrisonOnCondition()
		{
			return base.GetSelectedCulture().StringId != "empire" && base.GetSelectedCulture().StringId != "vlandia";
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x00099BDF File Offset: 0x00097DDF
		protected void YouthOtherGarrisonOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002625 RID: 9765 RVA: 0x00099BE1 File Offset: 0x00097DE1
		protected bool YouthSkirmisherOnCondition()
		{
			return base.GetSelectedCulture().StringId != "battania";
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x00099BF8 File Offset: 0x00097DF8
		protected bool YouthKernOnCondition()
		{
			return base.GetSelectedCulture().StringId == "battania";
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x00099C0F File Offset: 0x00097E0F
		protected void YouthKernOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x00099C11 File Offset: 0x00097E11
		protected bool YouthCamperOnCondition()
		{
			return this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Retainer;
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x00099C1F File Offset: 0x00097E1F
		protected void YouthCamperOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x00099C24 File Offset: 0x00097E24
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

		// Token: 0x0600262B RID: 9771 RVA: 0x00099C79 File Offset: 0x00097E79
		protected void AccomplishmentDefeatedEnemyOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x00099C7B File Offset: 0x00097E7B
		protected void AccomplishmentExpeditionOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x00099C7D File Offset: 0x00097E7D
		protected bool AccomplishmentRuralOnCondition()
		{
			return this.RuralType();
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x00099C85 File Offset: 0x00097E85
		protected bool AccomplishmentMerchantOnCondition()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Merchant;
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x00099C90 File Offset: 0x00097E90
		protected bool AccomplishmentPosseOnConditions()
		{
			return this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Retainer || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Herder || this._familyOccupationType == WoTCharacterCreation.OccupationTypes.Mercenary;
		}

		// Token: 0x06002630 RID: 9776 RVA: 0x00099CAF File Offset: 0x00097EAF
		protected bool AccomplishmentSavedVillageOnCondition()
		{
			return this.RuralType() && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Retainer && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Herder;
		}

		// Token: 0x06002631 RID: 9777 RVA: 0x00099CD0 File Offset: 0x00097ED0
		protected bool AccomplishmentSavedStreetOnCondition()
		{
			return !this.RuralType() && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Merchant && this._familyOccupationType != WoTCharacterCreation.OccupationTypes.Mercenary;
		}

		// Token: 0x06002632 RID: 9778 RVA: 0x00099CF1 File Offset: 0x00097EF1
		protected bool AccomplishmentUrbanOnCondition()
		{
			return !this.RuralType();
		}

		// Token: 0x06002633 RID: 9779 RVA: 0x00099CFC File Offset: 0x00097EFC
		protected void AccomplishmentWorkshopOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x00099CFE File Offset: 0x00097EFE
		protected void AccomplishmentSiegeHunterOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x00099D00 File Offset: 0x00097F00
		protected void AccomplishmentEscapadeOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002636 RID: 9782 RVA: 0x00099D02 File Offset: 0x00097F02
		protected void AccomplishmentTreaterOnApply(CharacterCreation characterCreation)
		{
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x00099D04 File Offset: 0x00097F04
		protected void AccomplishmentDefeatedEnemyOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_athlete"
			});
		}

		// Token: 0x06002638 RID: 9784 RVA: 0x00099D1C File Offset: 0x00097F1C
		protected void AccomplishmentExpeditionOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_gracious"
			});
		}

		// Token: 0x06002639 RID: 9785 RVA: 0x00099D34 File Offset: 0x00097F34
		protected void AccomplishmentMerchantOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_ready"
			});
		}

		// Token: 0x0600263A RID: 9786 RVA: 0x00099D4C File Offset: 0x00097F4C
		protected void AccomplishmentSavedVillageOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_vibrant"
			});
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x00099D64 File Offset: 0x00097F64
		protected void AccomplishmentSavedStreetOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_vibrant"
			});
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x00099D7C File Offset: 0x00097F7C
		protected void AccomplishmentWorkshopOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_decisive"
			});
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x00099D94 File Offset: 0x00097F94
		protected void AccomplishmentSiegeHunterOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_tough"
			});
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x00099DAC File Offset: 0x00097FAC
		protected void AccomplishmentEscapadeOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_clever"
			});
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x00099DC4 File Offset: 0x00097FC4
		protected void AccomplishmentTreaterOnConsequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_manners"
			});
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x00099DDC File Offset: 0x00097FDC
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

		// Token: 0x06002641 RID: 9793 RVA: 0x00099E34 File Offset: 0x00098034
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

		// Token: 0x06002642 RID: 9794 RVA: 0x00099E8C File Offset: 0x0009808C
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

		// Token: 0x06002643 RID: 9795 RVA: 0x00099EE4 File Offset: 0x000980E4
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

		// Token: 0x06002644 RID: 9796 RVA: 0x00099F3C File Offset: 0x0009813C
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

		// Token: 0x06002645 RID: 9797 RVA: 0x00099F94 File Offset: 0x00098194
		protected void StartingAgeYoungOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.YoungAdult;
		}

		// Token: 0x06002646 RID: 9798 RVA: 0x00099F9E File Offset: 0x0009819E
		protected void StartingAgeAdultOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.Adult;
		}

		// Token: 0x06002647 RID: 9799 RVA: 0x00099FA8 File Offset: 0x000981A8
		protected void StartingAgeMiddleAgedOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.MiddleAged;
		}

		// Token: 0x06002648 RID: 9800 RVA: 0x00099FB2 File Offset: 0x000981B2
		protected void StartingAgeElderlyOnApply(CharacterCreation characterCreation)
		{
			this._startingAge = WoTCharacterCreation.SandboxAgeOptions.Elder;
		}

		// Token: 0x06002649 RID: 9801 RVA: 0x00099FBC File Offset: 0x000981BC
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

		// Token: 0x0600264A RID: 9802 RVA: 0x0009A0B9 File Offset: 0x000982B9
		protected void SetHeroAge(float age)
		{
			Hero.MainHero.SetBirthDay(CampaignTime.YearsFromNow(-age));
		}

		protected void OriginOnInit(CharacterCreation characterCreation)
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

		// Token: 0x04000D42 RID: 3394
		protected const int FocusToAddYouthStart = 2;

		// Token: 0x04000D43 RID: 3395
		protected const int FocusToAddAdultStart = 4;

		// Token: 0x04000D44 RID: 3396
		protected const int FocusToAddMiddleAgedStart = 6;

		// Token: 0x04000D45 RID: 3397
		protected const int FocusToAddElderlyStart = 8;

		// Token: 0x04000D46 RID: 3398
		protected const int AttributeToAddYouthStart = 1;

		// Token: 0x04000D47 RID: 3399
		protected const int AttributeToAddAdultStart = 2;

		// Token: 0x04000D48 RID: 3400
		protected const int AttributeToAddMiddleAgedStart = 3;

		// Token: 0x04000D49 RID: 3401
		protected const int AttributeToAddElderlyStart = 4;

		// Token: 0x04000D4A RID: 3402
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

		// Token: 0x04000D4B RID: 3403
		protected WoTCharacterCreation.SandboxAgeOptions _startingAge = WoTCharacterCreation.SandboxAgeOptions.YoungAdult;

		// Token: 0x04000D4C RID: 3404
		protected WoTCharacterCreation.OccupationTypes _familyOccupationType;

		// Token: 0x04000D4D RID: 3405
		protected TextObject _educationIntroductoryText = new TextObject("{=!}{EDUCATION_INTRO}", null);

		// Token: 0x04000D4E RID: 3406
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
