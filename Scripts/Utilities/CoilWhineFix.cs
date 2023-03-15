using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace Assets.Scripts.Utilities
{
    //Doesnt work in full screen Unity play mode
    public class CoilWhineFix : MonoBehaviour
    {
        #if UNITY_EDITOR
        [SerializeField] public int frameRate = 60;

        
        void Start()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = frameRate;
        }

        #endif

    }

}
