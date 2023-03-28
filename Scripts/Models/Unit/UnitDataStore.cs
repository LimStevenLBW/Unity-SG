using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Formation Controller and Unit Controller will receive their data from unit data store. 
 * The purpose of unitdatastore is to be a mutable version of the Unit Scriptable Object
 */
public class UnitDataStore
{
    public UnitController controller;
    private Unit unitBase;
    public string unitName;
    public string faction;
    public Rank rank;

    private int Level;
    private double exp;

    private double currentPower;
    private double Power;
    private double currentArmor;
    private double Armor;

    private double currentStamina;
    private double maxStamina;  //Only used in combat
    private double Stamina;  //Persistent Stat outside of combat

    private double currentSpeed;
    private double Speed;

    private double currentCrit;
    private double Crit;

    private double currentCritBoost;
    private double CritBoost;
    private double baseCritBoost = 2; //IE. Crit damage, but more than just damage can be effected, standard for all characters

    private int currentTroopCount;
    private int maxTroopCount; //With buffs, we can go past the normal troop count during combat
    private int TroopCount;

    //Leadership Stats, these will be within the range of 1-99. In some cases can even affect combat.
    private int leadership;
    private int engineering;
    private int magic;
    private int economy;
    private int agriculture;

    private double upkeep;

    //SKILLS
    //We create a new instance of the skill, based on the ID
    public Skill movementSkill;
    public Skill skill1;
    public Skill skill2;
    public Skill skill3;
    public Skill skill4;
 

    public UnitDataStore(UnitController controller, Unit unitBase)
    {
        this.controller = controller;
        this.unitBase = unitBase;

        //Set the Stamina
        Stamina = unitBase.baseStamina;
        maxStamina = Stamina;
        currentStamina = maxStamina;

        //Stats
        unitName = unitBase.unitName;
        faction = unitBase.faction;
        rank = unitBase.rank;
        maxTroopCount = unitBase.baseTroopCount;
        currentPower = unitBase.basePower;
        currentArmor = unitBase.baseArmor;
        currentSpeed = unitBase.baseSpeed;
        currentCrit = unitBase.baseCrit;
        //CritBoost = baseCritBoost;

        InitSkills();
        //movementSkill = unitBase.movementSkill;
        //movementCD = movementSkill.baseCooldown;
     

    }


    /*
     * Sets up a new instance of a skills
     */
    public void InitSkills() {
        movementSkill = unitBase.GetSkill(unitBase.movementSkill_ID);
        if(movementSkill != null) movementSkill.Init(this, controller);
    }

    public string GetName() { return unitName; }

    public void StartListening()
    {
        Timer.OnSecondPassed += SecondPassed;
    }

    void SecondPassed()
    {
        if (movementSkill != null) movementSkill.SecondPassed();
    }

    public bool IsMovementAvailable()
    {
        if (movementSkill == null) return false;

       //If we still have stamina
       //Calculate how much stamina we would have IF we were to do the move
       double staminaResult = currentStamina - movementSkill.currentStaminaCost;

        //If we have enough stamina and if it is off cooldown, then the move is available
        if(staminaResult >= 0 && movementSkill.currentCooldown <= 0)
        { 
            return true;
        }

        return false;

    }



    public string GetRank()
    {
        if (rank == Rank.X) return "X";
        if (rank == Rank.S) return "S";
        if (rank == Rank.A) return "A";
        if (rank == Rank.B) return "B";
        if (rank == Rank.C) return "C";
        if (rank == Rank.D) return "D";

        return "?";
    }

    public int GetLevel() { return Level; }
    public void SetLevel(int value) { Level = value; }
    public double GetExp() { return exp; }
    public void SetExp(double value) { exp = value; }

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
}
