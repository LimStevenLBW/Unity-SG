using Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


internal sealed class FormationEditButton : ToggleButton, IPointerClickHandler
{
        public GameObject FormationEditorMenu;
        public GameObject FormationOverviewMenu; //Close this window

        /**
         * Registered IPointerClickHandler callback
         */
        public override void OnPointerClick(PointerEventData e)
        {
            if (!FormationEditorMenu.activeInHierarchy) // If the editor is not active, set it active and close the formation overview
            {
                PlayAudioClip(base.AudioClickOpen);
                FormationEditorMenu.SetActive(true);
                FormationOverviewMenu.SetActive(false);
        }
            else
            { //Or just close it
                PlayAudioClip(base.AudioClickClose);
                FormationEditorMenu.SetActive(false);
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

