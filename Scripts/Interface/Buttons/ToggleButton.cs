using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Buttons
{
    internal sealed class ToggleButton : Button, IPointerClickHandler
    {
        public GameObject AttachedObject;

        /**
         * Registered IPointerClickHandler callback
         */
        public override void OnPointerClick(PointerEventData e)
        {
            //Call Super to play attached audio clip
            base.OnPointerClick(e);
            ToggleVisibility(AttachedObject);

        }
        public void ToggleVisibility(GameObject AttachedObject)
        {
            if (AttachedObject.activeInHierarchy)
            {
                AttachedObject.SetActive(false);
            }
            else
            {
                AttachedObject.SetActive(true);
            }

        }
    }
}