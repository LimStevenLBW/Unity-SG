using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Class", menuName = "Unit/Trait")]
public class UnitTrait : ScriptableObject, IComparable<UnitTrait>
{
    public string traitName;
    public string description;
    public Sprite icon;

    public List<int> requirementTiers;
    public string effect;

    public int CompareTo(UnitTrait other)
    {
        return traitName.CompareTo(other.traitName);
    }
}
