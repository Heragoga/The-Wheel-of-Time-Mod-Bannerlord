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
    //class which contaisn a war, can either be a list of kingdoms which all fight against each other or a single kingdom which faces the entire world
    //DO NOT EDIT else loading and saving wont work
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

        //to redecalre the wars
        //IMPORTANT check wheather war is already declared BEFORE using this method
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

        //generates a message for debuggin purposes
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
