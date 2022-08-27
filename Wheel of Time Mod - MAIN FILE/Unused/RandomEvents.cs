using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

using WoT_Main.Support;
using WoT_Main.CampaignEventSystem;




namespace WoT_Main.Behaviours
{

     //Currently experimenting with it, NOT FINISHED
    public class RandomEvents : CampaignBehaviorBase
    {
        List<string> originalKingdomIds = null;
        ConstantWars constantWars;
        bool outOfKingdoms = false;
        public RandomEvents(ConstantWars constantWars)
        {
            
            if(originalKingdomIds == null)
            {

                originalKingdomIds = new List<string>();
                originalKingdomIds.Add("Seanchan");
                originalKingdomIds.Add("Altara");
                originalKingdomIds.Add("The White Tower");
                originalKingdomIds.Add("Mayene");
                originalKingdomIds.Add("The Black Tower");
                originalKingdomIds.Add("The Dragon Reborn Forces");
                originalKingdomIds.Add("Amadicia");
                originalKingdomIds.Add("Arafel");
                originalKingdomIds.Add("Ghealdan");
                originalKingdomIds.Add("Far Madding");
                originalKingdomIds.Add("Kandor");
                originalKingdomIds.Add("Tarabon");
                originalKingdomIds.Add("Shienar");
                originalKingdomIds.Add("Tear");
                originalKingdomIds.Add("Cairhien");
                originalKingdomIds.Add("Andor");
                originalKingdomIds.Add("Murandy");
                originalKingdomIds.Add("Shadowspawn");
                originalKingdomIds.Add("Illian");
                originalKingdomIds.Add("Arad Doman");
                originalKingdomIds.Add("Saldean");
                originalKingdomIds.Add("Aiel");

              
            }
            this.constantWars = constantWars;
        }
        public override void RegisterEvents()
        {

            
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, randomEvent);
            
            
        }
       
        public override void SyncData(IDataStore dataStore) {
            dataStore.SyncData("originalKingdomIds", ref originalKingdomIds);
            dataStore.SyncData("outOfKingdoms", ref outOfKingdoms);
        }
       
        private string GetRandomKingdom()
        {
            if(originalKingdomIds.Count > 0)
            {
                return originalKingdomIds.GetRandomElement();
            }

            return "invalid";
        }

      
        private void randomEvent()
        {
            //TODO: implement Scenario choser
        }

        //Method for a succesion war, determines the amount of rebels and wether there are kingdoms at all to declare a war in
        //TODO: make succesion war a scenario
        private void succesionWar(string kingdom)
        {

            string faction = GetRandomKingdom();

            if(kingdom != "invalid")
            {
                Kingdom victim = campaignSupport.getFaction(faction);


                int rebels;

                

                if (victim.Clans.Count > 2)
                {
                    rebels = MBRandom.RandomInt(2, victim.Clans.Count());
                }
                else
                {
                    rebels = victim.Clans.Count;
                }

                War war = innerFactionWarEvents.succesionWar(victim, rebels);
                constantWars.wars.Add(war);

                originalKingdomIds.Remove(faction);

                string inquiryText = " ";

                try
                {
                    foreach (Kingdom kingdom1 in war.kingdoms)
                    {
                        inquiryText += kingdom1.Name.ToString() + ", ";
                    }
                }
                catch (Exception ex)
                {
                    campaignSupport.displayMessageInChat(ex.Message, Colors.Red);
                }
                //message to the player
                InformationManager.ShowInquiry(new InquiryData("Succesion war in " + victim.Name.ToString() + "!", "The rebels are: " + inquiryText + ".", true, false, "Ok", null, null, null), true);
            }
           

                
            
        }
    }
}
