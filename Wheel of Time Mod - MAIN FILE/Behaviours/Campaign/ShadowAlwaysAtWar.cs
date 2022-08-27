using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;

using WoT_Main.Support;

namespace WoT_Main.Behaviours
{
    //ensures that the shadow always stays at war with everybody else
    public class ShadowAlwaysAtWar : CampaignBehaviorBase
    {
      
        public override void RegisterEvents()
        {

            CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, shadowWar);
            
        }

        public override void SyncData(IDataStore dataStore) { }

        
        private void shadowWar() 
        {
            List<Kingdom> factions = Kingdom.All.ToList();
            Kingdom shadow = campaignSupport.getFaction("Shadow");
            if(shadow != null && shadow.Settlements.Count > 0)
            {
                foreach (Kingdom kingdom in factions)
                {
                    if (kingdom != shadow)
                    {
                        if (!kingdom.IsAtWarWith(shadow))
                        {
                            FactionManager.DeclareWar(kingdom, shadow, true);
                        }
                    }
                }
            }
            
            
        }
    }
}
