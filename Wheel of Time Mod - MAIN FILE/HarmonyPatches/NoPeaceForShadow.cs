using System;
using System.Collections.Generic;
using System.Linq;

using HarmonyLib;
using MountAndBlade.CampaignBehaviors;
using SandBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation.OptionsStage;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace Wheel_of_Time_Mod___MAIN_FILE.HarmonyPatches
{
    //[HarmonyPatch]
    //class NoPeaceForShadow
    //{
    //    [HarmonyPrefix]
    //    [HarmonyPatch(typeof(KingdomDecisionProposalBehavior), "ConsiderPeace")]
    //    public static bool ConsiderPeacePatch(ref bool __result, Clan clan, Clan otherClan, Kingdom kingdom, IFaction otherFaction, out MakePeaceKingdomDecision decision)
    //    {
    //        if (!kingdom.Name.Contains("Shadowspawn") && !otherFaction.Name.Contains("Shadowspawn"))
    //        {
    //            decision = null;
    //            return true;
    //        }
    //        else
    //        {
    //            decision = new MakePeaceKingdomDecision(clan, otherFaction, 0, false);
    //            __result = false;
    //            return false;
    //        }
    //    }
    //}
}
