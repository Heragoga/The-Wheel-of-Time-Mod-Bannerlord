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

namespace Wheel_of_Time_Mod___MAIN_FILE.Patches
{
    public class ChannelingSkillTreePatcher
    {
        
        
        public void Patch()
        {
      
            Harmony harmony = new Harmony("patch");
           
            try
            {

                
                MethodInfo original = typeof(DefaultSkills).GetMethod("RegisterAll");
                MethodInfo replacement = typeof(NamePatch).GetMethod("Rename");
                Console.WriteLine("tet");
                harmony.Patch(original, null, new HarmonyMethod(replacement));
            }
            catch(Exception e)
            {
                FileLog.LogBuffered(e.ToString());
                throw e;
            }
        }
      
    }
}
