using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using System.Reflection;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Core;
using TaleWorlds.CampaignSystem.Settlements;

namespace WoT_Main.Support
{
    public class campaignSupport
    {
        //get a Kingodm 
        public static Kingdom getFaction(String name)
        {

            Kingdom[] kingdoms = Kingdom.All.ToArray();

            //Just a cycle TODO better comparisson
            foreach (Kingdom kingdom in kingdoms)
            {
                if (kingdom.Name.ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return kingdom;
                }
            }

            return null;


        }
        

        public static Settlement getSettlement(string name)
        {
            Settlement settlement = Settlement.Find(name);
            return settlement;
            
        }

        public static void displayMessageInChat(String s)
        {
            InformationManager.DisplayMessage(new InformationMessage(s));
        }
        public static void displayMessageInChat(String s, Color consoleColor)
        {
            InformationManager.DisplayMessage(new InformationMessage(s, consoleColor));
        }

        //just some relic, might need it in the future
        public static bool isClanPartOfKingdomArray(Kingdom[] kingdoms, Clan clan)
        {
            foreach (Kingdom kingdom in kingdoms)
            {
                Clan[] clans = kingdom.Clans.ToArray();
                foreach (Clan clan1 in clans)
                {
                    if (clan1.Equals(clan))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //really a shame that taleworlds didn't implement that properly
        public static void changeFactionBanner(object obj, Banner banner)
        {
            if(obj != null && banner != null && (obj.GetType() == typeof(Kingdom) || obj.GetType() == typeof(Clan)))
            {
                foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                {

                    if (fieldInfo.GetValue(obj).GetType() == typeof(Banner))
                    {
                        fieldInfo.SetValue(obj, banner);

                    }
                }
            }
            
        }

        public static void changeClanBanner(Clan clan, Banner banner)
        {

            FieldInfo fieldInfo = typeof(Clan).GetField("_banner", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(clan, banner);
        }

        public static Banner generateBanner()
        {
            Banner banner = null;

            banner = new Banner(Banner.CreateRandomBanner());

            return banner;
        }
    }
}


