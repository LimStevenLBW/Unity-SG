using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Systems.Combat
{
    public class PanelColorController : MonoBehaviour
    {
        private Image imgComponent;

        // Start is called before the first frame update
        void Start()
        {
            imgComponent = GetComponent<Image>();
            imgComponent.color = new Color(255, 255, 255, 0);
            CombatEventBroker.instance.onUnitMouseOver += UnitMouseOverHandler;
            CombatEventBroker.instance.onUnitMouseOut += UnitMouseOutHandler;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void UnitMouseOverHandler(int id)
        {
            //Color component only takes RGB as parameter, convert hex to to RGB beforehand
            imgComponent.color = new Color(0, 0, 0, 0.5f);
        
        }

        private void UnitMouseOutHandler(int id)
        {
            imgComponent.color = new Color(255, 255, 255, 0);
        }
    }
}
