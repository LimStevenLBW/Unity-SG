using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Systems.Combat
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private UnitZone UnitLF;
        [SerializeField] private UnitZone UnitCF;
        [SerializeField] private UnitZone UnitRF;
        [SerializeField] private UnitZone UnitLB;
        [SerializeField] private UnitZone UnitRB;

        [SerializeField] private LeaderZone ULeader;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void DeployLeader(CardLeader leader) {
            ULeader.SetZoneData(leader);
        }
    }
}