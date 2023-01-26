using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace WoT_Main.Behaviours
{
    class NoFriendlyFire : MissionLogic
    {
        
        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, in MissionWeapon affectorWeapon, in Blow blow, in AttackCollisionData attackCollisionData)
        {
            base.OnAgentHit(affectedAgent, affectorAgent, affectorWeapon, blow, attackCollisionData);
            if (affectorWeapon.Item != null && affectorWeapon.Item.Name.Contains("onepower"))
            {

                if (affectedAgent.Team == affectorAgent.Team)
                {

                    affectedAgent.Health = affectedAgent.Health + blow.InflictedDamage;

                }
            }
        }
       

    }
}
