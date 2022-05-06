using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoT_Main.Behaviours;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using WoT_Main.Support;

namespace WoT_Main.CampaignEventSystem
{
    
    public class War
    {
        public List<Kingdom> kingdoms;
        public Kingdom againstAllKingdom;
        bool againstAll = false;
        public War(List<Kingdom> kingdoms)
        {
            this.kingdoms = kingdoms;
        }

        public War(Kingdom kingdom)
        {
            againstAllKingdom = kingdom;
            againstAll = true;
        }

        public void declareWar()
        {
            if (!againstAll)
            {
                foreach (Kingdom kingdom in kingdoms)
                {
                    foreach (Kingdom kingdom1 in kingdoms)
                    {
                        if (kingdom != kingdom1 && !FactionManager.IsAtWarAgainstFaction(kingdom, kingdom1))
                        {
                            FactionManager.DeclareWar(kingdom, kingdom1, true);
                        }
                    }
                }
            }
            else
            {
                Kingdom[] allKingdoms = Campaign.Current.Kingdoms.ToArray();

                
                foreach (Kingdom kingdom in allKingdoms)
                {
                    
                    
                    FactionManager.DeclareWar(kingdom, againstAllKingdom);
                    
                   
                    
                }
            }
        }

        public bool isValid()
        {
            if(kingdoms != null || againstAllKingdom != null)
            {
                return true;
            }
            return false;
        }

        public string generateMessage()
        {
            string str = "";

            if(againstAllKingdom != null)
            {
                str += againstAllKingdom.Name.ToString();
            }
            else if (kingdoms != null)
            {
                foreach(Kingdom kingdom in kingdoms)
                {
                    str += " " + kingdom.Name.ToString();
                }
            }
            else
            {
                str += "war obsolete";
            }

            return str;
        }
    }
}
