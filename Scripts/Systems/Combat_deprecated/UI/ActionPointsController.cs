using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Systems.Combat.UI
{
    /*
     * Controls the display for current player's action points.
     */
    public class ActionPointsController: MonoBehaviour
    {
        [SerializeField] private Image actionPoint;
        
        void Awake()
        {
            GameEvents.instance.onActionTotalUpdate += SetActionTotalCounter;
        }

        /**
         * 
         */
        void SetActionTotalCounter(int amount)
        {
            foreach (Transform child in transform)
            {
                //Clear the current list of points
                Destroy(child.gameObject);
            }

            for (int i = 0; i < amount; i++)
            {
                //Prefab was saved active, so the object will instantiate active as well
                Instantiate(actionPoint, transform.position, transform.rotation, transform);
            }
        }
    }
}
