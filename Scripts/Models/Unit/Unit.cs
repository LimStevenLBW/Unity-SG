using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Defines the base definitions for a unit
 */
[CreateAssetMenu(fileName = "New Unit Character", menuName = "Unit/Unit")]
public class Unit : ScriptableObject, Card
{
    public UnitController model;
    public UnitClass unitClass;
    public UnitTrait faction;
    public UnitTrait special;

    //Identity
    public string unitName = "";
    public string bio;
    public Rank rank = Rank.C;

    //not yet implemented
    //public string dialogue; 
    //public string voice;
    //public Sprite portrait;

    //Level
    public int baseLevel = 1;
    public float expGrowthRate = 1;

    //Combat Stats and Modifiers
    public float basePower = 1;
    public float powerGrowthRate = 1;
    public float baseDefense = 1;
    public float defenseGrowthRate = 1;
    public float baseMagic = 0;
    public float magicGrowthRate = 0;

    //Stamina is spent to use skills
    public float baseStamina = 1;
    public float staminaGrowthRate = 1;

    //Speed affects cooldown reduction of skills
    public float baseSpeed = 1;
    public float speedGrowthRate = 1;

    public float baseCrit = 0;

    //Troops
    public int baseTroopCount = 10;

    //Leadership Stats, these will be within the range of 1-99. In some cases can even affect combat.
    public int leadership = 1;
    public int engineering = 1;
    public int magic = 1;
    public int economy = 1;
    public int agriculture = 1;

    public float upkeep = 0.1f;

    //Skills

    public int skill1_ID;
    public int skill2_ID;
    public int skill3_ID;
    public int skill4_ID;

    //Status Effects and Skills list
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    private List<Skill> skills = new List<Skill>();

    private UnitManager manager;
    private UnitController controller;

    //----------------------------------------------------------------

    public List<StatusEffect> GetStatusEffects() { return statusEffects;  }
    public List<Skill> GetSkills() { return skills; }

    public UnitClass GetClass() { return unitClass; }

} 
