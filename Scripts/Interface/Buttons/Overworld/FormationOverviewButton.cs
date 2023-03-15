using Buttons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


internal sealed class FormationOverviewButton : ToggleButton, IPointerClickHandler
{
    public GameObject FormationEditorMenu; //Open this window
    public GameObject FormationOverviewMenu; //Close this window

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            ToggleMenu();
        }
    }

    /**
     * Registered IPointerClickHandler callback
     */
    public override void OnPointerClick(PointerEventData e)
    {
        ToggleMenu();
    }
    public void ToggleMenu()
    {
        if (!FormationOverviewMenu.activeInHierarchy) // If the editor is not active, set it active and close the formation overview
        {
            PlayAudioClip(base.AudioClickOpen);
            FormationOverviewMenu.SetActive(true);
            FormationEditorMenu.SetActive(false);
        }
        else
        { //Or just close it
            PlayAudioClip(base.AudioClickClose);
            FormationOverviewMenu.SetActive(false);
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

