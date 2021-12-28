using HarmonyLib;
using System;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Missions;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.SandBox.CampaignBehaviors;
using Wheel_of_Time_Mod___MAIN_FILE.Patches;




namespace Wheel_of_Time_Mod___MAIN_FILE
{
    public class Main : MBSubModuleBase
    {
        public ChannelingSkillTreePatcher ChannelingSkillTreePatcher;
        protected virtual void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            ChannelingSkillTreePatcher ChannelingSkillTreePatcher = new ChannelingSkillTreePatcher();
            Console.WriteLine("test");
            ChannelingSkillTreePatcher.Patch();


            //Harmony harmony = new Harmony("Wheel_of_Time_Mod___MAIN_FILE.HarmonyPatches");
            //harmony.PatchAll();
            
        }

        //Mission (the battles) behaviours, behaviours are in seperate classes (Mission Behaviours) and must be added
        public override void OnMissionBehaviourInitialize(Mission mission)
        {
            base.OnMissionBehaviourInitialize(mission);
            //The NoFriendlyFire Module
            mission.AddMissionBehaviour(new NoFriendlyFire());
            

        }
        //called when the game reloads (a battle beginns, the man menu or campaign also dialog i think)
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            //behaviours for campaigns
            if(game.GameType is Campaign)
            {
                CampaignGameStarter starter = gameStarterObject as CampaignGameStarter;
                starter.AddBehavior(new ShadowAlwaysAtWar());
                //starter.AddBehavior(new ChannelingSkillTree());
                
            }
           
        }
        
    }
}
