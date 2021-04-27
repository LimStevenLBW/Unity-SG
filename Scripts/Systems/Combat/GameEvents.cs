using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class GameEvents: MonoBehaviour
    {
        public static GameEvents instance = null;

        public GameEvents()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                //Fatal Error
            }
        }

        public event Action onNextPhaseRequested;
        public event Action onCardInHandPlayed;

        public event Action<CardUnit> onDeployModeRequested;
        public event Action<CardUnit> onUnitDeployed;

        public event Action<int> onTurnPlayerDraw;
        public event Action<int, int> onLifeTotalUpdate;
        public event Action<int> onActionTotalUpdate;
        public event Action<string> onPlayerControlsUpdate;

        public event Action<Phase> onPhaseStarted;
        public event Action<SubPhase> onSubPhaseStarted;

        public void UnitDeployed(CardUnit unit)
        {
            onUnitDeployed?.Invoke(unit);
        }
        public void CardInHandPlayed(){
            onCardInHandPlayed?.Invoke();
        }

        public void DeployModeRequested(CardUnit unit)
        {
            onDeployModeRequested?.Invoke(unit);
        }

        public void PhaseControlsUpdate(string playerStatus)
        {
            onPlayerControlsUpdate?.Invoke(playerStatus);
        }

        public void NextPhaseRequested() 
        {
            onNextPhaseRequested?.Invoke();
        }
        public void PhaseStarted(Phase phase)
        {
            onPhaseStarted?.Invoke(phase);
        }
        public void SubPhaseStarted(SubPhase phase)
        {
            onSubPhaseStarted?.Invoke(phase);
        }
        public void TurnPlayerDraw(int amount)
        {
            onTurnPlayerDraw?.Invoke(amount);
        }

        public void LifeTotalUpdate(int amount, int playerId)
        {
            onLifeTotalUpdate(amount, playerId);
        }
        public void ActionTotalUpdate(int amount)
        {
            onActionTotalUpdate(amount);
        }
    }
}
