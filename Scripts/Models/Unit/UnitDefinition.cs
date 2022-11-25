using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Defines the base for a unit
 */
[CreateAssetMenu(fileName = "New Unit", menuName = "Unit")]
public class UnitDefinition : ScriptableObject
{
    //Descriptive
    public string unitName;
    public string bio;
    public Sprite portrait;

    /*Stats tied to Level*/
    public int level;
    public int levelUpThreshold;
    public int exp;
    
    public float baseHealth;
    public float basePower;

    public float healthGrowthRate;
    public float powerGrowthRate;

    /*Stats that do not grow*/
    public float armorRating;
    public float speedRating;
    
    /*Combat Modifiers*/
    public float currentAction;
    public float actionThreshold; //Default 100
    public float critChance;

    /*Out of Combat Stats*/
    //public float diplomacy;


    //Skills
    public AttackSkill attackSkill;

    public Skill skill_1;
    public Skill skill_2;
    public Skill skill_3;

    public BurstSkill burstSkill;

    //public string classes
    //public string abilities
    //public string star rating
} 
