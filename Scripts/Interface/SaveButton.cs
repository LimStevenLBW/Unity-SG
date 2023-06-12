using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Buttons
{
    internal sealed class SaveButton : UIButton, IPointerClickHandler
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * Registered IPointerClickHandler callback
         */
        public override void OnPointerClick(PointerEventData e)
        {
            //Call Super to play attached audio clip
            base.OnPointerClick(e);
            Debug.Log("SaveButton");
            // SaveManager.SaveGame();
        }
    }
}
