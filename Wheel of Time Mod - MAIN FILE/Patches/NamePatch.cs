using HarmonyLib;
using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.CampaignSystem.SandBox.Issues;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using TaleWorlds;


namespace Wheel_of_Time_Mod___MAIN_FILE.Patches
{
    class NamePatch
    {
       public void Rename()
        {
            InformationManager.DisplayMessage(new InformationMessage("IT WORKS!!!!!!!!!!!"));
        }
    }
}
