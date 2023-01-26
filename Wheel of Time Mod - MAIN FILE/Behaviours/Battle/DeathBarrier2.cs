using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

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
                    
               

                    Agent[] agents = Mission.Current.Agents.ToArray();
                    foreach (Agent agent in agents)
                    {
                       
                        if (agent.Position.x < missionObject.GameEntity.GlobalPosition.x + 2.5f &&
                            agent.Position.y < missionObject.GameEntity.GlobalPosition.y + 2.5f && 
                            agent.Position.z < missionObject.GameEntity.GlobalPosition.z + 2.5f && 
                            agent.Position.x > missionObject.GameEntity.GlobalPosition.x - 2.5f && 
                            agent.Position.y > missionObject.GameEntity.GlobalPosition.y - 2.5f && 
                            agent.Position.z > missionObject.GameEntity.GlobalPosition.z - 2.5f)
                        {


                           
                                agent.Die(CreateMissileBlow(Agent.Main));
                            
                         



                        }

                    }
                }
            }

        }

		private Blow CreateMissileBlow(Agent attackerAgent)
		{
			Blow blow = new Blow(attackerAgent.Index);
            
            blow.BlowFlag = BlowFlags.NoSound;

			blow.Direction = Vec3.Forward;
			blow.SwingDirection = blow.Direction;

			blow.Position = Vec3.Zero;
			
			blow.BoneIndex = 0;
			
			blow.StrikeType = StrikeType.Swing;
			
			blow.DamageType = DamageTypes.Cut;

			blow.VictimBodyPart = BoneBodyPartType.Head; 
				
			blow.BaseMagnitude = 1.0f;
			blow.MovementSpeedDamageModifier = 1;
			blow.AbsorbedByArmor = 1f;
			blow.InflictedDamage = 169;
			blow.SelfInflictedDamage = 69;
			blow.DamageCalculated = true;
			return blow;
		}
	}

}
