using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Defines the base definition for a unit
 */
[CreateAssetMenu(fileName = "New Unit Character", menuName = "Character/Unit")]
public class Unit : ScriptableObject
{
    public enum Rank
    {
        X,
        S,
        A,
        B,
        C,
        D
    }

    //Identity
    public string unitName = "test";
    public string bio;
    public Rank rank = Rank.C;

    //not yet implemented
    //public string dialogue; 
    //public string voice;
    //public Sprite portrait;

    //Level
    public int baseLevel = 1;
    private int currentLevel;
    private double exp;
    public double expGrowthRate = 1;

    //Combat Stats and Modifiers
    public double basePower = 1;
    private double currentPower;
    private double Power; 
    public double powerGrowthRate = 1;

    public double baseArmor = 1;
    private double currentArmor;
    private double Armor;
    public double armorGrowthRate = 1;

    //Stamina is spent to use skills
    public double baseStamina = 1;
    private double currentStamina;
    private double maxStamina;  //Only used in combat
    private double Stamina;  //Persistent Stat outside of combat
    public double staminaGrowthRate = 1;

    //Speed affects cooldown reduction of skills
    public double baseSpeed = 1;
    private double currentSpeed;
    private double Speed;
    public double speedGrowthRate = 1;

    public double baseCrit = 0;
    private double currentCrit;
    private double Crit;

    private double baseCritBoost = 2; //IE. Crit damage, but more than just damage can be effected, standard for all characters
    private double currentCritBoost;
    private double CritBoost;

    //Troops
    public int baseTroopCount = 10;
    private int currentTroopCount;
    private int maxTroopCount;
    private int TroopCount;

    //Leadership Stats, these will be within the range of 1-99. In some cases can even affect combat.
    public int leadership = 1;
    public int engineering = 1;
    public int magic = 1;
    public int economy = 1;
    public int agriculture = 1;

    public double upkeep = 0.1;

    //Skills
    public UnitClass unitClass;

    public Skill uniqueSkill;
    public Skill skill1;
    public Skill skill2;
    public Skill skill3;
    public Skill skill4;

    //Status Effects
    private List<StatusEffect> statusEffects = new List<StatusEffect>();

    public int GetLevel() { return currentLevel; }
    public void SetLevel(int value) { currentLevel = value; }
    public double GetExp() { return exp; }
    public void SetExp(double value) { exp = value; }
    //----------------------------------------------------------------
    public double GetCurrentPower() { return currentPower; }
    public void SetCurrentPower(double value) { currentPower = value; }
    public double GetPower() { return Power; }
    public void SetPower(double value) { Power = value; }

    public double GetCurrentArmor() { return currentArmor; }
    public void SetCurrentArmor(double value) { currentArmor = value; }
    public double GetArmor() { return Armor; }
    public void SetArmor(double value) { Armor = value; }

    public double GetCurrentStamina() { return currentStamina; }
    public void SetCurrentStamina(double value) { currentStamina = value; }
    public double GetMaxStamina() { return maxStamina; }
    public void SetMaxStamina(double value) { maxStamina = value; }
    public double GetStamina() { return Stamina; }
    public void SetStamina(double value) { Stamina = value; }

    public double GetCurrentSpeed() { return currentSpeed; }
    public void SetCurrentSpeed(double value) { currentSpeed = value; }
    public double GetSpeed() { return Speed; }
    public void SetSpeed(double value) { Speed = value; }

    public double GetCurrentCrit() { return currentCrit; }
    public void SetCurrentCrit(double value) { currentCrit = value; }
    public double GetCrit() { return Crit; }
    public void SetCrit(double value) { Crit = value; }

    private double GetBaseCritboost() { return baseCritBoost;  }
    public double GetCurrentCritBoost() { return currentCritBoost; }
    public void SetCurrentCritBoost(double value) { currentCritBoost = value; }
    public double GetCritBoost() { return CritBoost; }
    public void SetCritBoost(double value) { CritBoost = value; }

    public int GetCurrentTroopCount() { return currentTroopCount; }
    public void SetCurrentTroopCount(int value) { currentTroopCount = value; }
    public int GetMaxTroopCount() { return maxTroopCount; }
    public void SetMaxTroopCount(int value) { maxTroopCount = value; }
    public int GetTroopCount() { return TroopCount; }
    public void SetTroopCount(int value) { TroopCount = value; }

    public List<StatusEffect> GetStatusEffects() { return statusEffects;  }


} 
