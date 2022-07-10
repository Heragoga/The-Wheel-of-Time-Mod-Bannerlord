using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using WoT_Main.Support;

namespace WoT_Main.Behaviours
{
    class DeathBarrier2 : MissionLogic
    {
        
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            MissionObject[] missionObjects = Mission.ActiveMissionObjects.ToArray();
            foreach(MissionObject missionObject in missionObjects)
            {
                //campaignSupport.displayMessageInChat(missionObject.ToString() + " " + missionObject.GetType().Name);
                if(missionObject.GetType().Name == "DeathBarrier")
                {
                    campaignSupport.displayMessageInChat(Agent.Main.Position.x - missionObject.GameEntity.GetBoundingBoxMax().x + "");campaignSupport.displayMessageInChat(Agent.Main.Position.x - missionObject.GameEntity.GetBoundingBoxMax().y + "");campaignSupport.displayMessageInChat(Agent.Main.Position.z - missionObject.GameEntity.GetBoundingBoxMax().z + "");
                    Agent[] agents = Mission.Current.Agents.ToArray();
                    foreach (Agent agent in agents)
                    {
                       
                        if (agent.Position.x < missionObject.GameEntity.GetBoundingBoxMax().x &&
                            agent.Position.y < missionObject.GameEntity.GetBoundingBoxMax().y && 
                            agent.Position.z < missionObject.GameEntity.GetBoundingBoxMax().z && 
                            agent.Position.x > missionObject.GameEntity.GetBoundingBoxMin().x && 
                            agent.Position.y > missionObject.GameEntity.GetBoundingBoxMin().y && 
                            agent.Position.z > missionObject.GameEntity.GetBoundingBoxMin().z)
                        {
                            campaignSupport.displayMessageInChat("ded");
                            agent.Health = 0;
                        }

                    }
                }
            }

        }
    }
}
