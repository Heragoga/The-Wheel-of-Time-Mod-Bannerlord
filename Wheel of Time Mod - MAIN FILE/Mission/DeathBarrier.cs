using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using WoT_Main.Support;

namespace WoT_Main
{
    public class DeathBarrier : MissionObject
	{
		
		protected  override void OnInit()
		{
			base.OnInit();

			campaignSupport.displayMessageInChat("tick");

		}
        protected override void OnPhysicsCollision(ref PhysicsContact contact)
        {
            base.OnPhysicsCollision(ref contact);
			campaignSupport.displayMessageInChat(contact.body0.GetType().Name);
			campaignSupport.displayMessageInChat(contact.body1.GetType().Name);
			campaignSupport.displayMessageInChat("hit");
        }
        protected override void OnTickParallel(float dt)
        {
            base.OnTickParallel(dt);
			campaignSupport.displayMessageInChat("tickparallel");
		}
        protected override void OnEditorTick(float dt)
        {
            base.OnEditorTick(dt);
			campaignSupport.displayMessageInChat("editortick");
		}
        protected override void OnTickOccasionally(float currentFrameDeltaTime)
        {
            base.OnTickOccasionally(currentFrameDeltaTime);
			campaignSupport.displayMessageInChat("ontickoccasionally");
		}
		
        protected override void OnTick(float dt)	
		{
			base.OnTick(dt);
			Vec3 pos = base.GameEntity.GlobalPosition;
			Agent[] agents = Mission.Current.Agents.ToArray();
			foreach(Agent agent in agents)
            {
				if(agent.Position.x < base.GameEntity.GetBoundingBoxMax().x && agent.Position.y < base.GameEntity.GetBoundingBoxMax().y && agent.Position.z < base.GameEntity.GetBoundingBoxMax().z && agent.Position.x > base.GameEntity.GetBoundingBoxMin().x && agent.Position.y > base.GameEntity.GetBoundingBoxMin().y && agent.Position.z > base.GameEntity.GetBoundingBoxMin().z)
                {
					campaignSupport.displayMessageInChat("ded");
					agent.Health = 0;
				}
				
            }
			campaignSupport.displayMessageInChat("tick");
		}
	}
}
