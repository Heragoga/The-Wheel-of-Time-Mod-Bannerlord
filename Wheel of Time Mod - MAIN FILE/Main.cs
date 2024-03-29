﻿using HarmonyLib;
using System;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;

using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem;
using WoT_Main.Behaviours;
using WoT_Main.Support;
using TaleWorlds.Library;
using System.Collections.Generic;
using System.Reflection;

using System.IO;
using TaleWorlds.Engine.Screens;

using SandBox;

using WoT_Main.Cheats;
using WoT_Main.Behaviours.Campaign;
using Newtonsoft.Json.Linq;

namespace WoT_Main
{
    public class Main : MBSubModuleBase
    {
        private JObject config;
        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {

            base.OnBeforeInitialModuleScreenSetAsRoot();
            //Just for confirmation
            campaignSupport.displayMessageInChat("WoT Core loaded", Color.White);

        }

        protected override void OnSubModuleLoad()
        {

            base.OnSubModuleLoad();

            try
            {
                this.config = JObject.Parse(File.ReadAllText(Path.GetFullPath(BasePath.Name + "Modules/Wheel of Time Mod/config.json")));
            }
            catch (Exception)
            {
            }

            TextObject coreContentDisabledReason = new TextObject("Disabled during installation.", null);

            //Removing Start screen options which are not needed for the mod

            startScreenSupport.removeInitialStateOption("CustomBattle");
            startScreenSupport.removeInitialStateOption("StoryModeNewGame");
            startScreenSupport.removeInitialStateOption("Credits");
            startScreenSupport.removeInitialStateOption("SandBoxNewGame");
            
            //Adds out customn gamemode, currently not much
            TaleWorlds.MountAndBlade.Module.CurrentModule.AddInitialStateOption(new InitialStateOption("SandBoxNewGame", new TextObject("New WoT Campaign", null), 3, delegate ()
            {
                MBGameManager.StartNewGame(new WoTCampaignManager());
            }, () => new ValueTuple<bool, TextObject>(TaleWorlds.MountAndBlade.Module.CurrentModule.IsOnlyCoreContentEnabled, coreContentDisabledReason)));



            //Bread
            InitialStateOption initialStateOption = new InitialStateOption("Donate", new TextObject("Donate"), 1233, Donate, menuFunc);
            TaleWorlds.MountAndBlade.Module.CurrentModule.AddInitialStateOption(initialStateOption);


            Harmony harmony = new Harmony("WoT_Main.HarmonyPatches");
            Harmony.DEBUG = true;
            harmony.PatchAll();
        }
        

        public override void OnMissionBehaviorInitialize(Mission mission)
        {

            base.OnMissionBehaviorInitialize(mission);

            //Mission behaviour which disables friendly fire
            mission.AddMissionBehavior(new NoFriendlyFire());
            //mission.AddMissionBehavior(new ChannelingSound());
            mission.AddMissionBehavior(new DeathBarrier2());

        }
      
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {

            base.OnGameStart(game, gameStarterObject);

            if(game.GameType is Campaign)
            {

                CampaignGameStarter starter = gameStarterObject as CampaignGameStarter;

                
                

                //starter.AddBehavior(constantWars);
                //starter.AddBehavior(new ShayolGhulCaptureMechanic());
                //starter.AddBehavior(new RandomEvents(constantWars));
                starter.AddBehavior(new ShadowAlwaysAtWar());


                starter.AddBehavior(new PartyMapStuckFix());

                
            }
        }  
        
        private void Donate()
        {
            //Open browser and navigate to given link
            System.Diagnostics.Process.Start("https://www.patreon.com/WoT_Mod");
        }

       

        private (bool, TextObject) menuFunc()
        {
            return (false, null);
        }
    }
}
