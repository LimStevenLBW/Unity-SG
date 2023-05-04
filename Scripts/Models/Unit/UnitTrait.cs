using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Class", menuName = "Unit/Trait")]
public class UnitTrait : ScriptableObject
{
    public string traitName;
    public string description;
    public Sprite icon;

}
