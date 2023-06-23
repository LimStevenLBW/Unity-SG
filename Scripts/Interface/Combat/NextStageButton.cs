using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttons;
using UnityEngine.EventSystems;

public class NextStageButton : UIButton, IPointerClickHandler
{

    /*
     * Registered IPointerClickHandler callback
     */
    public override void OnPointerClick(PointerEventData e)
    {
        Director.Instance.TransitionIntoNextStage();
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
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public bool IsMousedOver()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResultList);
        foreach (RaycastResult r in raycastResultList) if (r.gameObject.GetComponent<NextStageButton>() != null) return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Director.Instance.TransitionIntoNextStage();
        }
    }


}
