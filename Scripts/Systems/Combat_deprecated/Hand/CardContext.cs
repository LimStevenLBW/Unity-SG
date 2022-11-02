using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    /*
     * The card context can contain a number of selectable options depending on
     * 
     */
    public class CardContext : MonoBehaviour
    {
        public Camera cam;
        public ContextButton button;

        private List<ContextButton> optionList;

        void Start()
        {
            optionList = new List<ContextButton>();
            //CombatEventBroker.instance.onCardInHandSelected += DisplayOptionPrompt;
            CombatEventBroker.instance.onResetWindows += ClearOptionPrompt;
        }

        /**
         * 
         */
        public void CreateNewContext(CardUnit unit, Vector3 pos)
        {
            ClearOptionPrompt();

            ContextButton newButton = Instantiate(button, transform.position, transform.rotation, transform);
            newButton.SetOption(new DeployOption(unit));
            //optionList.Add(newButton);

            pos.z = 0;
            GetComponent<RectTransform>().localPosition = pos;
        }

        /**
         * Function is dependent on the panel area parent having bottom left pivot and position for local position to correctly calculate
         */
        private void DisplayOptionPrompt(CardUnit unit, Vector3 pos)
        {
            /*
            ClearOptionPrompt();

            ContextButton newButton = Instantiate(button, transform.position, transform.rotation, transform);
            newButton.SetOption(new DeployOption(unit));
            optionList.Add(newButton);

            pos.z = 0;
            GetComponent<RectTransform>().localPosition = pos;
            */
        }

        private void ClearOptionPrompt()
        {
            optionList.Clear();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }


    }
}
