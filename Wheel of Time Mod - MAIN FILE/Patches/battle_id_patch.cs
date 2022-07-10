using HarmonyLib;
using Helpers;
using SandBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.GameComponents.Map;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using WoT_Main.Support;

namespace WoT_Main.Patches
{
    [HarmonyPatch]
    public static class GetFaceTerrainTypePatch
    {
        //Testing, the method crashes, tried to get it by face id, but it doesn't seem to depend on that
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapScene), "GetFaceTerrainType")]
        public static bool PreFix(ref PathFaceRecord navMeshFace, ref TerrainType __result)
        {
			if (!navMeshFace.IsValid())
			{
				__result =  TerrainType.Plain;
			}
			int faceGroupIndex = navMeshFace.FaceGroupIndex;
			switch (faceGroupIndex)
			{
				case 1:
					__result = TerrainType.Plain; break;
				case 2:
					__result = TerrainType.Desert; break;
				case 3:
					__result = TerrainType.Snow; break;
				case 4:
					__result = TerrainType.Forest; break;
				case 5:
					__result = TerrainType.Steppe; break;
				case 6:
					__result = TerrainType.ShallowRiver; break;
				case 7:
					__result = TerrainType.Mountain; break;
				case 8:
					__result = TerrainType.Lake; break;
				case 10:
					__result = TerrainType.Water; break;
				case 11:
					__result = TerrainType.River; break;
				case 13:
					__result = TerrainType.Canyon; break;
				case 14:
					__result = TerrainType.RuralArea; break;
				default:
					__result = TerrainType.Plain; break;
			}

		


			return false;
        }
    }    
	
	[HarmonyPatch]
    public static class GetNavigationMeshIndexOfTerrainTypePatch
	{
        //Testing, the method crashes, tried to get it by face id, but it doesn't seem to depend on that
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapScene), "GetNavigationMeshIndexOfTerrainType")]
        public static bool PreFix(ref TerrainType terrainType, ref int __result)
        {
			switch (terrainType)
			{
				case TerrainType.Water:
					__result = 10; break;
				case TerrainType.Mountain:
					__result = 7; break;
				case TerrainType.Snow:
					__result = 3; break;
				case TerrainType.Steppe:
					__result = 5; break;
				case TerrainType.Plain:
					__result = 1; break;
				case TerrainType.Desert:
					__result = 2; break;
				case TerrainType.River:
					__result = 11; break;
				case TerrainType.Forest:
					__result = 4; break;
				case TerrainType.ShallowRiver:
					__result = 6; break;
				case TerrainType.Lake:
					__result = 8; break;
				case TerrainType.Canyon:
					__result = 13; break;
				case TerrainType.RuralArea:
					__result = 14; break;
				default:  __result = -1; break;
			}
			

			return false;
        }
    }
	
	[HarmonyPatch]
    public static class GetTerrainTypeNamePatch
	{
        //Testing, the method crashes, tried to get it by face id, but it doesn't seem to depend on that
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapScene), "GetTerrainTypeName")]
        public static bool PreFix(ref TerrainType type, ref string __result)
        {
			string result = "Invalid";
			switch (type)
			{
				case TerrainType.Water:
					result = "Water";
					break;
				case TerrainType.Mountain:
					result = "Mountain";
					break;
				case TerrainType.Snow:
					result = "Snow";
					break;
				case TerrainType.Steppe:
					result = "Steppe";
					break;
				case TerrainType.Plain:
					result = "Plain";
					break;
				case TerrainType.Desert:
					result = "Desert";
					break;
				case TerrainType.Swamp:
					result = "Swamp";
					break;
				case TerrainType.Dune:
					result = "Dune";
					break;
				case TerrainType.Bridge:
					result = "Bridge";
					break;
				case TerrainType.River:
					result = "River";
					break;
				case TerrainType.Forest:
					result = "Forest";
					break;
				case TerrainType.ShallowRiver:
					result = "ShallowRiver";
					break;
				case TerrainType.Lake:
					result = "Lake";
					break;
				case TerrainType.Canyon:
					result = "Canyon";
					break;
			}
			__result =  result;
			

			return false;
        }
    }
	
	[HarmonyPatch]
    public static class GetBattleSceneForMapPatchPatch
	{
        //Testing, the method crashes, tried to get it by face id, but it doesn't seem to depend on that
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerEncounter), "GetBattleSceneForMapPatch")]
        public static bool PreFix(ref MapPatchData mapPatch, ref string __result)
        {
			PathFaceRecord id1 = Campaign.Current.MapSceneWrapper.GetFaceIndex(MobileParty.MainParty.Position2D);
			campaignSupport.displayMessageInChat(id1.FaceIndex + " re");
			int id = mapPatch.sceneIndex;
			List<SingleplayerBattleSceneData> list = (from scene in GameSceneDataManager.Instance.SingleplayerBattleScenes
													  where scene.MapIndices.Contains(id)
													  select scene).ToList<SingleplayerBattleSceneData>();
			string sceneID;
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

			__result = "scn_blight2";
			//__result = sceneID;

			campaignSupport.displayMessageInChat("found scene " + sceneID + " " + id);

			return false;
        }
    }
}
