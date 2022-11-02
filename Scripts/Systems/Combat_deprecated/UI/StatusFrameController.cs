using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class StatusFrameController : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text troopCountText;
        public TMP_Text statsText;
        public TMP_Text descriptionText;

        // Start is called before the first frame update
        void Start()
        {
            transform.localScale = new Vector3(0, 0, 0);
            CombatEventBroker.instance.onUnitMouseOver += UnitMouseOverHandler;
            CombatEventBroker.instance.onUnitMouseOut += UnitMouseOutHandler;
            CombatEventBroker.instance.onUnitDataAvailable += UnitDataAvailableHandler;
        }

        private void UnitMouseOverHandler(int id)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        private void UnitMouseOutHandler(int id)
        {
            transform.localScale = new Vector3(0, 0, 0);
        }

        private void UnitDataAvailableHandler(CardUnit unit)
        {
           nameText.SetText(unit.cardName);
           troopCountText.SetText("Troops: " + unit.baseTroopCount.ToString());
           statsText.SetText("Attack: " + unit.baseAttack.ToString() + ", Speed: " + unit.baseSpeed.ToString() + ", Cost: " + unit.deployCost);
           descriptionText.SetText(unit.description);
        }
    }
}