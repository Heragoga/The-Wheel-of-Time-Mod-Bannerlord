using TaleWorlds.CampaignSystem;
using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using WoT_Main.Support;
using SandBox;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;

namespace WoT_Main.Behaviours.Campaign
{
    //Teleports a party a little bit forward if it is in a place for too long, not very efficient, not sure how to improve it
    //basically moves a party up in "danger"
    public class PartyMapStuckFix : CampaignBehaviorBase
    {
        float amountToMove = 0.5f;
        List<MobileParty> parties = new List<MobileParty>();
        List<Vec2> partiesPositions = new List<Vec2>();
        List<MobileParty> partiesDanger1 = new List<MobileParty>();
        List<MobileParty> partiesDanger2 = new List<MobileParty>();
        List<MobileParty> partiesDanger3 = new List<MobileParty>();
        public override void RegisterEvents()
        {

            CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, HourlyTickPartyEventListener);
            CampaignEvents.OnPartyDisbandedEvent.AddNonSerializedListener(this, OnPartyDisbandedEventListener);
            
        }

        public override void SyncData(IDataStore dataStore)
        {

           

        }

        private void HourlyTickPartyEventListener(MobileParty mobileParty)
        {
            if (!parties.Contains(mobileParty))
            {
                parties.Add(mobileParty);
                partiesPositions.Add(mobileParty.Position2D);
            }
            else
            {
                if(mobileParty.Position2D == partiesPositions[parties.IndexOf(mobileParty)])
                {
                    if (!partiesDanger1.Contains(mobileParty))
                    {
                        partiesDanger1.Add(mobileParty);
                    }
                    else if(!partiesDanger2.Contains(mobileParty))
                    {
                        partiesDanger2.Add(mobileParty);
                    }
                    else if (!partiesDanger3.Contains(mobileParty))
                    {
                        partiesDanger3.Add(mobileParty);
                    }
                    else
                    {
                        Vec2 targetPos = mobileParty.TargetPosition;
                        Vec2 currentPos = mobileParty.Position2D;
                        float xChange = 0;
                        float yChange = 0;

                        Vec2 normalizedDirection = targetPos - currentPos;
                        float x = (float)Math.Sqrt(normalizedDirection.x * normalizedDirection.x + normalizedDirection.y * normalizedDirection.y);
                        normalizedDirection = new Vec2(normalizedDirection.x * (1 / x), normalizedDirection.x * (1 / x));

                        xChange = normalizedDirection.x;
                        yChange = normalizedDirection.y;

                        try
                        {
                            if(mobileParty != MobileParty.MainParty && mobileParty.IsMoving)
                            {
                                MapScene scene = (TaleWorlds.CampaignSystem.Campaign.Current.MapSceneWrapper as MapScene);

                               
                               
                                if (scene.GetFaceIndex(new Vec2(mobileParty.Position2D.X + xChange, mobileParty.Position2D.Y + yChange)).IsValid())
                                {
                                    mobileParty.Position2D = new Vec2(mobileParty.Position2D.X + xChange, mobileParty.Position2D.Y + yChange);
                                }
                                else
                                {
                                    mobileParty.Position2D = mobileParty.TargetPosition;
                                    
                                }
                            }
                            
                        }
                        catch(Exception ex)
                        {
                            campaignSupport.displayMessageInChat(ex.Message);
                        }
                        
                       
                        
                        partiesDanger1.Remove(mobileParty);
                        partiesDanger2.Remove(mobileParty);
                        partiesDanger3.Remove(mobileParty);
                    }
                }
                else
                {
                    partiesPositions[parties.IndexOf(mobileParty)] = mobileParty.Position2D;
                }
            }
            

            
            
        }

        private void OnPartyDisbandedEventListener(MobileParty mobileParty, Settlement settlement)
        {
           
            try
            {
                partiesPositions.RemoveAt(parties.IndexOf(mobileParty));
                parties.Remove(mobileParty);
               
            }
            catch(Exception ex)
            {
                campaignSupport.displayMessageInChat(ex.Message, Colors.Red);
            }
            
        }
    }
}
