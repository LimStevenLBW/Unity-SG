using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttons;
using UnityEngine.EventSystems;

public class StartDeploymentButton : Button, IPointerClickHandler
{

    /*
     * Registered IPointerClickHandler callback
     */
    public override void OnPointerClick(PointerEventData e)
    {

        Director.Instance.SetPhase("DEPLOYMENT");
    }

    public override void PlayAudioClip(AudioClip clip)
    {
        AudioPlayer.clip = clip;
        AudioPlayer.Play();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
