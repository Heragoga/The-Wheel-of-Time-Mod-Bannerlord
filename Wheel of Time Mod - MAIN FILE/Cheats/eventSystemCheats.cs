using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using WoT_Main.CampaignEventSystem;
using System.Windows;
using WoT_Main.Behaviours;
using WoT_Main.Support;

namespace WoT_Main.Cheats
{
	//for testing, doesn't add the war to ConstantWars
    class eventSystemCheats
    {

		[TaleWorlds.Library.CommandLineFunctionality.CommandLineArgumentFunction("declareSuccesionWar", "WoT_Main")]
		public static string FullCompanion(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}

			if (strings[0].ToLower() == "help")
			{
				string text2 = "";
				text2 += "\n";
				text2 += "Format is \"WoT_Main.declareSuccesionWar [faction(string)] [amoutnOfRebels(int)]\".";
				return text2;
			}
			if (strings.Count != 2)
            {
				return "WoT_Main.declareSuccesionWar [faction(string)] [amoutnOfRebels(int)]";
            }

			string faction = "";
			faction += strings[0];
			int amountofRebels = Convert.ToInt32(strings[1]);

			innerFactionWarEvents.succesionWar(campaignSupport.getFaction(faction), amountofRebels);
			
			return "Success";
		}
	}
}
