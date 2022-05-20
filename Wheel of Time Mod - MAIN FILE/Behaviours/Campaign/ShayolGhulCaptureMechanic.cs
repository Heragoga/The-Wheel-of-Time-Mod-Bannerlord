using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using WoT_Main.Support;
using TaleWorlds.SaveSystem;


namespace WoT_Main.Behaviours
{
    public class ShayolGhulCaptureMechanic : CampaignBehaviorBase
    {
       
        private bool wonTheGame;

       
        public override void RegisterEvents()
        {

            CampaignEvents.HourlyTickSettlementEvent.AddNonSerializedListener(this, new Action<Settlement>(settlementUpdate));

        }

        public override void SyncData(IDataStore dataStore)
        {

            dataStore.SyncData("wonTheGame", ref wonTheGame);
            
        }




        public void settlementUpdate(Settlement settlement)
        {

            IFaction shadow = settlement.MapFaction;
            if (!wonTheGame && settlement.OwnerClan == Hero.MainHero.Clan && shadow != null && settlement.Name.ToString() == "Shayol Ghul")
            {

                InformationManager.ShowInquiry(new InquiryData("Shayol Ghul is taken!", "Shayol ghul is taken..." + Environment.NewLine + "but what to do with the dark one?", true, true, "I AM THE" + Environment.NewLine + "DARK ONE!!!", "Kill him and " + Environment.NewLine + "dissolve the shadow.", takeControllOfShadow, dissolveShadow), true);

                wonTheGame = true;
            }
            

        }

        private void dissolveShadow()
        {
            IFaction shadow = campaignSupport.getFaction("Shadowspawn");
           
            Hero[] shadowLords = shadow.Heroes.ToArray();
            List<Clan> clans = new List<Clan>();

            foreach (Hero hero in shadowLords)
            {
                if (!clans.Contains(hero.Clan))
                {
                    clans.Add(hero.Clan);
                }

            }

            foreach (Clan clan in clans)
            {


                KingdomManager kingdomManager = new KingdomManager();
                kingdomManager.CreateKingdom(new TextObject(clan.Name.ToString() + "'s Kingdom"), new TextObject(clan.Name.ToString() + "'s Kingdom"), clan.Culture, clan);
                Kingdom _kingdom = null;
                Kingdom[] _kingdoms = Kingdom.All.ToArray();
                foreach (Kingdom faction in _kingdoms)
                {
                    if (faction.Name.ToString() == clan.Name.ToString())
                    {
                        _kingdom = faction;

                    }
                }
            }
        }

        private void takeControllOfShadow()
        {


            IFaction shadow = campaignSupport.getFaction("Shadowspawn");


            KingdomManager kingdomManager = new KingdomManager();
            if (shadow.Leader != Hero.MainHero && shadow != null && Hero.MainHero.CanLeadParty() && Hero.MainHero != null && !shadow.IsEliminated)
            {

                kingdomManager.CreateKingdom(new TextObject("New Shadow"), new TextObject("New Shadow"), shadow.Culture, Hero.MainHero.Clan);


            }
            Settlement[] shadowSettlements = shadow.Settlements.ToArray();
            IFaction newShadow = campaignSupport.getFaction("New Shadow");
            foreach (Settlement settlement in shadowSettlements)
            {
                kingdomManager.GiftSettlementOwnership(settlement, Hero.MainHero.Clan);
            }


        }
    }
}
