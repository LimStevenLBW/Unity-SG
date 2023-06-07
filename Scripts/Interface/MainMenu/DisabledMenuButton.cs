using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using GameSystems;
using UnityEngine.UI;

namespace Buttons
{
    internal sealed class DisabledMenuButton : Button
    {
        private Animator animator;
        private Image bg;

        void Start()
        {
            bg = GetComponent<Image>();
            bg.color = Color.grey;
            //animator = GetComponent<Animator>();
        }

        public override void OnPointerEnter(PointerEventData e)
        {
           // animator.SetBool("isHovered", true);
            //PlayAudioClip(AudioHover);
            //Debug.Log("Button Hovered");
        }
        public override void OnPointerExit(PointerEventData e)
        {
           // animator.SetBool("isHovered", false);
        }

        /**
         * Registered IPointerClickHandler callback
         */
        public override void OnPointerClick(PointerEventData e)
        {
            //Call Super to play attached audio clip
           // base.OnPointerClick(e);
            //Debug.Log("Button Click Child");

        }


    }
}
