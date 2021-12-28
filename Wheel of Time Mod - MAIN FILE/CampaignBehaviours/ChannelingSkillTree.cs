using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Diamond;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;
using TaleWorlds.Starter;

namespace Wheel_of_Time_Mod___MAIN_FILE
{
    class ChannelingSkillTree : CampaignBehaviorBase
    {
       
        public override void RegisterEvents()
        {
           // CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, skillUpdate);
        }
        public override void SyncData(IDataStore dataStore)
        {
        }
        private void skillUpdate()
        {
           
        }
    }
}
