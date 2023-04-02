using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Defines the base definitions for a unit
 */
[CreateAssetMenu(fileName = "New Unit Character", menuName = "Character/Unit")]
public class Unit : ScriptableObject
{
    public string faction;

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
    public double expGrowthRate = 1;

    //Combat Stats and Modifiers
    public double basePower = 1;
    public double powerGrowthRate = 1;
    public double baseDefense = 1;
    public double defenseGrowthRate = 1;
    public double baseMagic = 0;
    public double magicGrowthRate = 0;

    //Stamina is spent to use skills
    public double baseStamina = 1;
    public double staminaGrowthRate = 1;

    //Speed affects cooldown reduction of skills
    public double baseSpeed = 1;
    public double speedGrowthRate = 1;

    public double baseCrit = 0;

    //Troops
    public int baseTroopCount = 10;

    //Leadership Stats, these will be within the range of 1-99. In some cases can even affect combat.
    public int leadership = 1;
    public int engineering = 1;
    public int magic = 1;
    public int economy = 1;
    public int agriculture = 1;

    public double upkeep = 0.1;

    //Skills
    public UnitClass unitClass;

    public int movementSkill_ID;
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


    /*
     * We can determine the IDs for Skills here
     */
    public Skill GetSkill(int ID)
    {
        switch (ID)
        {
            case 0: return new MarchSkill();
            case 2: return new EngageSkill();
        }
        return null;
    }
} 
