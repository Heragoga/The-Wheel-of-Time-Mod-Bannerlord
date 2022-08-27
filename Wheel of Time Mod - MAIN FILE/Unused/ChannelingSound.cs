using System;
using System.Linq;
using System.Reflection;
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

            if (Input.IsKeyDown(InputKey.J))
            {
                string s = Assembly.GetAssembly(typeof(ChannelingSound)).Location.Replace("WoT_Main.dll", "test.wav");
                //campaignSupport.displayMessageInChat(s);
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(s);
                player.Play();
                
            }
           
        }
    }
}
       