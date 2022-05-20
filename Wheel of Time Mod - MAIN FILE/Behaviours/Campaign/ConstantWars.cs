using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;
using WoT_Main.CampaignEventSystem;
using WoT_Main.Support;
using TaleWorlds.SaveSystem;
using TaleWorlds.MountAndBlade;
using System;

namespace WoT_Main.Behaviours
{
    public partial class ConstantWars : CampaignBehaviorBase
    {

        public List<War> wars;

        public bool warsInitialized = false;

        public List<string> warsSave;


        public ConstantWars()
        {
            if(warsInitialized == false)
            {
                warsInitialized = true;
                
                warsSave = new List<string>();
            }
            wars = new List<War>();

        }

        public override void RegisterEvents()
        {

            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, war);
            CampaignEvents.OnBeforeSaveEvent.AddNonSerializedListener(this, save);
            CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, load);
            
        }

        public override void SyncData(IDataStore dataStore)
        {
            
            dataStore.SyncData("warsInitialized", ref warsInitialized);
            dataStore.SyncData("warsSave", ref warsSave);

        }
        private void war()
        {
            if (wars != null)
            {
                foreach (War war in wars)
                {
                    if(war != null && war.isValid())
                    {
                        war.declareWar();
                        
                    }
                }
            }

        }
        private void save()
        {
            foreach (War war in wars)
            {
                
                if(war.againstAllKingdom != null)
                {
                    warsSave.Add("againstAllHeader");
                    warsSave.Add(war.againstAllKingdom.Name.ToString());
                }
                else if (war.kingdoms != null)
                {
                    warsSave.Add("multipleKingdomsHeader");
                    warsSave.Add( ""+war.kingdoms.Count);
                    foreach (Kingdom kingdom in war.kingdoms)
                    {
                        if(!kingdom.IsEliminated && kingdom != null)
                        {
                            warsSave.Add(kingdom.Name.ToString());
                        }
                        
                    }
                }
            }

            
        }
        private void load()
        {
            string[] strings = warsSave.ToArray();
            for (int i = 0; i < strings.Length; i++)
            {

                if (strings[i] == "againstAllHeader")
                {
                    wars.Add(new War(campaignSupport.getFaction(strings[i + 1])));
                }
                else if (strings[i] == "multipleKingdomsHeader")
                {
                    List<Kingdom> kingdoms = new List<Kingdom>();

                    int amountofKingdoms = Convert.ToInt32(strings[i + 1]);
                    for(int j = 1; j <= amountofKingdoms; j++)
                    {
                        if(strings[i + j + 1] == "againstAllHeader" || strings[i + j + 1] == "multipleKingdomsHeader")
                        {
                            continue;
                        }
                        else
                        {
                            kingdoms.Add(campaignSupport.getFaction(strings[i + j + 1]));
                        }
                    }
                    if(kingdoms.Count > 0)
                    {
                        wars.Add(new War(kingdoms));
                    }
                   
                }
            }
        }
       
    }
}

