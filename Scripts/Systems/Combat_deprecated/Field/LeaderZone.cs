using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Systems.Combat
{
    public class LeaderZone : MonoBehaviour, IFieldZone
    {
        private CardLeader zoneData;
        public Image portrait;
        public Image classIcon;
        public TMP_Text troopCount;

        public void SetZoneData(Card cardData)
        {
            zoneData = (CardLeader)cardData;
            //troopCount.text = zoneData.baseTroopCount.ToString();
            portrait.sprite = zoneData.portrait;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}