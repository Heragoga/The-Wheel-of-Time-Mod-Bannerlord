using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

namespace WoT_Main.Cheats
{
    class DebugCheats
    {
		[TaleWorlds.Library.CommandLineFunctionality.CommandLineArgumentFunction("herag", "tp_main_party")]
		public static string FullCompanion(List<string> strings)
		{
			if (!CampaignCheats.CheckCheatUsage(ref CampaignCheats.ErrorType))
			{
				return CampaignCheats.ErrorType;
			}


			if(strings.Count != 2)
            {
				return "Format is [int amount] [string direction (left, right, up, down)]";
            }

			int amount = Convert.ToInt32(strings[0]);

			float xChange = 0;
			float yChange = 0;

            switch (strings[1])
            {
				case "left":  xChange = -amount; break;
				case "right": xChange = amount; break;
				case "up": yChange = amount; break;
				case "down": yChange = -amount; break;
				default: xChange = amount; break;
			}

			MobileParty.MainParty.Position2D = new TaleWorlds.Library.Vec2(MobileParty.MainParty.Position2D.X + xChange, MobileParty.MainParty.Position2D.Y + yChange);



			return "Success";
		}
	}
}
