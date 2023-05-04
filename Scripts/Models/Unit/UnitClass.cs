using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Defines the base definition for a unit's class
 * The class defines a set of behaviours that all units of that class share
 */
[CreateAssetMenu(fileName = "New Character Class", menuName = "Unit/Class")]
public class UnitClass : ScriptableObject
{
    public string className;
    public string description;
    public Sprite icon;

    public int movementSkill_ID;

}
