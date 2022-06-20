using HarmonyLib;
using Helpers;
using SandBox;
using SandBox.GauntletUI.CharacterCreation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.CharacterCreationContent;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterCreation;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;
using WoT_Main.Support;

namespace WoT_Main.Patches
{
    //[HarmonyPatch]
    //public static class ClanNamingStageFix
    //{
    //
    //    [HarmonyPrefix]
    //    [HarmonyPatch(typeof(CharacterCreationReviewStageVM), "AddReviewedItems")]
    //    public static bool Prefix(ref CharacterCreationReviewStageVM __instance)
    //    {
    //        FieldInfo fieldInfo = typeof(CharacterCreationReviewStageVM).GetField("_currentContent", BindingFlags.Instance | //BindingFlags.NonPublic);
    //
    //        var v = fieldInfo.GetValue(__instance);
    //
    //        FieldInfo fieldInfo2 = typeof(CharacterCreationContentBase).GetField("_culture", BindingFlags.Instance | //BindingFlags.NonPublic);
    //
    //        CultureObject culture = null;
    //
    //        CultureObject[] cultures = MBObjectManager.Instance.GetObjectTypeList<CultureObject>().ToArray();
    //        foreach (CultureObject culture1 in cultures)
    //        {
    //            if (culture1.Name.ToString().ToLower() == "The White Tower".ToLower())
    //            {
    //                culture = culture1;
    //            }
    //           
    //        }
    //        fieldInfo2.SetValue(v, culture);
    //        return true;
    //    }
    //}
    //
    //[HarmonyPatch]
    //public static class CultureStageSkiper
    //{

    //[HarmonyPrefix]
    //[HarmonyPatch(typeof(CharacterCreationContentBase), "GetSelectedCulture")]
    //public static bool Prefix(ref CharacterCreationContentBase __instance,  ref CultureObject __result)
    //{
    //     try
    //     {
    //         
    //         FieldInfo fieldInfo = typeof(CharacterCreationContentBase).GetField("_culture", BindingFlags.NonPublic | BindingFlags.Instance);
    //         var v = fieldInfo.GetValue(CharacterCreationContentBase.Instance);
    //         
    //         if (v == null)
    //         {
    //             CultureObject[] cultures = CharacterCreationContentBase.Instance.GetCultures().ToArray();
    //             foreach(CultureObject culture in cultures){
    //                 if(culture.Id.ToString() == "sturgia")
    //                 {
    //                     v = culture;
    //                 }
    //             }
    //             if(v == null)
    //             {
    //                 foreach (CultureObject culture in cultures)
    //                 {
    //                     
    //                      v = culture;
    //                     
    //                 }
    //             }
    //         }
    //
    //         //updates the value
    //         fieldInfo.SetValue(typeof(CharacterCreationContentBase).GetField("_culture", BindingFlags.NonPublic | BindingFlags.Instance), v);
    //     }
    //     catch (Exception ex)
    //     {
    //
    //         campaignSupport.displayMessageInChat(ex.Message, Color.White);
    //     }
    //
    //     return false;
    //}
    //
    //[HarmonyPatch]
    //public static class CultureSorter
    //{
    //
    //    [HarmonyPrefix]
    //    [HarmonyPatch(typeof(CharacterCreationContentBase), "GetCultures")]
    //    public static bool Prefix( ref IEnumerable<CultureObject> __result)
    //    {
    //        campaignSupport.displayMessageInChat("sfafg");
    //        IEnumerable<CultureObject> cultures = new List<CultureObject>();
    //        foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
    //        {
    //            if (cultureObject.IsMainCulture && (cultureObject.Id.ToString() == "empire" || /cultureObject.Id.ToString/() == "sturgia" || cultureObject.Id.ToString() == "Dragonsworn" || /cultureObject.Id.ToString() == /"Seanchan" || cultureObject.Id.ToString() == "WhiteTower"))
    //            {
    //                cultures.AddItem(cultureObject);
    //            }
    //        }
    //
    //        foreach (CultureObject cultureObject in MBObjectManager.Instance.GetObjectTypeList<CultureObject>())
    //        {
    //            if (cultureObject.IsMainCulture && !cultures.Contains(cultureObject))
    //            {
    //                cultures.AddItem(cultureObject);
    //            }
    //        }
    //        __result = cultures;
    //        campaignSupport.displayMessageInChat("sfafg");
    //        return false;
    //    }
    //}

   //s [HarmonyPatch]
   //s public static class CultureSorter
   //s {
   //s 
   //s     [HarmonyPostfix]
   //s     [HarmonyPatch(typeof(CharacterCreationContentBase), "GetCultures")]
   //s     public static bool Postfix( ref IEnumerable<CultureObject> __result)
   //s     {
   //s         campaignSupport.displayMessageInChat("Don't forget to scroll down for more cultures.");
   //s        
   //s         return true;
   //s     }
   //s }
}
