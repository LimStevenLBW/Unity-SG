using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GameSystems;

namespace Buttons
{
    internal sealed class EndTurnButton : UIButton, IPointerClickHandler
    {
        /**
         * Registered IPointerClickHandler callback
         */
        new public void OnPointerClick(PointerEventData e)
        {
            //Call Super to play attached audio clip
            base.OnPointerClick(e);
            Debug.Log("Button Click Child");

            //Raise End Turn Increment Event
            EventBroker.InvokeIncrementTurn();

        }


    }
}
