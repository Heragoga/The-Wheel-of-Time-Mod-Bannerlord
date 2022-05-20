using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoT_Main.CampaignEventSystem.Scenarios
{
    public abstract class Scenario
    {
        
        public abstract War start();
        public abstract bool checkVictory();
        public abstract War finishUp();
        public abstract List<string> save();
    }
}
