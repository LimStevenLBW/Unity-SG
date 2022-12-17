using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Utilities
{
    public class CoilWhineFix : MonoBehaviour
    {
        #if UNITY_EDITOR
        public int frameRate = 120;

        
        void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRate;
        }

        #endif

    }

}
