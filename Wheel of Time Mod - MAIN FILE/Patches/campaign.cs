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
using TaleWorlds.CampaignSystem.GameComponents;

using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using WoT_Main.Support;

namespace WoT_Main.Patches
{
    class campaign
    {
        [HarmonyPatch]
        public static class WorldMapDebugPatch
        {
            //Testing, the method crashes, tried to get it by face id, but it doesn't seem to depend on that
            [HarmonyPrefix]
            [HarmonyPatch(typeof(MapScene), "GetNavigationMeshCenterPosition")]
            public static bool PreFix(ref MapScene __instance, ref PathFaceRecord face, ref Vec2 __result)
            {
                Vec3 zero = Vec3.Zero;
                FieldInfo fieldInfo = typeof(MapScene).GetField("_scene", BindingFlags.NonPublic | BindingFlags.Instance);
                var v = fieldInfo.GetValue(__instance);
                Scene scene = (Scene)v;
                scene.GetNavMeshCenterPosition(face.FaceIndex, ref zero);
                
                __result =  zero.AsVec2;


                return false;
            }
        }


        //Need to find a workaround around using GetNavigationMeshCenterPosition
        [HarmonyPatch]
        public static class DefaultMapDistanceModelPatch
        {
            private static Dictionary<int, Settlement> _navigationMeshClosestSettlementCache = new Dictionary<int, Settlement>();

            [HarmonyPrefix]
            [HarmonyPatch(typeof(DefaultMapDistanceModel), "GetClosestSettlementForNavigationMesh")]
            public static bool HarmonyPrefix(ref PathFaceRecord face, ref Settlement __result)
            {
                List<Settlement> _settlementsToConsider = new List<Settlement>();

                for(int i = 0; i < Settlement.All.Count; i++)
                {
                    _settlementsToConsider.Add(Settlement.All[i]);
                }

                Settlement settlement;
                if (!_navigationMeshClosestSettlementCache.TryGetValue(face.FaceIndex, out settlement))
                {
                      
                    Vec2 navigationMeshCenterPosition = Campaign.Current.MapSceneWrapper.GetNavigationMeshCenterPosition(face);
                    float num = float.MaxValue;
                    foreach (Settlement settlement2 in _settlementsToConsider)
                    {
                        float num2 = settlement2.GatePosition.DistanceSquared(navigationMeshCenterPosition);
                        if (num > num2)
                        {
                            num = num2;
                            settlement = settlement2;
                        }
                    }
                    _navigationMeshClosestSettlementCache[face.FaceIndex] = settlement;
                }
                __result = settlement;
                
                return false;
            }
           
        }
    }
}
