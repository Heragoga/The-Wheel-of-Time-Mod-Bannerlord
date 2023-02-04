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
					JObject config = JObject.Parse(File.ReadAllText(Path.GetFullPath(BasePath.Name + "Modules/Wheel of Time Mod - MAIN FILE/config.json")));
					
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
}
