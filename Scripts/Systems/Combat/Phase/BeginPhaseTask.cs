using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    class BeginPhaseTask 
    {
        private Player player;
        private PhaseController controller;

        public BeginPhaseTask(PhaseController controller)
        {
            this.controller = controller;
            GameEvents.instance.onPhaseStarted += StartTask;
        }

        private void StartTask(Phase phase)
        {
            player = controller.GetCurrentPlayer();
            if (phase == Phase.BEGIN)
            {
                //Resolve any queued effects on the stack
                GameEvents.instance.NextPhaseRequested();
                GameEvents.instance.ActionTotalUpdate(player.actionTotal);
            }
        }
    }
    
}
