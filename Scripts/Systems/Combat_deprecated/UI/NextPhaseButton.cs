using Assets.Scripts.Systems.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class NextPhaseButton : MonoBehaviour
    {

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            GameEvents.instance.NextPhaseRequested(); 
        }
    }
}