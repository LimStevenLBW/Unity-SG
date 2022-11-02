using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    class SubPhaseController
    {
        private PhaseController parentController;
        private SubPhase currentSubPhase;

        public SubPhaseController(PhaseController mainPhaseController)
        {
            parentController = mainPhaseController;
            SetSubPhase(SubPhase.NEUTRAL);
            GameEvents.instance.onDeployModeRequested += EnableDeployMode;
            GameEvents.instance.onUnitDeployed += DisableDeployMode;
        }

        private void EnableDeployMode(CardUnit unit)
        {
            if(parentController.GetCurrentPhase() == Phase.PREP)
            {
                SetSubPhase(SubPhase.DEPLOYMENT);
            }
            
        }
        private void DisableDeployMode(CardUnit unit)
        {
            SetSubPhase(SubPhase.NEUTRAL);
        }

        private void SetSubPhase(SubPhase nextSubPhase)
        {
            currentSubPhase = nextSubPhase;
            GameEvents.instance.SubPhaseStarted(nextSubPhase);
        }

        private SubPhase GetSubPhase()
        {
            return currentSubPhase;
        }
    }
}
