using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.Extensions;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;
using WoT_Main.Support;

namespace WoT_Main
{

	public class WoTCharacterCreation : CharacterCreationContentBase
	{
		
		private string clothing = "";
		private string weapon = "";
		private string amunition = "none";
		private bool channeler = false;
		private Vec2 positionToSpawnTo;

		public WoTCharacterCreation()
        {
			campaignSupport.displayMessageInChat("Don't forget to scroll down for more cultures!");
			campaignSupport.displayMessageInChat("Certain choices in the origin menu will enable you to select different channeling weapons, they are easy to spot.");
		}

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
				mapState.Handler.ResetCamera(true, true);
				mapState.Handler.TeleportCameraToMainParty();
			}
			this.SetHeroAge((float)this._startingAge);
		}

		
		protected override void OnInitialized(CharacterCreation characterCreation)
		{
			this.AddOriginMenu(characterCreation);
			this.AddProfessionMenu(characterCreation);
			this.AddTalentMenu(characterCreation);
			this.AddSpawnMenu(characterCreation);
		}

		
		protected override void OnApplyCulture()
		{
		}


		protected void AddOriginMenu(CharacterCreation characterCreation)
        {

			
			CharacterCreationMenu characterCreationMenu = new CharacterCreationMenu(new TextObject("Origin", null), new TextObject("You were...", null), new CharacterCreationOnInit(this.OriginOnInit), CharacterCreationMenu.MenuTypes.MultipleChoice);
			CharacterCreationCategory characterCreationCategory1 = characterCreationMenu.AddMenuCategory(Aiel_condition);


			//Aiel

			characterCreationCategory1.AddCategoryOption(new TextObject("an offshot of a cheftan's family", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_chieftan_OnApply, Aiel_chieftan_OnApply, new TextObject("Your uncle was a chieftan of a smaller sept, a great opportunity for you! He took you under his wing, showed you what governing was all about and let you do minor tasks around the management of his sept. Who knows, maybe you'll someday take his position.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory1.AddCategoryOption(new TextObject("born into a family of farmers", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Throwing
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_farmer_OnApply, Aiel_farmer_OnApply, new TextObject("Although many think Aiel are all warriors, there are people who for generations tend gardens of a hold, producing all kinds of food to fuel the empty bellys of the renowned warriors.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("a part of a craftsmans family", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Crafting
				},
				DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_craftsmen_OnApply, Aiel_craftsmen_OnApply, new TextObject("In the forges and shops of a hold your family was to be found, honest folk who earned their living not by killing and stealing, but by crafting and building.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("closely connected to a wise one", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_wisefolk_OnApply, Aiel_wisefolk_OnApply, new TextObject("Wise ones were the dispensers of peace and herbes in the life of the Aiel. You had the luck of beeing kin of a relativly powerfull Wise one, you learned a great deal from hear, even inheriting her talent to channel.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory1.AddCategoryOption(new TextObject("born into a family of warriors", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.Polearm
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_raiders_OnApply, Aiel_raiders_OnApply, new TextObject("Although most soldiers came from the ordinary folk, you were born into a family of warriors who only did war for a living, raiding mostly, as the feuds between clans usually were fought out. A bloody and cruel buisness, but it granted you a lot of experience.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory1.AddCategoryOption(new TextObject("kin to wasteland traders", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Engineering
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Aiel_traiders_OnApply, Aiel_traiders_OnApply, new TextObject("You crisscrossed the waste, buying and selling wares from one hold to another. Gold, jewlery and especially books were all wares, with the help of which many fortunes were made.", null), null, 0, 0, 0, 0, 0);


			
			//Shadowspawn Origin

			CharacterCreationCategory characterCreationCategory = characterCreationMenu.AddMenuCategory(Shadowspawn_condition);
			List<SkillObject> effectedSkills1 = new List<SkillObject>
			{
				DefaultSkills.Riding,
				DefaultSkills.Polearm
			};
			CharacterAttribute effectedAttribute = DefaultCharacterAttributes.Vigor;
			characterCreationCategory.AddCategoryOption(new TextObject("kidnapped as a child", null), effectedSkills1, effectedAttribute, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Kidnapped_Child_Origin_On_Apply, Shadowspawn_Origin_Kidnapped_Child_Origin_On_Apply, new TextObject("As a child you were draged from your home village by the forces of the shadow. You have spent most of your life in the company of others, being forced to work for the shadow, yet you managed to escape, even surviving a dangerous journey through the blight. Now you stand ragged but free and seek only one thing: revenge against the shadow and your captores.", null), null, 0, 0, 0, 0, 0);

			List<SkillObject> effectedSkills2 = new List<SkillObject>
			{
				DefaultSkills.OneHanded,
				DefaultSkills.Athletics
			};
			CharacterAttribute effectedAttribute2 = DefaultCharacterAttributes.Vigor;
			characterCreationCategory.AddCategoryOption(new TextObject("born of trollocs", null), effectedSkills2, effectedAttribute2, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Trolloc_Origin_On_Apply, Shadowspawn_Origin_Trolloc_Origin_On_Apply, new TextObject("In a dirty trolloc-breeding pit you first saw light. As one of many siblings you began your life as a cheap, worthless monsters the shadow used in their main fighting force, yet you were better than others, you survived when most died and were an exceptional fighter among trollocs.", null), null, 0, 0, 0, 0, 0);

            characterCreationCategory.AddCategoryOption(new TextObject("born in a family of darkfriends", null), new List<SkillObject>{
				DefaultSkills.Roguery,
				DefaultSkills.Bow
			},
			DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Darkfriends_Origin_On_Apply, Shadowspawn_Origin_Darkfriends_Origin_On_Apply, new TextObject("Your family stood in the service of the dark one for a long time. So do you, a young darkfriend, convinced of the ideals of the shadow since childhood, ready to give his life for the lord of chaos.", null), null, 0, 0, 0, 0, 0);


           
				characterCreationCategory.AddCategoryOption(new TextObject("a member of the black Ajah", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Medicine
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, Shadowspawn_Origin_Channeler_Origin_On_Apply, Shadowspawn_Origin_Channeler_Origin_On_Apply, new TextObject("As soon as you obtained the shal, you joined the black Ajah. The reasons were many: the normal sisters simply didn't understand you, you despised the laws forbidding research in dark parts of channeling and either way you prefered the lord of chaos over some mortal Amyrlin.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("a Samma N'Sei", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Medicine
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, Shadowspawn_Origin_Channeler_Origin_On_Apply, Shadowspawn_Origin_Channeler_Origin_On_Apply, new TextObject("Involuntarily or out of your free will you landed in the ranks of the Samma N'Sei, channeling the one power for the dark one.", null), null, 0, 0, 0, 0, 0);




			characterCreationCategory.AddCategoryOption(new TextObject("a daughter of a dreadlord", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, Shadowspawn_Origin_Dreadlord_Origin_On_Apply, Shadowspawn_Origin_Dreadlord_Origin_On_Apply, new TextObject("You were part of the ruling class of the shadowspawn. Your father, a powerful dreadlord ensured an excellent childhood, away from slaves, trollocs and filth. You were tought in many disciplines, mostly how to command and organise troops.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("a son of a dreadlord", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Steward
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, Shadowspawn_Origin_Dreadlord_Origin_On_Apply, Shadowspawn_Origin_Dreadlord_Origin_On_Apply, new TextObject("You were part of the ruling class of the shadowspawn. Your father, a powerful dreadlord ensured an excellent childhood, away from slaves, trollocs and filth. You were tought in many disciplines, mostly how to command and organise troops.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategory.AddCategoryOption(new TextObject("an offspring of slaves", null), new List<SkillObject>{
				DefaultSkills.Roguery,
				DefaultSkills.Polearm
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Shadowspawn_Origin_Slaves_Origin_On_Apply, Shadowspawn_Origin_Slaves_Origin_On_Apply, new TextObject("Your life was hard, working from morning till dawn. As an offspring of slaves you had no saying in what was done to you, your darkfriend-owner decided everything about you.", null), null, 0, 0, 0, 0, 0);


			


			//default

			CharacterCreationCategory characterCreationCategoryDefault = characterCreationMenu.AddMenuCategory(default_condition);


			

			characterCreationCategoryDefault.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, one of them inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDefault.AddCategoryOption(new TextObject("a relative of a wisdom", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("As a relative of your villages wisdom you had a good living, learning a lot from her. You even inherited the ability to channel from her.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDefault.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDefault.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDefault.AddCategoryOption(new TextObject("an offspring of a noble", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_rich_1_OnApply, default_rich_1_OnApply, new TextObject("You were a fourth child of some minor country nobleman, which meant you couldn't inherit his estates and were left with two options: stay with one of your brothers, looking after the estates as a squire or going of on a life of adventure. You decided on the second option.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDefault.AddCategoryOption(new TextObject("born into a family of shopkeepers", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_middle_1_OnApply, default_middle_1_OnApply, new TextObject("You were part of the town's middle class, helping out in your family's shop all your life, offering sevices of different kinds to your clients and making a decent living of it.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDefault.AddCategoryOption(new TextObject("a bandit child", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_warrior_1_OnApply, default_warrior_1_OnApply, new TextObject("Your family was driven out of their farm a long time ago, adopting a new life of banditry. You lived in camps all your life, imitating your parents, brothers and uncles in their ways. When you didn't have the opportunity to accept a job from some gang leader you robbed people on the road.", null), null, 0, 0, 0, 0, 0);

			//White tower

			CharacterCreationCategory characterCreationCategoryWT = characterCreationMenu.AddMenuCategory(WT_condition);




			characterCreationCategoryWT.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategoryWT.AddCategoryOption(new TextObject("a daughter of an Aes Sedai and Warder", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, WT_channeler_1_OnApply, WT_channeler_1_OnApply, new TextObject("You were a rare child of an Aes Sedai and her warder. You joined the white tower as your mother did as soon as you could, benefiting from a lot of advantages due to your mothers position.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryWT.AddCategoryOption(new TextObject("a son of an Aes Sedai and Warder", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, WT_channeler_1_OnApply, WT_channeler_1_OnApply, new TextObject("You were a rare child of an Aes Sedai and her warder, yet many in the White Tower still believe the taint is present and so your mother tried to drown you. Your father managed to save you in secret, giving you to a friend to be raised. As soon as you could speak, you sweared vengance, will you realise that promise?", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryWT.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryWT.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryWT.AddCategoryOption(new TextObject("an offspring of a noble", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_rich_1_OnApply, default_rich_1_OnApply, new TextObject("You were a fourth child of some minor country nobleman, which meant you couldn't inherit his estates and were left with two options: stay with one of your brothers, looking after the estates as a squire or going of on a life of adventure. You decided on the second option.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryWT.AddCategoryOption(new TextObject("born into a family of shopkeepers", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_middle_1_OnApply, default_middle_1_OnApply, new TextObject("You were part of the town's middle class, helping out in your family's shop all your life, offering sevices of different kinds to your clients and making a decent living of it.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryWT.AddCategoryOption(new TextObject("an offspring of a turned down novice", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, WT_channeler_2_OnApply, WT_channeler_2_OnApply, new TextObject("Your mother tried to become an Aes Sedai, yet failed due to a lack of skill and dicipline. Yet she caried the spark to you, giving you the ability to channel. Will you try at what your mother failed?", null), null, 0, 0, 0, 0, 0);



			//BlackTower
			CharacterCreationCategory characterCreationCategoryBT = characterCreationMenu.AddMenuCategory(BT_condition);




			characterCreationCategoryBT.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategoryBT.AddCategoryOption(new TextObject("a daughter of an Asha'man", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, BT_channeler_2_OnApply, BT_channeler_2_OnApply, new TextObject("You are a child of an Asha'man and his bonded Aes Sedai, yet you decided against training in the black tower. You thought your family tought you enough about channeling to survive.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBT.AddCategoryOption(new TextObject("a son of an Asha'man", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, BT_channeler_2_OnApply, BT_channeler_2_OnApply, new TextObject("You are a child of an Asha'man and his bonded Aes Sedai, yet you decided against training in the black tower. You thought your family tought you enough about channeling to survive.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBT.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBT.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBT.AddCategoryOption(new TextObject("an offspring of a noble", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_rich_1_OnApply, default_rich_1_OnApply, new TextObject("You were a fourth child of some minor country nobleman, which meant you couldn't inherit his estates and were left with two options: stay with one of your brothers, looking after the estates as a squire or going of on a life of adventure. You decided on the second option.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBT.AddCategoryOption(new TextObject("born into a family of shopkeepers", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_middle_1_OnApply, default_middle_1_OnApply, new TextObject("You were part of the town's middle class, helping out in your family's shop all your life, offering sevices of different kinds to your clients and making a decent living of it.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBT.AddCategoryOption(new TextObject("a black tower initiate", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, BT_channeler_1_OnApply, BT_channeler_1_OnApply, new TextObject("As soon as you could, you joined the black tower to learn to channel and eventually become an Asha'man.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategoryBT.AddCategoryOption(new TextObject("a black tower initiate", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, BT_channeler_1_OnApply, BT_channeler_1_OnApply, new TextObject("You are a female channeler in the black tower, a rarity, yet you were thrown out of the white tower for a misshap and now seek your revenge here. You train with the bonded Aes Sedai and seek to become one of them or bond your own Asha'man.", null), null, 0, 0, 0, 0, 0);

			// dragonsworn
			CharacterCreationCategory characterCreationCategoryDragon = characterCreationMenu.AddMenuCategory(Dragonsworn_condition);




			characterCreationCategoryDragon.AddCategoryOption(new TextObject("a former follower of the Prophet", null), new List<SkillObject>{
				DefaultSkills.Polearm,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Dragon_warrior_2_OnApply, Dragon_warrior_2_OnApply, new TextObject("When the Prophet first said his preechings you were among many who began to follow him. At first you enjoyed it, yet soon you realised what it was getting too: an immense crowd of fanatic bandits who used every excuse to rape, steal and murder. You left the Prophet's mobs and instead joined the forces of the actual Dragon Reborn.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDragon.AddCategoryOption(new TextObject("a relative of a wisdom", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("As a relative of your villages wisdom you had a good living, learning a lot from her. You even inherited the ability to channel from her, incredible luck.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDragon.AddCategoryOption(new TextObject("a daughter of a dead Legion of the Dragon officer", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.TwoHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, Dragon_warrior_1_OnApply, Dragon_warrior_1_OnApply, new TextObject("Your father joined the Legion of the Dragon at it's creation and managed to work his way up through the ranks. He died as a Captain, in some unknown skirmish, leaving you only his old armour.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDragon.AddCategoryOption(new TextObject("a son of a dead Legion of the Dragon officer", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.TwoHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, Dragon_warrior_1_OnApply, Dragon_warrior_1_OnApply, new TextObject("Your father joined the Legion of the Dragon at it's creation and managed to work his way up through the ranks. He died as a Captain, in some unknown skirmish, leaving you only his old armour.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDragon.AddCategoryOption(new TextObject("a Two Rivers bowman", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, Dragon_warrior_3_OnApply, Dragon_warrior_3_OnApply, new TextObject("You left your farm in the Two Rivers to join your countryman Rand, the dragon in his glorious conquests, hoping to be treated better, because you come from the same region.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDragon.AddCategoryOption(new TextObject("a Two Rivers bow-woman", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, Dragon_warrior_3_OnApply, Dragon_warrior_3_OnApply, new TextObject("You left your farm in the Two Rivers to join your countryman Rand, the dragon in his glorious conquests, hoping to be treated better, because you come from the same region.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDragon.AddCategoryOption(new TextObject("born into a family of shopkeepers", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_middle_1_OnApply, default_middle_1_OnApply, new TextObject("You were part of the town's middle class, helping out in your family's shop all your life, offering sevices of different kinds to your clients and making a decent living of it.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryDragon.AddCategoryOption(new TextObject("a recruit of the Band of the Red Hand", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Tactics
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Dragon_warrior_4_OnApply, Dragon_warrior_4_OnApply, new TextObject("The second you heared the songs about war, the soldiers of Matt's band sung as they marched through your town, you knew where you belonged: in the Band. You joined immideatly, hoping to follow Matt's example and maybe become an officer or even lead your own band one day.", null), null, 0, 0, 0, 0, 0);


			//borderlands
			CharacterCreationCategory characterCreationCategoryBorderlands = characterCreationMenu.AddMenuCategory(Borderlands_condition);




			characterCreationCategoryBorderlands.AddCategoryOption(new TextObject("a relative of a noble", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Borderlands_warrior_1_OnApply, Borderlands_warrior_1_OnApply, new TextObject("A close relative of yours was a minor noble, you served in his retainers as a knight, fighting for him.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBorderlands.AddCategoryOption(new TextObject("a healer", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("In the borderlands your proffesion was vital, fighters often were injured, but rarly killed by trollocs, a pleasant side effect of their suppirior armor. Yet without proper care most wounds will kill, mostly due to infections. It was your family's job to care and heal the soldiers of your lord, you even could channel to help.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBorderlands.AddCategoryOption(new TextObject("a daughter of scouts", null), new List<SkillObject>{
				DefaultSkills.Riding,
				DefaultSkills.Scouting
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, Borderlands_warrior_3_OnApply, Borderlands_warrior_3_OnApply, new TextObject("Your father was a scout, he sniffed out trollocs and other shadowspawn with his crew. They were very fast on horses, often warning villages days before a trolloc raid. It seems you have inherited your father's talent.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBorderlands.AddCategoryOption(new TextObject("a son of scouts", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, Borderlands_warrior_3_OnApply, Borderlands_warrior_3_OnApply, new TextObject("Your father was a scout, he sniffed out trollocs and other shadowspawn with his crew. They were very fast on horses, often warning villages days before a trolloc raid. It seems you have inherited your father's talent.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBorderlands.AddCategoryOption(new TextObject("a trolloc hunter", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Borderlands_warrior_2_OnApply, Borderlands_warrior_2_OnApply, new TextObject("Why fight trollocs on your soil if you can fight them on theirs? This was the thinking of you and your band, together you roamed the borderlands, occasionaly going of into the blight to fight trollocs. You were paid by many to do so, by merchants to keep the roads safe, by villages to decrease the chance of raids and by lords, to protect their holdings.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBorderlands.AddCategoryOption(new TextObject("a frontier farmer", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Bow
				},
					DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("Many said your kind is among the toughest lot to ever exist, you tilled the soil almost at the edge of the blight, far from any protection. A difficult job in it self, given the nearness of the Dark One, but then there were the trollocs... everyone in your family had to fight, at times you were outnumbered 10 to one, yet you prevailed and stubbornly continued tilling the soil.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryBorderlands.AddCategoryOption(new TextObject("a survivor of the Malkieri", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Borderlands_warrior_4_OnApply, Borderlands_warrior_4_OnApply, new TextObject("You are one of the few who still hold to some resemblance of tradition after the fall of the Malkier. Although you were a child back then you adopted the ways of your forefathers and now stand as a proud Makkieri, ready to reclaim your land as soon as the opportunity presents itself... or you yourself make it happen.", null), null, 0, 0, 0, 0, 0);

			//Seanchan
			CharacterCreationCategory characterCreationCategorySeanchan = characterCreationMenu.AddMenuCategory(Seanchan_condition);




			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);


			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("an escaped Damane", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, Seanchan_channeler_1_OnApply, Seanchan_channeler_1_OnApply, new TextObject("As all women who can channel in the lands controlled by the Seanchan you were enslaved and became a Damane. Although to the eyes of your Suldam you lost any will quickly, you retained a spark of hope and a far more though Damane with thoughts to escape managed to reignite it, together you fled and are now free to take your revenge.", null), null, 0, 0, 0, 0, 0);

			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("a survived male channeler", null), new List<SkillObject>{
				DefaultSkills.Throwing,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, Seanchan_channeler_1_OnApply, Seanchan_channeler_1_OnApply, new TextObject("Although in the Seanchan controlled lands all male channelers are killed your managed to survive, over time you developed extraordinary abilities to survive on your own, hunted. You also developed a raging hatred towards the Seanchan, lusting to kill them all.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("part of the Seanchan minor blood", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_rich_1_OnApply, default_rich_1_OnApply, new TextObject("Although your family was somehwat insignificant you still got an excellent education, away from the common filth. You went to a university of officers and trained to one day, lead men into battle. Yet your family was exiled in some plot, and thus you land in the westlands, no longer being respected as you were, yet you have amition and plenty of it. Who knows, maybe one day you'll be the emperor of the Seanchan?", null), null, 0, 0, 0, 0, 0);
			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("a relative of a Deathwatch Guard", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Seanchan_warrior_1_OnApply, Seanchan_warrior_1_OnApply, new TextObject("As a relative of an excellent Deathwatch Guard your biggest amition was to follow in his footsteps, and thus you tried to join the ranks of the Guards, even got an armor. Yet you got on the wrong side of a plot, and thus were ordered to be killed, but you managed to escape and now have to find a way of your own.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategorySeanchan.AddCategoryOption(new TextObject("a recruit in the army of the Seanchan", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Polearm
				},
				DefaultCharacterAttributes.Control, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Seanchan_warrior_2_OnApply, Seanchan_warrior_2_OnApply, new TextObject("The ever hungry warmachine of the Seanchan empire always needs new soldiers, you were one of many who decided to fight for the empress, in the hopes of earning glory, but most of all money.", null), null, 0, 0, 0, 0, 0);


			//amadicia

			CharacterCreationCategory characterCreationCategoryAmadicia = characterCreationMenu.AddMenuCategory(Amadicia_condition);




			characterCreationCategoryAmadicia.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAmadicia.AddCategoryOption(new TextObject("a relative of a wisdom", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("As a relative of your villages wisdom you had a good living, learning a lot from her. You even inherited the ability to channel from her, incredible luck.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAmadicia.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAmadicia.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAmadicia.AddCategoryOption(new TextObject("a White Cloak", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Amadicia_warrior_1_OnApply, Amadicia_warrior_1_OnApply, new TextObject("You were a soldier among the ranks of the Children of the Light, fighting for, what you thought, was the right cause.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAmadicia.AddCategoryOption(new TextObject("born into a family of shopkeepers", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_middle_1_OnApply, default_middle_1_OnApply, new TextObject("You were part of the town's middle class, helping out in your family's shop all your life, offering sevices of different kinds to your clients and making a decent living of it.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAmadicia.AddCategoryOption(new TextObject("a loyalsit of the king", null), new List<SkillObject>{
				DefaultSkills.Polearm,
				DefaultSkills.Roguery
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Amadicia_warrior_2_OnApply, Amadicia_warrior_2_OnApply, new TextObject("You are aware that the Children of the Light controll most of Amadicia, and your king is just a puppet without power. This is why you decided to try and throw the fanatics of the Light out of your country and serve your king, like it should be. The only question is: Will you suceed in your quest?", null), null, 0, 0, 0, 0, 0);


			//Andor

			CharacterCreationCategory characterCreationCategoryAndor = characterCreationMenu.AddMenuCategory(Andor_condition);




			characterCreationCategoryAndor.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAndor.AddCategoryOption(new TextObject("a relative of a wisdom", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("As a relative of your villages wisdom you had a good living, learning a lot from her. You even inherited the ability to channel from her, incredible luck.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAndor.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAndor.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAndor.AddCategoryOption(new TextObject("a noble retainer", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Andor_warrior_3_OnApply, Andor_warrior_3_OnApply, new TextObject("You were part of some minor country nobility, your family sent you to a more important lord, to serve in as one of his retainers. You were a knight on foot, part of your lords elite force.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAndor.AddCategoryOption(new TextObject("a recruit in the Queens Guard", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Andor_warrior_1_OnApply, Andor_warrior_1_OnApply, new TextObject("The Queens Guard is the best fighting force in your realm, you were recruited early in your life for managing to organize a village to resist a raid. Yet you soon noticed that your place wasn't among the guards, you simply couldn't understand why you should give your life for some monarch and you fled. You joined a company of mercenaries, but soon left even them, hoping to manage on your own.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryAndor.AddCategoryOption(new TextObject("an common levy", null), new List<SkillObject>{
				DefaultSkills.Polearm,
				DefaultSkills.Crafting
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Andor_warrior_2_OnApply, Andor_warrior_2_OnApply, new TextObject("Your simple life in your home village was ended abruptly, when a weapontake was called. You were judged old and stong enough and not too vital for the economy to be pressed into service as a common infrantry man. You are forced to fight and die for causes you neither understand nor care about, yet the prospect of foraging gives you hope to maybe come out of this richer.", null), null, 0, 0, 0, 0, 0);


			//Cairhien

			CharacterCreationCategory characterCreationCategoryCairhien = characterCreationMenu.AddMenuCategory(Cairhien_condition);




			characterCreationCategoryCairhien.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryCairhien.AddCategoryOption(new TextObject("a relative of a wisdom", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("As a relative of your villages wisdom you had a good living, learning a lot from her. You even inherited the ability to channel from her, incredible luck.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryCairhien.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryCairhien.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryCairhien.AddCategoryOption(new TextObject("a noble officer", null), new List<SkillObject>{
				DefaultSkills.Tactics,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Cairhien_warrior_1_OnApply, Cairhien_warrior_1_OnApply, new TextObject("Your father always wanted you to join the army, so you did. Yet you were young and weren't use for anything except drinking and having fun. Surprisingly a year or two changed your, you became a dutyful and intellegent officer, on a good way to becoming a senior officer.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryCairhien.AddCategoryOption(new TextObject("born into a family of shopkeepers", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_middle_1_OnApply, default_middle_1_OnApply, new TextObject("You were part of the town's middle class, helping out in your family's shop all your life, offering sevices of different kinds to your clients and making a decent living of it.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryCairhien.AddCategoryOption(new TextObject("a squire", null), new List<SkillObject>{
				DefaultSkills.OneHanded,
				DefaultSkills.Scouting
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Cairhien_warrior_2_OnApply, Cairhien_warrior_2_OnApply, new TextObject("You are a noble squire on a good way to becoming a knight yourself. Yet your amition doesn't end there, you want to become a renowned mercenary captain.", null), null, 0, 0, 0, 0, 0);

			//illian
			CharacterCreationCategory characterCreationCategoryIllian = characterCreationMenu.AddMenuCategory(Illian_condition);




			characterCreationCategoryIllian.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryIllian.AddCategoryOption(new TextObject("a relative of a wisdom", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("As a relative of your villages wisdom you had a good living, learning a lot from her. You even inherited the ability to channel from her, incredible luck.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryIllian.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryIllian.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryIllian.AddCategoryOption(new TextObject("an Illianer companion", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Illian_warrior_1_OnApply, Illian_warrior_1_OnApply, new TextObject("You joined the Illianer companions long ago, when you were just 17, hoping to make an succesfull military career. Yet numerous fights and skirmishers dishartened you, war was not the glorious game of swords you thought it was, it was a brutal and usless carnage where the rich got richer and poor went into their graves. You left the companions and are now left to survive on your own.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryIllian.AddCategoryOption(new TextObject("born into a family of shopkeepers", null), new List<SkillObject>{
				DefaultSkills.Trade,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_middle_1_OnApply, default_middle_1_OnApply, new TextObject("You were part of the town's middle class, helping out in your family's shop all your life, offering sevices of different kinds to your clients and making a decent living of it.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryIllian.AddCategoryOption(new TextObject("a hunter for the horn", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Throwing
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Illian_warrior_2_OnApply, Illian_warrior_2_OnApply, new TextObject("You took the hunters oath some years ago and went of into the world, searching for the legendary horn of valeer. Yet so far you haven't found any hints, except a curious message form the creator: 'Quests will be implemented into the mod at a later date, among the first will be a quest for the horn of valeer', so you suppose you'll find your own way through the world for now.", null), null, 0, 0, 0, 0, 0);

			//default

			CharacterCreationCategory characterCreationCategoryTear = characterCreationMenu.AddMenuCategory(Tear_condition);




			characterCreationCategoryTear.AddCategoryOption(new TextObject("part of a farmer's household", null), new List<SkillObject>{
				DefaultSkills.Athletics,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Endurance, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_poor_1_OnApply, default_poor_1_OnApply, new TextObject("You were raised at a farm in a rural area. A poor living and since you had elder brothers, they inhereted most of the farm, you were only left with your brains and clothes after you left the household.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryTear.AddCategoryOption(new TextObject("a relative of a wisdom", null), new List<SkillObject>{
				DefaultSkills.Medicine,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, default_channeler_1_OnApply, default_channeler_1_OnApply, new TextObject("As a relative of your villages wisdom you had a good living, learning a lot from her. You even inherited the ability to channel from her, incredible luck.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryTear.AddCategoryOption(new TextObject("a daughter of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Female, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryTear.AddCategoryOption(new TextObject("a son of a labourer", null), new List<SkillObject>{
				DefaultSkills.Crafting,
				DefaultSkills.OneHanded
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, Male, default_poor_2_OnApply, default_poor_2_OnApply, new TextObject("Your father was a labourer, heading from town to town in search of jobs that suited his craft. He was working at construction sites, harbours and reparing for his entire life, sharing a great deal of that experience with you.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryTear.AddCategoryOption(new TextObject("a defender of the Stone", null), new List<SkillObject>{
				DefaultSkills.TwoHanded,
				DefaultSkills.Leadership
				},
				DefaultCharacterAttributes.Intelligence, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Tear_warrior_1_OnApply, Tear_warrior_1_OnApply, new TextObject("A place in the legendary Defenders of the stone always was your dream, and just a few month ago it became reality. You joied a company of the defenders and to your missfortune you were immediatly sent of into action. It was a minor rebelion, yet your commander was incompetent and thus your were surrounded. Out of a hundred or so men barley a dozen survived, you among them.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryTear.AddCategoryOption(new TextObject("a levy dodging service", null), new List<SkillObject>{
				DefaultSkills.Roguery,
				DefaultSkills.Riding
				},
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Tear_warrior_2_OnApply, Tear_warrior_2_OnApply, new TextObject("By hairs breath you managed to avoid being pressed into service and instead fled into the countryside. There you seek now to try and find a living, who nows, maybe you'll end up fighting for or against Tear anyways.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategoryTear.AddCategoryOption(new TextObject("a fresh noble knight", null), new List<SkillObject>{
				DefaultSkills.Polearm,
				DefaultSkills.Charm
				},
				DefaultCharacterAttributes.Cunning, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Tear_warrior_3_OnApply, Tear_warrior_3_OnApply, new TextObject("Just a few weeks ago you were still drinking and corousing around Tear with your friends, but you were called upon to serve in the cavalry as a noble knight by the high lords. After realising just what military service was you deserted and are now in hiding.", null), null, 0, 0, 0, 0, 0);

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
				DefaultCharacterAttributes.Social, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, Profession_trader_consequence, null, new TextObject("Travelling from town to town, buying and selling, this is how you pased your days, a job which gave you a lot of profit, yet involved the risks of being robed.", null), null, 0, 0, 0, 0, 0);
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
			
			characterCreationCategory.AddCategoryOption(new TextObject("the Great Blight", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Blight_Spawn, new TextObject("You start near Shayol Ghul, where the heart of the shadow is.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("the Waste", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Waste_Spawn, new TextObject("You start near Ruidian, an ancient ruin where Wiseones and Chiefs of the Aiel are made.", null), null, 0, 0, 0, 0, 0);
			characterCreationCategory.AddCategoryOption(new TextObject("Andor", null), new List<SkillObject>{
				DefaultSkills.Bow,
				DefaultSkills.Athletics
				},
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, Andor_Spawn, new TextObject("You start near Camelyn, Andor's capital.", null), null, 0, 0, 0, 0, 0);
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
				DefaultCharacterAttributes.Vigor, this.FocusToAdd, this.SkillLevelToAdd, this.AttributeLevelToAdd, null, null, WT_Spawn, new TextObject("You start near the White Tower, the school and place of residence of the Aes Sedai.", null), null, 0, 0, 0, 0, 0);


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

			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "channeler_CC_1_F");
				clothing = "channeler_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "channeler_CC_1_M");
				clothing = "channeler_CC_1_M";
			}
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

		protected void default_poor_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "default_poor_CC_1_F");
				clothing = "default_poor_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "default_poor_CC_1_M");
				clothing = "default_poor_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void default_poor_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "default_poor_CC_2_F");
				clothing = "default_poor_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "default_poor_CC_2_M");
				clothing = "default_poor_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void default_middle_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "default_middle_CC_1_F");
				clothing = "default_middle_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "default_middle_CC_1_M");
				clothing = "default_middle_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void default_rich_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "default_rich_CC_1_F");
				clothing = "default_rich_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "default_rich_CC_1_M");
				clothing = "default_rich_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void default_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "default_warrior_CC_1_F");
				clothing = "default_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "default_warrior_CC_1_M");
				clothing = "default_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void default_channeler_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "default_channeler_CC_1_F");
				clothing = "default_channeler_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "default_channeler_CC_1_M");
				clothing = "default_channeler_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}

		protected void WT_channeler_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "wt_channeler_CC_1_F");
				clothing = "wt_channeler_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "wt_channeler_CC_1_M");
				clothing = "wt_channeler_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}
		protected void WT_channeler_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "wt_channeler_CC_2_F");
				clothing = "wt_channeler_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "wt_channeler_CC_2_M");
				clothing = "wt_channeler_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}
		protected void BT_channeler_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "bt_channeler_CC_1_F");
				clothing = "bt_channeler_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "bt_channeler_CC_1_M");
				clothing = "bt_channeler_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}
		protected void BT_channeler_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "bt_channeler_CC_2_F");
				clothing = "bt_channeler_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "bt_channeler_CC_2_M");
				clothing = "bt_channeler_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}

		protected void Dragon_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "dragon_warrior_CC_1_F");
				clothing = "dragon_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "dragon_warrior_CC_1_M");
				clothing = "dragon_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void Dragon_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "dragon_warrior_CC_2_F");
				clothing = "dragon_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "dragon_warrior_CC_2_M");
				clothing = "dragon_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Dragon_warrior_3_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "two_river_warrior_CC_1_F");
				clothing = "two_river_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "two_river_warrior_CC_1_M");
				clothing = "two_river_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void Dragon_warrior_4_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "dragon_warrior_CC_3_F");
				clothing = "dragon_warrior_CC_3_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "dragon_warrior_CC_3_M");
				clothing = "dragon_warrior_CC_3_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Borderlands_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, base.GetSelectedCulture() + "_warrior_CC_1_F");
				clothing = base.GetSelectedCulture() + "_warrior_CC_1_F";
				
			}
			else
			{
				ChangePlayerOutfit(characterCreation, base.GetSelectedCulture() + "_warrior_CC_1_M");
				clothing = base.GetSelectedCulture() + "_warrior_CC_1_M";
			}
			
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Borderlands_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "borderlands_warrior_CC_2_F");
				clothing = "borderlands_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "borderlands_warrior_CC_2_M");
				clothing = "borderlands_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Borderlands_warrior_3_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "borderlands_warrior_CC_3_F");
				clothing = "borderlands_warrior_CC_3_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "borderlands_warrior_CC_3_M");
				clothing = "borderlands_warrior_CC_3_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}


		protected void Borderlands_warrior_4_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "borderlands_warrior_CC_4_F");
				clothing = "borderlands_warrior_CC_4_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "borderlands_warrior_CC_4_M");
				clothing = "borderlands_warrior_CC_4_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Seanchan_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "seanchan_warrior_CC_1_F");
				clothing = "seanchan_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "seanchan_warrior_CC_1_M");
				clothing = "seanchan_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Seanchan_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "seanchan_warrior_CC_2_F");
				clothing = "seanchan_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "seanchan_warrior_CC_2_M");
				clothing = "seanchan_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Seanchan_channeler_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "seanchan_channeler_CC_1_F");
				clothing = "seanchan_channeler_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "seanchan_channeler_CC_1_M");
				clothing = "seanchan_channeler_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = true;
		}

		protected void Amadicia_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "amadicia_warrior_CC_1_F");
				clothing = "amadicia_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "amadicia_warrior_CC_1_M");
				clothing = "amadicia_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Amadicia_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "amadicia_warrior_CC_2_F");
				clothing = "amadicia_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "amadicia_warrior_CC_2_M");
				clothing = "amadicia_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Andor_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "andor_warrior_CC_1_F");
				clothing = "andor_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "andor_warrior_CC_1_M");
				clothing = "andor_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void Andor_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "andor_warrior_CC_2_F");
				clothing = "andor_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "andor_warrior_CC_2_M");
				clothing = "andor_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void Andor_warrior_3_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "andor_warrior_CC_3_F");
				clothing = "andor_warrior_CC_3_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "andor_warrior_CC_3_M");
				clothing = "andor_warrior_CC_3_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Cairhien_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "cairhien_warrior_CC_1_F");
				clothing = "cairhien_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "cairhien_warrior_CC_1_M");
				clothing = "cairhien_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Cairhien_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "cairhien_warrior_CC_2_F");
				clothing = "cairhien_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "cairhien_warrior_CC_2_M");
				clothing = "cairhien_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}


		protected void Illian_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "illian_warrior_CC_1_F");
				clothing = "illian_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "illian_warrior_CC_1_M");
				clothing = "illian_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Illian_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "illian_warrior_CC_2_F");
				clothing = "illian_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "illian_warrior_CC_2_M");
				clothing = "illian_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected void Tear_warrior_1_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "tear_warrior_CC_1_F");
				clothing = "tear_warrior_CC_1_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "tear_warrior_CC_1_M");
				clothing = "tear_warrior_CC_1_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void Tear_warrior_2_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "tear_warrior_CC_2_F");
				clothing = "tear_warrior_CC_2_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "tear_warrior_CC_2_M");
				clothing = "tear_warrior_CC_2_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}
		protected void Tear_warrior_3_OnApply(CharacterCreation characterCreation)
		{
			if (Hero.MainHero.IsFemale)
			{
				ChangePlayerOutfit(characterCreation, "tear_warrior_CC_3_F");
				clothing = "tear_warrior_CC_3_F";
			}
			else
			{
				ChangePlayerOutfit(characterCreation, "tear_warrior_CC_3_M");
				clothing = "tear_warrior_CC_3_M";
			}
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);
			channeler = false;
		}

		protected bool Shadowspawn_condition()
        {
			return base.GetSelectedCulture().StringId == "sturgia";
		}
		protected bool WT_condition()
		{
			return base.GetSelectedCulture().StringId == "WhiteTower";
		}

		protected bool Aiel_condition()
		{
			return base.GetSelectedCulture().StringId == "khuzait";

		}
		protected bool BT_condition()
		{
			return base.GetSelectedCulture().StringId == "BlackTower";
		}
		protected bool Dragonsworn_condition()
		{
			return base.GetSelectedCulture().StringId == "Dragonsworn";
		}

		protected bool Seanchan_condition()
		{
			return base.GetSelectedCulture().StringId == "Seanchan";
		}

		protected bool Borderlands_condition()
		{
			return base.GetSelectedCulture().StringId == "Arafel" || base.GetSelectedCulture().StringId == "Shienar" || base.GetSelectedCulture().StringId == "Kandor" || base.GetSelectedCulture().StringId == "battania";
		}


		protected bool Amadicia_condition()
		{
			return base.GetSelectedCulture().StringId == "Amamdicia";
		}

		protected bool Andor_condition()
		{
			return base.GetSelectedCulture().StringId == "empire";
		}

		protected bool Cairhien_condition()
		{
			return base.GetSelectedCulture().StringId == "Cairhien";
		}

		protected bool Illian_condition()
		{
			return base.GetSelectedCulture().StringId == "aserai";
		}

		protected bool Tear_condition()
		{
			return base.GetSelectedCulture().StringId == "Tear";
		}

		protected bool default_condition()
		{
			if(!Shadowspawn_condition() && !Aiel_condition() && !WT_condition() && !BT_condition() && !Dragonsworn_condition() && !Borderlands_condition() && !Seanchan_condition() && !Amadicia_condition() && !Andor_condition() && !Cairhien_condition() && !Illian_condition() && !Tear_condition())
            {
				return true;
            }
			return false;
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
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_manners" });
		}
		protected void Profession_trader_consequence(CharacterCreation characterCreation)
		{
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_clever" });
		}
		protected void Profession_blacksmith_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_1_t1", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_tough" });
		}
		protected void Profession_hunter_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "hunting_bow", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
		}
		protected void Profession_fisher_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "northern_javelin_1_t2", true, "");
			characterCreation.ChangeCharsAnimation(new List<string>{ "act_childhood_ready" });
		}

		protected void Profession_sheep_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "carry_bostaff_rogue1", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_athlete" });
		}
		protected void Profession_trolloc_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "western_javelin_3_t4", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_athlete" });
		}
		protected void Profession_fortify_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "peasant_hammer_2_t1", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_gracious" });
		}

		protected void Profession_raider_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "iron_spatha_sword_t2", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_ready" });
		}
		protected void Profession_pirate_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "simple_sabre_sword_t2", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_vibrant" });
		}
		protected void Profession_horse_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "horse_whip", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_tough" });
		}
		protected void Profession_healer_consequence(CharacterCreation characterCreation)
		{
			RefreshPropsAndClothing(characterCreation, false, "the_scalpel_sword_t3", true, "");
			characterCreation.ChangeCharsAnimation(new List<string> { "act_childhood_clever" });
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

		protected bool Female()
        {
			return Hero.MainHero.IsFemale;
        }
		protected bool Male()
		{
			return !Hero.MainHero.IsFemale;
		}
		protected void Talent_Fireball_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerblossomsoffiresingle";
			amunition = "none";
			ApplyEquipments(characterCreation);	
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Windstorms_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerwindstorm1FireBall";
			amunition = "onepowerwindstorm1Fireballammoarrow";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Lightning_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerlightningstormsmall";
			amunition = "none";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Earth_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "onepowerrollingearthandfire";
			amunition = "none";
			ApplyEquipments(characterCreation);

			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Shield_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "OnePowerAirShield";
			amunition = "vlandia_sword_1_t2";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Spear_Consequence(CharacterCreation characterCreation)
		{
			if(base.GetSelectedCulture().StringId == "khuzait")
            {
				weapon = "AielSpeargrade1";
				amunition = "none";
			}
            else
            {
				weapon = "western_spear_1_t2";
				amunition = "none";
			}
			RefreshPlayerAppearance(characterCreation);
			
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_Bow_Consequence(CharacterCreation characterCreation)
		{
			if(base.GetSelectedCulture().StringId == "khuzait")
            {
				amunition = "default_arrows";
				weapon = "Aielbowlevel1";
			}
			
				
			else if (base.GetSelectedCulture().StringId == "sturgia")
			{
				weapon = "TrollocBow";
				amunition = "default_arrows";

			}
			else
            {
				amunition = "default_arrows";
				weapon = "steppe_bow";
			}
			RefreshPlayerAppearance(characterCreation);
			
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_CrossBow_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			amunition = "bolt_a";
			weapon = "crossbow_a";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_throwing_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			amunition = "western_javelin_1_t2";
			weapon = "western_javelin_1_t2";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_sword_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			if(base.GetSelectedCulture().StringId == "sturgia")
            {
				weapon = "Trollocweapon1";
				amunition = "none";

			}
			weapon = "sturgia_sword_1_t2";
			amunition = "none";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_horse_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "battania_mace_2_t2";
			amunition = "western_kite_shield";
			ChangePlayerMount(characterCreation, Hero.MainHero);
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_buckler_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "Aiel_shield";
			amunition = "AielSpeargrade1";
			ApplyEquipments(characterCreation);
			RefreshPlayerAppearance(characterCreation);

		}
		protected void Talent_longbow_Consequence(CharacterCreation characterCreation)
		{
			RefreshPlayerAppearance(characterCreation);
			weapon = "Aielbowlevel1";
			amunition = "default_arrows";
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
				ItemObject @object2 = Game.Current.ObjectManager.GetObject<ItemObject>(amunition);
				base.PlayerStartEquipment.AddEquipmentToSlotWithoutAgent(EquipmentIndex.Weapon2, new EquipmentElement(@object2, null, null, false));
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
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this._startingAge, "act_childhood_schooled"));
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
			characterCreation.ChangeFaceGenChars(WoTCharacterCreation.ChangePlayerFaceWithAge((float)this._startingAge, "act_childhood_schooled"));
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

	

		

	
		protected WoTCharacterCreation.SandboxAgeOptions _startingAge = WoTCharacterCreation.SandboxAgeOptions.MiddleAged;

		
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
