using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using WoT_Main.Support;
using TaleWorlds.Core;


namespace WoT_Main.CampaignEventSystem.Scenarios
{
    class TaxCollectorArrivesToRegion : Scenario
    {
        CampaignTime expirationDate;
        string region = "";
        string _attacker = "";
        string _victim = "";

        //still not finished, first implementation of scenario, likley doesn't work

        public TaxCollectorArrivesToRegion(List<string> strings)
        {

            expirationDate = CampaignTime.Days((float)Convert.ToDouble(strings[0]));
            region = strings[1];
            _attacker = strings[2];
            _victim = strings[3];
        }

        public TaxCollectorArrivesToRegion()
        {
            
        }

        public override War start()
        {
            War war;

            Kingdom attacker = campaignSupport.getFaction("Andor");
            Kingdom victim = campaignSupport.getFaction("The Dragon Reborn Forces");

            Settlement _region = campaignSupport.getSettlement("Emonds Field");

            if(attacker != null && victim != null && region != null)
            {
                war = new War(new List<Kingdom> { attacker, victim });
                expirationDate = CampaignTime.Now + CampaignTime.Years(0.5f);
                region = _region.Name.ToString();
                _attacker = attacker.GetName().ToString();
                _victim = victim.GetName().ToString();


                InformationManager.ShowInquiry(new TaleWorlds.Library.InquiryData("News", "A tax collector from " + attacker.GetName() + " has arrived at " + region + " to collect the first money in generations. He was met with laughter. " + attacker.GetName() + " have send an army.", true, false, "Ok", null, null, null), true);
            }
            else
            {
                return null;
            }

            return war;
        }

        public override bool checkVictory()
        {
            if(CampaignTime.Now >= expirationDate || campaignSupport.getSettlement(region).MapFaction == campaignSupport.getFaction(_attacker))
            {
                InformationManager.ShowInquiry(new TaleWorlds.Library.InquiryData("News", "The dispute between " + _attacker + " and " + _victim + " over " + region + " has ended!", true, false, "Ok", null, null, null), true);
                return true;
            }
            return false;
        }

        public override War finishUp()
        {
            War warToRemove = new War(new List<Kingdom> { campaignSupport.getFaction(_attacker), campaignSupport.getFaction(_victim) });
            expirationDate = CampaignTime.Never;
            region = null;
            _attacker = null;
            _victim = null;
            return warToRemove;
        }

        public override List<string> save(){
            List<string> result = new List<string>();
            result.Add("Type");
            result.Add("TaxCollectorArrivesToRegion");
            result.Add("Values");
            result.Add("" + expirationDate.ElapsedDaysUntilNow);
            result.Add(region);
            result.Add(_attacker);
            result.Add(_victim);

            return result;
           
        }
    }
}
