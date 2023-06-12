using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Buttons
{
    internal class ToggleButton : UIButton, IPointerClickHandler
    {
        public GameObject AttachedObject; 
        
        /**
         * Registered IPointerClickHandler callback
         */
        public override void OnPointerClick(PointerEventData e)
        {
            if (!AttachedObject.activeInHierarchy)
            {
                PlayAudioClip(base.AudioClickOpen);
                AttachedObject.SetActive(true);
            }
            else
            {
                PlayAudioClip(base.AudioClickClose);
                AttachedObject.SetActive(false);
            }
           

        }

        public override void OnPointerEnter(PointerEventData e)
        {
            PlayAudioClip(base.AudioHover);
        }

        public override void PlayAudioClip(AudioClip clip)
        {
            AudioPlayer.clip = clip;
            AudioPlayer.Play();
        }
    }
}