using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *
 */
//[CreateAssetMenu(fileName = "New Role", menuName = "Card/Attribute/Role")]
public class CardUnitRole : ScriptableObject
{
    public string roleName;
    public Sprite icon;

    public Sprite getIcon()
    {
        return icon;
    }
}
 