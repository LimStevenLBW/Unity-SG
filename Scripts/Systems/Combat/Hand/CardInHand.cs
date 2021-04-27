using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Systems.Combat
{
    public class CardInHand : MonoBehaviour
    {
        public Image portrait;

        private int id;
        private bool isCursorOver;
        private bool isFocused;

        private Player owner;
        private Hand ownerHand;
        private CardUnit unit;

        // Start is called before the first frame update
        void Awake()
        {
            id = GetInstanceID();
            isCursorOver = false;
            isFocused = false;

            CombatEventBroker.instance.onUnitSelect += UnitSelectHandler;
            CombatEventBroker.instance.onUnitMouseOver += UnitMouseOverHandler;
            CombatEventBroker.instance.onUnitMouseOut += UnitMouseOutHandler;

            GameEvents.instance.onCardInHandPlayed += RemoveFocusedCard;
        }

        public void InitCard(CardUnit unit, Hand ownerHand, Player owner)
        {
            portrait.sprite = unit.portrait;
            this.owner = owner;
            this.ownerHand = ownerHand;
            this.unit = unit;
        }

        private void OnMouseDown()
        {
            CombatEventBroker.instance.UnitSelect(id);
        }

        private void OnMouseOver()
        {
            CombatEventBroker.instance.UnitMouseOver(id);

        }

        private void OnMouseExit()
        {
            CombatEventBroker.instance.UnitMouseOut(id);
        }

        /**
         * Passes data over to those subscribed 
         */
        private void UnitMouseOverHandler(int id)
        {
            if(this.id == id && !isCursorOver)
            {
                isCursorOver = true;
                CombatEventBroker.instance.UnitDataAvailable(unit);
            }
        }
        private void UnitMouseOutHandler(int id)
        {
            if (this.id == id && isCursorOver)
            {
                isCursorOver = false;
            }
        }

        /**
         * If selected, mark the card as focused and send it to the context options object
         * When not selected, unfocus the card
         */
        private void UnitSelectHandler(int id)
        {
            if(id == this.id)
            {
                ownerHand.CreateNewContext(unit, Input.mousePosition);
                isFocused = true;
            }
            else
            {
                isFocused = false;
            }

            //Debug.Log(Director.Scene.GetCurrentPhase());
            //if (subPhase != SubPhase.DEPLOYMENT && phase == Phase.PREP)
            // {                CombatEventBroker.instance.CardInHandSelected();


            //  } 


        }

        private void RemoveFocusedCard()
        {
            if (isFocused)
            {
                Destroy(this.gameObject);
            }
        }

    }
}
