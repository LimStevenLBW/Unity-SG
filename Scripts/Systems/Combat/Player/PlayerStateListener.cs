using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class PlayerStateListener
    {
        private int ownerId;
        private int actionTotal;
        private int lifeTotal;

        public PlayerStateListener(int ownerId, int actionTotal, int lifeTotal)
        {
            this.ownerId = ownerId;
            this.actionTotal = actionTotal;
            this.lifeTotal = lifeTotal;
            InitListeners();
        }

        private void InitListeners()
        {
            GameEvents.instance.onLifeTotalUpdate += SetLifeTotal;
            GameEvents.instance.onActionTotalUpdate += SetActionTotal;
        }

        private void SetLifeTotal(int amount, int ownerId)
        {
            if (this.ownerId == ownerId)
            {
                lifeTotal = amount;
            }
        }

        private void SetActionTotal(int amount)
        { 
        
        }

        public int GetActionTotal()
        {
            return actionTotal;
        }

        public int GetLifeTotal()
        {
            return lifeTotal;
        }

        public int GetId()
        {
            return ownerId;
        }

    }
}

