using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    class DrawPhaseTask
    {
        public DrawPhaseTask(PhaseController phaseController)
        {
            GameEvents.instance.onPhaseStarted += StartTask;
        }

        private void StartTask(Phase phase)
        { 
            if (phase == Phase.DRAW)
            {
                GameEvents.instance.TurnPlayerDraw(1);
                GameEvents.instance.NextPhaseRequested();
                //GameEvents.instance.ActionTotalUpdate();
            }
        }
    }

   
}
