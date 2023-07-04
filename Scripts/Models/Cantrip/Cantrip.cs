using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Cantrip", menuName = "Cantrip/Cantrip")]
public class Cantrip : ScriptableObject
{
    public string cantripName;
    public string effectText;
    public int cantripCost;

    public GameObject controller;
}
