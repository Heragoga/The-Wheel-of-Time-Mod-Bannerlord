using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoT_Main.CampaignEventSystem.Scenarios
{
    public abstract class Scenario
    {
        //must be implemented by all scneatios so that they can be checked, finished and saved without worry
        public abstract War start();
        public abstract bool checkVictory();
        public abstract War finishUp();
        public abstract List<string> save();
    }
}
