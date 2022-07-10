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
    class ChannelingSound : MissionLogic
    {
     
        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            int soundIndex = SoundEvent.GetEventIdFromString("fireball1");
            int soundIndex1 = SoundEvent.GetEventIdFromString("example/voice/charge");
            
            //campaignSupport.displayMessageInChat("Sound index: " + soundIndex);
            //campaignSupport.displayMessageInChat("Sound index: " + soundIndex1);

            if (Input.IsKeyDown(InputKey.J))
            {
                Mission.MakeSound(soundIndex, Agent.Main.Position, false, true, -1, -1);
            }
           
        }
    }
}
       