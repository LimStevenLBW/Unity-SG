using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Defines the base definition for a unit's class
 * The class defines a set of behaviours that all units of that class share
 */
[CreateAssetMenu(fileName = "New Character Class", menuName = "Unit/Class")]
public class UnitClass : UnitTrait
{
    public int movementSkill_ID;

    //41-50 is front row
    //71-80 is back row, 80 is the final hex, reserved for captain

    //The highest value for frontSpawn MUST be lower than the rearSpawn minimum
    public int[] frontSpawnPref = new int[2];
    public int[] rearSpawnPref = new int[2];

}
