using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class PhaseController
    {
        private Phase currentPhase;
        private SubPhaseController subPhaseController;

        private Player player;
        private Player opponent;

        private BeginPhaseTask beginPhaseTask;
        private DrawPhaseTask drawPhaseTask;
        
        private int turnNumber;

        public PhaseController(Player player, Player opponent)
        {
            this.player = player;
            this.opponent = opponent;
            subPhaseController = new SubPhaseController(this);
            
            beginPhaseTask = new BeginPhaseTask(this);
            drawPhaseTask = new DrawPhaseTask(this);

            //When the next phase is requested, handle which phase to shift to 
            GameEvents.instance.onNextPhaseRequested += NextPhaseShift;

            //First player always go first for now
            player.isActive = true;

            SetCurrentPhase(Phase.BEGIN);
        }

        public Player GetCurrentPlayer()
        {
            if (player.isActive) return player;
            else if (opponent.isActive) return opponent;

            Debug.Log("ERROR, BOTH PLAYERS INACTIVE");
            return null;
            
        }

        public Phase GetCurrentPhase()
        {
            return currentPhase;
        }

        private void SetCurrentPhase(Phase phase)
        {
            currentPhase = phase;
            GameEvents.instance.PhaseStarted(currentPhase);
        }

        /*
         * Defines the main phase step order of the game, when called, shift into the appopriate phase
         */
        private void NextPhaseShift()
        {
            if (currentPhase == Phase.BEGIN)
            {
                SetCurrentPhase(Phase.DRAW);
            }
            else if(currentPhase == Phase.DRAW)
            {
                SetCurrentPhase(Phase.PREP);
            }
            else if (currentPhase == Phase.PREP && turnNumber > 1)
            {
                SetCurrentPhase(Phase.COMBAT);
            }
            else if (currentPhase == Phase.PREP && turnNumber <= 1)
            {
                SetCurrentPhase(Phase.END);
            }
            else if(currentPhase == Phase.COMBAT)
            {
                SetCurrentPhase(Phase.END);
            }
        }

    }
}
