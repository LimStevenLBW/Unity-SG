using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A template for a playable unit card
 * Last revision, 3/9/21
 */
[CreateAssetMenu(fileName = "New Unit", menuName = "Card/Unit")]
public class CardUnit : Card
{
    //Character class of the unit
    public CardUnitRole role;
    //Tags to indicate type or tribe
    public List<CardUnitTag> tagList;

    //Base Stats 
    public int baseAttack;
    public int baseSpeed;
    public int baseTroopCount;

    public int deployCost;

    public Sprite getUnitRoleIcon()
    {
        return role.getIcon();
    }

}
