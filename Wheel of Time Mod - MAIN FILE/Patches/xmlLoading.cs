using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using TaleWorlds.Core;

namespace WoT_Main.Patches
{
    class xmlLoading
    {
        //Relic

        //[HarmonyPatch(typeof(GameTextManager), "LoadGameTexts")]
        //public class GameTextManagerLoadGameTexts
        //{
        //    private static string filePath = new FileInfo(Assembly.GetExecutingAssembly().Location)
        //        .Directory + "/../../ModuleData/module_strings.xml";
        //    public static void Prefix(ref string xmlPath)
        //    {
        //        if (xmlPath.Equals(@"..\..\Modules\Native/ModuleData/module_strings.xml"))
        //        {
        //            xmlPath = filePath;
        //        }
        //    }
        //}
    }
}
