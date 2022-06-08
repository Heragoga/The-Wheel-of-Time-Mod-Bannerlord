using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace WoT_Main
{
	// Token: 0x02000222 RID: 546
	public class WoTCharacterCreation : CharacterCreationContentBase
	{
		// Token: 0x17000A22 RID: 2594
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x00095B19 File Offset: 0x00093D19
		private string clothing = "";
		private string weapon = "";
		private bool channeler = false;
		private Vec2 positionToSpawnTo;
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
			
			MobileParty.MainParty.Position2D = positionToSpawnTo;
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
			this.AddTalentMenu(characterCreation);
			this.AddSpawnMenu(characterCreation);
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
			characterCreationCategory.AddCategoryOption(new TextObject("kidnapped as a child", null), effectedSkills1, effectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Kidnapped_Child_Origin_On_Apply, Shadowspawn_Origin_Kidnapped_Child_Origin_On_Apply, new TextObject("As a child you were draged from your home village by the forces of the shadow. You have spent most of your life in the company of others, involuntarely working for the shadow.", null), null, 0, 0, 0, 0, 0);

			List<SkillObject> effectedSkills2 = new List<SkillObject>
			{
				DefaultSkills.OneHanded,
				DefaultSkills.Athletics
			};
			CharacterAttribute effectedAttribute2 = DefaultCharacterAttributes.Vigor;
			characterCreationCategory.AddCategoryOption(new TextObject("born of trollocs", null), effectedSkills2, effectedAttribute2, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Trolloc_Origin_On_Apply, Shadowspawn_Origin_Trolloc_Origin_On_Apply, new TextObject("In a dirty trolloc-breeding pit you first saw light. As one of many siblings you began your life as the cheap, worthless monsters the shadow used as their main force, yet you were better than others, you survived when most died.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new TextObject("born in a family of darkfriends", null), new List<SkillObject>{
				DefaultSkills.Roguery,
				DefaultSkills.Bow
			},
			DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Darkfriends_Origin_On_Apply, Shadowspawn_Origin_Darkfriends_Origin_On_Apply, new TextObject("Your family stood in the service of the dark one for a long time. So do you, a young darkfriend, convinced of the ideals of the shadow since childhood, ready to give his life for the dark one.", null), null, 0, 0, 0, 0, 0);


           
				characterCreationCategory.AddCategoryOption(new TextObject("a member of the black Ajah / a male Aiel channeler", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Medicine
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Channeler_Origin_On_Apply, Shadowspawn_Origin_Channeler_Origin_On_Apply, new TextObject("As soon as you obtained the shal, you joined the black Ajah. The reasons were many: the normal sisters simply didn't understand you, you despised the laws forbidding research in dark parts of channeling and either way the lord of chaos was your prefered master, not some mortal Amyrlin. / Involuntarily or out of your free will you landed in the ranks of the Samma N'Sei, channeling the one power for the dark one.", null), null, 0, 0, 0, 0, 0);
			
			
			

				characterCreationCategory.AddCategoryOption(new TextObject("a daughter/son of a dreadlord", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Dreadlord_Origin_On_Apply, Shadowspawn_Origin_Dreadlord_Origin_On_Apply, new TextObject("You were part of the ruling class of the shadowspawn. Your father, a powerful dreadlord insured an excellent childhood, away from slaves, trollocs and filth. You were tought in many disciplines, mainly how to command and organise troops.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("an offspring of slaves", null), new List<SkillObject>{
				DefaultSkills.Roguery,
				DefaultSkills.Polearm
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Slaves_Origin_On_Apply, Shadowspawn_Origin_Slaves_Origin_On_Apply, new TextObject("Your life was hard, working from morning till dawn. As an offspring of slaves you had no saying in what was done to you, your darkfriend-owner decided everything about you.", null), null, 0, 0, 0, 0, 0);


			characterCreation.AddNewMenu(characterCreationMenu);
		}
		protected void AddProfessionMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Profession", null), new TextObject("You earned your living as a...", null), new CharacterCreationOnInit(this.ProfessionOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);


			characterCreationCategory.AddCategoryOption(new TextObject("as a farmer", null), new List<SkillObject>{
				DefaultSkills.Scouting,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Profession_farmer_consequence, null, new TextObject("Tilling, sowing and harvesting, this was the rythm of your life. An honest, though boring job.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a blacksmith's apprentice", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.Engineering
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Profession_blacksmith_consequence, null, new TextObject("Your village's blacksmith took you under his hood, teaching you everaything you need.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a hunter in the woods", null), new List<SkillObject>{
				DefaultSkills.Scouting,
				DefaultSkills.Bow
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Profession_hunter_consequence, null, new TextObject("Hunting all kinds of animals was your main profession, laying traps, sniffing out gain, a tedious yet rewarding job.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a merchant's assistant", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Profession_trader_consequence, null, new TextObject("Travelling from town to town, buying and selling, this is how you pased your days, a job which gave you a lot of profit, yet involved the risks of the dangerous roads.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a fisher", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Medicine
				},
				DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_fisher_condition, Profession_fisher_consequence, null, new TextObject("On a boat you cought fishes, making a living out of it.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a sheep herder", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Bow
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_sheep_condition, Profession_sheep_consequence, null, new TextObject("In the wide planes you herded sheep, defending them from wolfs with the help of your trusty bow and dog.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a trolloc breeder", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Bow
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_trolloc_condition, Profession_trolloc_consequence, null, new TextObject("You watched over the trolloc breeding pits, an ugly job which always left you quesy afterwards.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("as a engineer on fortification", null), new List<SkillObject>{
				DefaultSkills.Engineering,
				DefaultSkills.Tactics
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_fortify_condition, Profession_fortify_consequence, null, new TextObject("You fortified towns, villages and camps with your crew, getting a good feal of how to defend your fortifications and how to attack them.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a raider and bandit", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_raider_condition, Profession_raider_consequence, null, new TextObject("As a bandit you scowerd the lands, raping, robing and killing. 'Take everything and leave nothing', this was your moto.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a pirate", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Crossbow
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_pirate_condition, Profession_pirate_consequence, null, new TextObject("On a boat under the service of your captain you raided and boarded smaller traiding vehicles, even plundering a village on accasion.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a stable hand", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Throwing
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_horse_condition, Profession_horse_consequence, null, new TextObject("You worked at stables, grooming, riding and caring horses. A great job your employers said to you, until realised everything was about clearing away shit.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("as a healer", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Profession_healer_condition, Profession_healer_consequence, null, new TextObject("You were your villages hedge doctor's/wisdom's apprentice, learning the use of herbs to heal wounds and sicknesses.", null), null, 0, 0, 0, 0, 0);

			characterCreation.AddNewMenu(characterCreationMenu);
		}
		protected void AddTalentMenu(CharacterCreation characterCreation)
		{
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Talent", null), new TextObject("You were exceptionaly well with...", null), new CharacterCreationOnInit(this.TalentOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);


			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(Channeler_on_condition);
			characterCreationCategory.AddCategoryOption(new TextObject("fireballs", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_Fireball_Consequence, null, new TextObject("You were very well with fireballs, making your enemys burn.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("windstorms", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_Windstorms_Consequence, null, new TextObject("You were very well with windstorms, bundling the wind to blast your enemys to oblivion.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("lightning", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.TwoHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_Lightning_Consequence, null, new TextObject("You were very well with lightning, making the sky burn your enemys to a cinder like Zeus.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("rolling earth and fire", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Polearm
				},
			DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_Earth_Consequence, null, new TextObject("You were very well with rolling earth and fire, making the ground virtaly consume your enemys.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("an airshield", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Bow
				},
			DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_Shield_Consequence, null, new TextObject("You were very well withan airshield, creating a protective shield of air.", null), null, 0, 0, 0, 0, 0);




			CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(NO_Channeler_on_condition);
			characterCreationCategory1.AddCategoryOption(new TextObject("a spear", null), new List<SkillObject>{
				DefaultSkills.Polearm,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_Spear_Consequence, null, new TextObject("You were the best man to have in a spear wall.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("a short bow", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_Bow_Consequence, null, new TextObject("You could manage to hit a target a mile away, some said, either way every bow-competition ended with your victory.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("a crossbow", null), new List<SkillObject>{
				DefaultSkills.Crossbow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_CrossBow_Consequence, null, new TextObject("In a crossbowline you were the fastest one to reload and the fastest one to hit flesh.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("a throwing spear", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Talent_throwing_Consequence, null, new TextObject("No matter the size, shape or weight of a spear, you could throw it anywhere precisly.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("a sword", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.TwoHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, No_Channeler_but_horse_on_condition, Talent_sword_Consequence, null, new TextObject("Some called you almost a blademaster, you were that good with a sword, winning every duel.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("on horseback", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, No_Channeler_but_horse_on_condition, Talent_horse_Consequence, null, new TextObject("As soon as you were on horseback you were an enemy few would dare to face.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("with a buckler", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, No_Channeler_no_horse_on_condition, Talent_buckler_Consequence, null, new TextObject("With a hide buckler you could fend of almost any attack.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("with a longbow", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, No_Channeler_no_horse_on_condition, Talent_horse_Consequence, null, new TextObject("You best shot with a long bow over great distances.", null), null, 0, 0, 0, 0, 0);


			characterCreation.AddNewMenu(characterCreationMenu);
		}
		protected void AddSpawnMenu(CharacterCreation characterCreation)
		{

			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Spawn Location", null), new TextObject("Your journey starts in...", null), new CharacterCreationOnInit(SpawnOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);


			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(null);
			
			characterCreationCategory.AddCategoryOption(new TextObject("the blight", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Blight_Spawn, new TextObject("You start near shayol ghul.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("the waste", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Waste_Spawn, new TextObject("You start near Ruidian", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("andor", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Andor_Spawn, new TextObject("You start near Camelyn", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("Mayene", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Mayene_Spawn, new TextObject("You start near the city of Mayene.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("Malkier", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Malkier_Spawn, new TextObject("You start near the remains of the seven towers.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("the White Tower", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, WT_Spawn, new TextObject("You start near the white tower.", null), null, 0, 0, 0, 0, 0);


			characterCreation.AddNewMenu(characterCreationMenu);
		}
		protected void Blight_Spawn(CharacterCreation characterCreation)
        {
			positionToSpawnTo = new Vec2(566.8f, 741.63f);
        }
		protected void Waste_Spawn(CharacterCreation characterCreation)
		{
			positionToSpawnTo = new Vec2(721.3f, 345.3f);
		}
		protected void Andor_Spawn(CharacterCreation characterCreation)
		{
			positionToSpawnTo = new Vec2(480.53f, 287.61f);
		}
		protected void Mayene_Spawn(CharacterCreation characterCreation)
		{
			positionToSpawnTo = new Vec2(672.2f, 87.9f);
		}
		protected void Malkier_Spawn(CharacterCreation characterCreation)
		{
			positionToSpawnTo = new Vec2(655.55f, 671.41f);
		}
		protected void WT_Spawn(CharacterCreation characterCreation)
		{
			positionToSpawnTo = new Vec2(534.47f, 428.15f);
		}

		protected void Shadowspawn_Origin_Kidnapped_Child_Origin_On_Apply(CharacterCreation characterCreation)
        {
			ChangePlayerOutfit(characterCreation, "poor_CC_1");
			clothing = "poor_CC_1";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		
		protected void Shadowspawn_Origin_Trolloc_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "trolloc_CC_1");
			clothing = "trolloc_CC_1";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		
		protected void Shadowspawn_Origin_Darkfriends_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "middle_CC_1");
			clothing = "middle_CC_1";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		
		protected void Shadowspawn_Origin_Channeler_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "channeler_CC_1");
			clothing = "channeler_CC_1";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}
		protected void Shadowspawn_Origin_Dreadlord_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "rich_CC_1");
			clothing = "rich_CC_1";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}
		protected void Shadowspawn_Origin_Slaves_Origin_On_Apply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "poor_CC_2");
			clothing = "poor_CC_2";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
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

		protected void Profession_farmer_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "peasant_2haxe_1_t1", true, "");
		}
		protected void Profession_trader_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "lowland_throwing_knife", true, "");
		}
		protected void Profession_blacksmith_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
		}
		protected void Profession_hunter_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "hunting_bow", true, "");
		}
		protected void Profession_fisher_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "northern_javelin_1_t2", true, "");
		}

		protected void Profession_sheep_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
		}
		protected void Profession_trolloc_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "western_javelin_3_t4", true, "");
		}
		protected void Profession_fortify_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_2_t1", true, "");
		}

		protected void Profession_raider_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "aserai_axe_1_t2", true, "");
		}
		protected void Profession_pirate_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "crossbow_a", true, "");
		}
		protected void Profession_horse_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "horse_whip", true, "");
		}
		protected void Profession_healer_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "the_scalpel_sword_t3", true, "");
		}


		protected bool Channeler_on_condition()
		{
			return channeler;
		}
		protected bool NO_Channeler_on_condition()
		{
			return !channeler;
		}
		protected bool No_Channeler_but_horse_on_condition()
		{
			return !channeler && base.GetSelectedCulture().StringId != "khuzait";
		}
		protected bool No_Channeler_no_horse_on_condition()
		{
			return !channeler && base.GetSelectedCulture().StringId == "khuzait";
		}

		
		protected void Talent_Fireball_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerblossomsoffiresingle";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Windstorms_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerwindstorm1FireBall";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Lightning_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerwindstorm1Lightningammo";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Earth_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerwindstormearthammo";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Shield_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "OnePowerAirShield";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Spear_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "western_spear_1_t2";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Bow_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "steppe_bow";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_CrossBow_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "crossbow_a";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_throwing_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "western_javelin_1_t2";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_sword_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "vlandia_sword_1_t2";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_horse_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "aserai_mace_3_t3";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_buckler_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "Aiel_shield";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_longbow_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "Aielbowlevel1";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}


		
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

	
		protected void RefreshPropsAndClothing(CharacterCreation characterCreation, bool isChildhoodStage, string itemId, bool isLeftHand, string secondItemId = "")
		{
			characterCreation.ClearFaceGenPrefab();
			characterCreation.ClearCharactersEquipment();
			string text = clothing;
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
	
		// Token: 0x06002603 RID: 9731 RVA: 0x00099788 File Offset: 0x00097988
		protected void RefreshPlayerAppearance(CharacterCreation characterCreation)
		{
			
			this.ChangePlayerOutfit(characterCreation, clothing);
			this.ApplyEquipments(characterCreation);
		}

		

	
		

		protected void ApplyEquipments(CharacterCreation characterCreation)
		{
			WoTCharacterCreation.ClearMountEntity(characterCreation);
			string text = clothing;
			MBEquipmentRoster @object = Game.Current.ObjectManager.GetObject<MBEquipmentRoster>(text);
			base.PlayerStartEquipment = (((@object != null) ? @object.DefaultEquipment : null) ?? MBEquipmentRoster.EmptyEquipment);
			base.PlayerCivilianEquipment = (((@object != null) ? @object.GetCivilianEquipments().FirstOrDefault<Equipment>() : null) ?? MBEquipmentRoster.EmptyEquipment);
			if (base.PlayerStartEquipment != null && base.PlayerCivilianEquipment != null)
			{
				ItemObject @object1 = Game.Current.ObjectManager.GetObject<ItemObject>(weapon);
				base.PlayerStartEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon1, new EquipmentElement(@object1, null, null, false));
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
		protected void TalentOnInit(CharacterCreation characterCreation)
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
		
		protected void ProfessionOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			//characterCreation.ClearFaceGenPrefab();
			//characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this.AccomplishmentAge, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			//this.RefreshPlayerAppearance(characterCreation);
		}
		protected void SpawnOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
			//characterCreation.ClearFaceGenPrefab();
			//characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this.AccomplishmentAge, "act_childhood_schooled"));
			characterCreation.ChangeCharsAnimation(new List<string>
			{
				"act_childhood_schooled"
			});
			//this.RefreshPlayerAppearance(characterCreation);
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
