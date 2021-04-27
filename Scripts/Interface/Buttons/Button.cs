using UnityEngine.EventSystems;
using UnityEngine;

namespace Buttons
{
    /**
     *  Generic behavior for clickable buttons
     */
   internal class Button : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        [SerializeField] private AudioSource AudioPlayer;
        [SerializeField] private AudioClip AudioHover;
        [SerializeField] private AudioClip AudioClick;

        void PlayAudioClip(AudioClip clip)
        {
            AudioPlayer.clip = clip;
            AudioPlayer.Play();
        }

        /**
         * Registered IPointerEnterHandler callback
         */
        public void OnPointerEnter(PointerEventData e)
        {
            PlayAudioClip(AudioHover);
            Debug.Log("Button Hovered");
        }

        /**
         * Registered IPointerClickHandler callback
         */
        public virtual void OnPointerClick(PointerEventData e)
        {
            PlayAudioClip(AudioClick);
            Debug.Log("Button Clicked");
        }


    }
}
