using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttons;
using UnityEngine.EventSystems;

public class StartCombatButton : Button, IPointerClickHandler, IPointerEnterHandler
{

    /*
     * Registered IPointerClickHandler callback
     */
    public override void OnPointerClick(PointerEventData e)
    {

        Director.Instance.SetPhase("COMBAT");
        gameObject.SetActive(false);
    }
    public override void OnPointerEnter(PointerEventData pointerEventData)
    {
        //AudioPlayer.PlayOneShot(AudioHover);
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
