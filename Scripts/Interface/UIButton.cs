using UnityEngine.EventSystems;
using UnityEngine;


/**
    *  Generic behavior for clickable buttons
    */
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] internal AudioSource AudioPlayer;
    [SerializeField] internal AudioClip AudioHover;
    [SerializeField] internal AudioClip AudioClickOpen;
    [SerializeField] internal AudioClip AudioClickClose;

    public virtual void PlayAudioClip(AudioClip clip)
    { 
        AudioPlayer.PlayOneShot(clip);
    }

    /**
        * Registered IPointerEnterHandler callback
        */
    public virtual void OnPointerEnter(PointerEventData e)
    {
        //PlayAudioClip(AudioHover);
        //Debug.Log("Button Hovered");
    }

    /**
        * Registered IPointerClickHandler callback
        */
    public virtual void OnPointerClick(PointerEventData e)
    {
        PlayAudioClip(AudioClickOpen);
        //Debug.Log("Button Clicked");
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
           
    }
}
