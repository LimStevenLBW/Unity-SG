using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Systems.Combat.UI
{
    class PhaseLabel : MonoBehaviour
    {
        public TMP_Text label;

        void Start()
        {
            GameEvents.instance.onPhaseStarted += UpdateLabelText;
        }

        private void UpdateLabelText(Phase phase)
        {
            this.label.SetText(phase.ToString());
        }
    }
}
