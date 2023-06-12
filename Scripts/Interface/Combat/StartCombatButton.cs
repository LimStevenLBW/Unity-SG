using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Buttons;
using UnityEngine.EventSystems;

public class StartCombatButton : UIButton, IPointerClickHandler
{
    private bool ready;
    /*
     * Registered IPointerClickHandler callback
     */
    public override void OnPointerClick(PointerEventData e)
    {
        Director.Instance.SetPhase("COMBAT");
        gameObject.SetActive(false);
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
        if(ready) gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetReadyStatus(bool status)
    {
        ready = status;
    }

    public bool IsMousedOver()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> raycastResultList = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResultList);
        foreach (RaycastResult r in raycastResultList) if (r.gameObject.GetComponent<StartCombatButton>() != null) return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Director.Instance.SetPhase("COMBAT");
            Hide();
        }
    }


}
