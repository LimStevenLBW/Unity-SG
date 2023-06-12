using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttons;
using UnityEngine.EventSystems;

public class StartDeploymentButton : UIButton, IPointerClickHandler, IPointerEnterHandler
{

    /*
     * Registered IPointerClickHandler callback
     */
    public override void OnPointerClick(PointerEventData e)
    {

        Director.Instance.SetPhase("DEPLOYMENT");
        Hide();
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

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Director.Instance.SetPhase("DEPLOYMENT");
            Hide();
        }
    }
}
