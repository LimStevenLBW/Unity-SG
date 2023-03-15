using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Systems.Combat;

/**
 * This is the base card class, that all types of cards inherit from 
 */

public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public string flavorText;
    
    public Sprite portrait;

    //Set only during combat scenes
    private Player owner;

    public void SetOwner(Player player)
    {
        owner = player;
    }

    public Player GetOwner()
    {
        return owner;
    }
}
