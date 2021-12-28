using System;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace Wheel_of_Time_Mod___MAIN_FILE
{
    class NoFriendlyFire : MissionLogic
    {
        //On Agent Hit is called every time and agent (unit, including including the player) receives damage
        public override void OnAgentHit(Agent affectedAgent, Agent affectorAgent, int damage, in MissionWeapon affectorWeapon)
        {
            //One Power no friendly Fire
            //Checks the name of the Weapon used
            if (affectorWeapon.Item != null && affectorWeapon.Item.Name.Contains("onepower"))
            {
                
                if(affectedAgent.Team == affectorAgent.Team)
                {
                    //kinda crude and does not work always but i have no better solution
                    affectedAgent.Health = affectedAgent.Health + damage;
                }
            }
        }
        
    }
}
