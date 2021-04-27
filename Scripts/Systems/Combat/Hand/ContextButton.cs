using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class ContextButton : MonoBehaviour
    {
        public TMP_Text optionText;
        private ISelectableOption option;

        public void SetOption(ISelectableOption option)
        {
            this.option = option;
            optionText.SetText(this.option.GetName());
        }

        public void Init()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseDown()
        {
            option.ExecuteTasks();
            CombatEventBroker.instance.ResetWindows();
        }
    }
}