using UnityEngine;
using UnityEngine.EventSystems;

namespace Buttons
{
    internal sealed class LoadButton : UIButton, IPointerClickHandler
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
            Debug.Log("LoadButton");
            //SaveableData savedata = SaveDataManager.LoadGame();
            // EventManager.TriggerDataLoaded(savedata);
        }
    }
}