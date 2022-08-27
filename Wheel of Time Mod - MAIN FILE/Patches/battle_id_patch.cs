using HarmonyLib;
using Helpers;
using Newtonsoft.Json.Linq;
using SandBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using WoT_Main.Support;
using Path = System.IO.Path;


namespace WoT_Main.Patches
{
    
	[HarmonyPatch]
    public static class GetBattleSceneForMapPatchPatch
	{
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerEncounter), "GetBattleSceneForMapPatch")]
        public static bool PreFix(ref MapPatchData mapPatch, ref string __result)
        {
			PathFaceRecord faceIndex = Campaign.Current.MapSceneWrapper.GetFaceIndex(MobileParty.MainParty.Position2D);

			
			int id = faceIndex.FaceGroupIndex;

			string sceneID = "n";

			if (id <= 14)
            {
				
				List<SingleplayerBattleSceneData> list = (from scene in GameSceneDataManager.Instance.SingleplayerBattleScenes
														  where scene.MapIndices.Contains(id)
														  select scene).ToList<SingleplayerBattleSceneData>();
				
				if (list.IsEmpty<SingleplayerBattleSceneData>())
				{
					sceneID = GameSceneDataManager.Instance.SingleplayerBattleScenes.GetRandomElement<SingleplayerBattleSceneData>().SceneID;
				}
				else if (list.Count<SingleplayerBattleSceneData>() > 1)
				{
					sceneID = list.GetRandomElement<SingleplayerBattleSceneData>().SceneID;
				}
				else
				{
					sceneID = list[0].SceneID;
				}
			}
            else
            {
				
				try
				{
					JObject config = JObject.Parse(File.ReadAllText(Path.GetFullPath(BasePath.Name + "Modules/Wheel of Time Mod - MAIN FILE/config.json")));
					
					string[] sceneIds = null;
					
					
					switch (id)
                    {
						case 50: sceneIds = config.GetValue("faceIndex50").ToObject<string[]>(); break;
						case 51: sceneIds = config.GetValue("faceIndex51").ToObject<string[]>(); break;
						case 52: sceneIds = config.GetValue("faceIndex52").ToObject<string[]>(); break;
						case 53: sceneIds = config.GetValue("faceIndex53").ToObject<string[]>(); break;
						case 54: sceneIds = config.GetValue("faceIndex54").ToObject<string[]>(); break;
						case 55: sceneIds = config.GetValue("faceIndex55").ToObject<string[]>(); break;
						case 56: sceneIds = config.GetValue("faceIndex56").ToObject<string[]>(); break;
						case 57: sceneIds = config.GetValue("faceIndex57").ToObject<string[]>(); break;
						case 58: sceneIds = config.GetValue("faceIndex58").ToObject<string[]>(); break;
						case 59: sceneIds = config.GetValue("faceIndex59").ToObject<string[]>(); break;
						default: config.GetValue("faceIndex50").ToObject<string[]>(); break;
					}
				

						
					if (sceneID == "n" && sceneIds != null)
                    {
						sceneID = sceneIds.GetRandomElement();
					}
					
				}
				catch (Exception)
				{
					campaignSupport.displayMessageInChat("config wasn't loaded", Colors.Red);
				}
			}

			//__result = "scn_test";
			__result = sceneID;

			return false;
        }
    }
}
