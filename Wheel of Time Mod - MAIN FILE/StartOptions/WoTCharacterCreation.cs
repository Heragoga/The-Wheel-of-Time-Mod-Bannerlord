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

	public class WoTCharacterCreation : CharacterCreationContentBase
	{
		
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
			CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(Aiel_condition);


			//Aiel

			characterCreationCategory1.AddCategoryOption(new TextObject("an offshot of a cheftan's family", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_chieftan_OnApply, Aiel_chieftan_OnApply, new TextObject("Your uncle was a chieftan of a smaller sept, a great opportunity for you! He took you under his wing, showed you what governing was about and let you do minor tasks around the management of the sept. Who knows, maybe some day you'll take his position.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory1.AddCategoryOption(new TextObject("born into a family of farmers", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Throwing
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_farmer_OnApply, Aiel_farmer_OnApply, new TextObject("Although many think Aiel are all warriors, they are mistaken. There are people like you as well, who for generations tend to the gardens in the hold, producing all kinds of food to fuel the empty bellys of the renowned warriors.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("a part of a craftsmans family", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Crafting
				},
				DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_craftsmen_OnApply, Aiel_craftsmen_OnApply, new TextObject("In the forges and shops of a hold your family was to be found, ernest folk who earned their living not by killing and stealing, but by crafting and building. An added bonus was the status some of you had as blacksmiths, giving you the possibility not to fight at all, but to live in peace and focus on your craft.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("closely connected to a wise one", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_wisefolk_OnApply, Aiel_wisefolk_OnApply, new TextObject("Wise ones were the dispensers of peace and herbes in the life of Aiel. You had the luck of beeing kin of a relativly known Wise one, you learned a great deal from hear, even inheriting the talent to channel.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory1.AddCategoryOption(new TextObject("born into a family of warriors", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.Polearm
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_raiders_OnApply, Aiel_raiders_OnApply, new TextObject("Although most soldiers came from the ordinary folk, you were born into a family of warriors who did war for a living, raiding mostly, as the feuds between cland usually went. A bloody and cruel buisness, but it granted you a lot of experience.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("kin to wasteland traders", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Engineering
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_traiders_OnApply, Aiel_traiders_OnApply, new TextObject("You crisscrossed the waste, buying and selling wares from one hold to another. Gold, jewlery and especially books were all wares, everybody needed.", null), null, 0, 0, 0, 0, 0);


			
			//Shadowspawn Origin

			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(default_condition);
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


			//aiel

			
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

		// aiel origin applys

		protected void Aiel_chieftan_OnApply(CharacterCreation characterCreation)
		{
			ChangePlayerOutfit(characterCreation, "rich_CC_chieftan_aiel");
			clothing = "rich_CC_chieftan_aiel";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Aiel_farmer_OnApply(CharacterCreation characterCreation)
		{
            if (Hero.MainHero.IsFemale)
            {
				ChangePlayerOutfit(characterCreation, "poor_CC_farmers_aiel_F");
				clothing = "poor_CC_farmers_aiel_F";
			}
            else
            {
				ChangePlayerOutfit(characterCreation, "poor_CC_farmers_aiel_M");
				clothing = "poor_CC_farmers_aiel_M";
			}

			
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Aiel_craftsmen_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "middle_CC_craftsmen_aiel_F");
				clothing = "middle_CC_craftsmen_aiel_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "middle_CC_craftsmen_aiel_M");
				clothing = "middle_CC_craftsmen_aiel_M";
			}


			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Aiel_wisefolk_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "channeler_CC_wisefolk_aiel_F");
				clothing = "channeler_CC_wisefolk_aiel_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "channeler_CC_wisefolk_aiel_M");
				clothing = "channeler_CC_wisefolk_aiel_M";
			}


			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}

		protected void Aiel_raiders_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "warrior_CC_raider_aiel_F");
				clothing = "warrior_CC_raider_aiel_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "warrior_CC_raider_aiel_M");
				clothing = "warrior_CC_raider_aiel_M";
			}


			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Aiel_traiders_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "rich2_CC_trader_aiel_F");
				clothing = "rich2_CC_trader_aiel_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "rich2_CC_trader_aiel_M");
				clothing = "rich2_CC_trader_aiel_M";
			}


			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected bool default_condition()
        {
            if (!Aiel_condition())
            {
				return true;
            }
			return false;
        }
		protected bool Aiel_condition()
		{
			return base.GetSelectedCulture().StringId == "khuzait";

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
		}
		protected void SpawnOnInit(CharacterCreation characterCreation)
		{
			characterCreation.IsPlayerAlone = true;
			characterCreation.HasSecondaryCharacter = false;
		}

	

		

	
		protected WoTCharacterCreation.SandboxAgeOptions _startingAge = WoTCharacterCreation.SandboxAgeOptions.YoungAdult;

		
		protected TextObject _educationIntroductoryText = new TextObject("{=!}{EDUCATION_INTRO}", null);

		
		protected TextObject _youthIntroductoryText = new TextObject("{=!}{YOUTH_INTRO}", null);

		
		protected enum SandboxAgeOptions
		{
			
			YoungAdult = 20,
			
			Adult = 30,
			
			MiddleAged = 40,
			
			Elder = 50
		}

		
	}
}
