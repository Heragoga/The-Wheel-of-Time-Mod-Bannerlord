using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;




namespace Wheel_of_Time_Mod___MAIN_FILE
{
    public class ShadowAlwaysAtWar : CampaignBehaviorBase
    {
       // Color color = Color.ConvertStringToColor("green");
        Settlement shayolGhul;
        public override void RegisterEvents()
        {

            
            CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(settelment));
            

        }
       
        public override void SyncData(IDataStore dataStore)
        {
        }
       

        
        
        private void settelment(Settlement settlement)
        {
            InformationManager.DisplayMessage(new InformationMessage(Convert.ToString("4321")));
            if (Convert.ToString(settlement.GetName()) == "Shayol Ghul")
            {

                IFaction shadow = settlement.MapFaction;

        
                IFaction[] kingdoms = Kingdom.All.ToArray();
                foreach (IFaction faction in kingdoms)
                {
                    if (!shadow.IsAtWarWith(faction))
                    {
                        //FactionManager.DeclareWar(shadow, faction, true);
                        FactionManager.SetStanceTwoSided(shadow, faction, -100);
                        
                    }
                }
                InformationManager.DisplayMessage(new InformationMessage(Convert.ToString(shadow.Aggressiveness))); 


            }
            //Shayol Ghul
          
        }
    }
}
