using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using WoT_Main.CampaignEventSystem;
using WoT_Main.Support;

namespace WoT_Main.Behaviours
{
    //ensures that the shadow always stays at war with everybody else
    public class ShadowAlwaysAtWar : CampaignBehaviorBase
    {
        private ConstantWars constantWars;
        private War war;

        public ShadowAlwaysAtWar(ConstantWars constantWars)
        {
            this.constantWars = constantWars;
            
        }
        public override void RegisterEvents()
        {

            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, shadowWar);
            
        }

        public override void SyncData(IDataStore dataStore) { }

        //just adds the shadow-war if it's no longer there
        private void shadowWar() 
        {


            if (!constantWars.wars.Contains(war))
            {
                war = new War(campaignSupport.getFaction("Shadowspawn"));
                constantWars.wars.Add(war);
                
            }
            else
            {
                
            }
            
        }
    }
}
