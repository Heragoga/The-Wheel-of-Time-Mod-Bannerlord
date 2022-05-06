using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using WoT_Main.Support;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.Actions;
using System.Reflection;
using TaleWorlds.Library;
using WoT_Main.Behaviours;
using WoT_Main.CampaignEventSystem;

namespace WoT_Main.CampaignEventSystem
{
    class innerFactionWarEvents
    {
        //intensity means how many clans will be on the adversarys faction side
        public static War succesionWar(Kingdom factionName, int amountOfRebels)
        { 
            Kingdom faction = null;
            faction = factionName;
            
            int totalAmountOfLordsInFormerFaction = faction.Lords.Count();

            Clan[] leaderClansForRebels = new Clan[amountOfRebels];

            if (totalAmountOfLordsInFormerFaction >= 2 && amountOfRebels >= 2)
            {

                
                for(int i = 0; i < leaderClansForRebels.Length; i++)
                {
                    if(i == 0)
                    {
                        leaderClansForRebels[i] = faction.Leader.Clan;
                        continue;
                    }
                    Clan[] formerFactionClans = faction.Clans.ToArray();

                    Clan mostPowerfullClan = null;
                    foreach(Clan clan in formerFactionClans)
                    {
                        if (clan != faction.Leader.Clan && !leaderClansForRebels.Contains(clan))
                        {
                            if (mostPowerfullClan != null)
                            {
                                if (clan.TotalStrength > mostPowerfullClan.TotalStrength)
                                {
                                    mostPowerfullClan = clan;
                                }
                            }
                            else
                            {
                                mostPowerfullClan = clan;
                            }
                        }
                    }
                    if(mostPowerfullClan != null)
                    {
                        leaderClansForRebels[i] = mostPowerfullClan;
                    }
                }

                Kingdom[] subFactions = new Kingdom[amountOfRebels];
                KingdomManager kingdomManager = new KingdomManager();
                
                for(int i = 0; i < subFactions.Length; i++)
                {
                    if(subFactions != null)
                    {
                        kingdomManager.CreateKingdom(new TextObject(leaderClansForRebels[i].Name.ToString() + "'s " + faction.Name), new TextObject(leaderClansForRebels[i].Name.ToString() + "'s " + faction.Name), leaderClansForRebels[i].Culture, leaderClansForRebels[i]);
                        
                        subFactions[i] = campaignSupport.getFaction(leaderClansForRebels[i].Name.ToString() + "'s " + faction.Name);
                        Banner newBanner = campaignSupport.generateBanner();
                        campaignSupport.changeFactionBanner(subFactions[i], newBanner);
                        
                    } 
                }

                Clan[] remainingClans = faction.Clans.ToArray();
                
                foreach(Clan clan in remainingClans)
                {
                    
                    ChangeKingdomAction.ApplyByJoinToKingdom(clan, subFactions.GetRandomElement(), false);
                    campaignSupport.changeClanBanner(clan, clan.Kingdom.Banner);

                }

                if(Hero.MainHero.Clan.Kingdom == faction)
                {
                    Hero.MainHero.Clan.Kingdom = subFactions.GetRandomElement();
                }
                
                return new War(subFactions.ToList());
            }
            else
            {
                return null;
            }
        }
        public static War succesionWar(string factionName, int amountOfRebels)
        {
            Kingdom faction = null;
            faction = campaignSupport.getFaction(factionName);

            int totalAmountOfLordsInFormerFaction = faction.Lords.Count();

            Clan[] leaderClansForRebels = new Clan[amountOfRebels];

            if (totalAmountOfLordsInFormerFaction >= 2 && amountOfRebels >= 2)
            {


                for (int i = 0; i < leaderClansForRebels.Length; i++)
                {
                    if (i == 0)
                    {
                        leaderClansForRebels[i] = faction.Leader.Clan;
                        continue;
                    }
                    Clan[] formerFactionClans = faction.Clans.ToArray();

                    Clan mostPowerfullClan = null;
                    foreach (Clan clan in formerFactionClans)
                    {
                        if (clan != faction.Leader.Clan && !leaderClansForRebels.Contains(clan))
                        {
                            if (mostPowerfullClan != null)
                            {
                                if (clan.TotalStrength > mostPowerfullClan.TotalStrength)
                                {
                                    mostPowerfullClan = clan;
                                }
                            }
                            else
                            {
                                mostPowerfullClan = clan;
                            }
                        }
                    }
                    if (mostPowerfullClan != null)
                    {
                        leaderClansForRebels[i] = mostPowerfullClan;
                    }
                }

                Kingdom[] subFactions = new Kingdom[amountOfRebels];
                KingdomManager kingdomManager = new KingdomManager();

                for (int i = 0; i < subFactions.Length; i++)
                {
                    if (subFactions != null)
                    {
                        kingdomManager.CreateKingdom(new TextObject(leaderClansForRebels[i].Name.ToString() + "'s " + faction.Name), new TextObject(leaderClansForRebels[i].Name.ToString() + "'s " + faction.Name), leaderClansForRebels[i].Culture, leaderClansForRebels[i]);

                        subFactions[i] = campaignSupport.getFaction(leaderClansForRebels[i].Name.ToString() + "'s " + faction.Name);
                        Banner newBanner = campaignSupport.generateBanner();
                        campaignSupport.changeFactionBanner(subFactions[i], newBanner);

                    }
                }

                Clan[] remainingClans = faction.Clans.ToArray();

                foreach (Clan clan in remainingClans)
                {

                    ChangeKingdomAction.ApplyByJoinToKingdom(clan, subFactions.GetRandomElement(), false);
                    campaignSupport.changeClanBanner(clan, clan.Kingdom.Banner);

                }

                if (Hero.MainHero.Clan.Kingdom == faction)
                {
                    Hero.MainHero.Clan.Kingdom = subFactions.GetRandomElement();
                }

                return new War(subFactions.ToList());
            }
            else
            {
                return null;
            }
        }


    }
}
