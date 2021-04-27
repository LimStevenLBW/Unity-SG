using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Systems.Combat
{
    public class DeployOption: ISelectableOption
    {
        private string optionName = "Deploy";
        private CardUnit unitToDeploy;

        public DeployOption(CardUnit unit)
        {
            unitToDeploy = unit;
        }

        /*
         * Raise a request for deploy mode sub phase to begin
         */
        public void ExecuteTasks()
        {
            GameEvents.instance.DeployModeRequested(unitToDeploy);
        }

        public string GetName()
        {
            return optionName;
        }
    }
}
