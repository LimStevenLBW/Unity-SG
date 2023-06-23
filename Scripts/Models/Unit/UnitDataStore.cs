using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Formation Controller and Unit Controller will receive their data from unit data store. 
 * The purpose of unitdatastore is to be a mutable version of the Unit Scriptable Object
 */
public class UnitDataStore: IComparable<UnitDataStore>
{
    public UnitController prefab;
    public UnitController controller;

    public Unit unitBase;
    public string unitName;

    public UnitClass unitClass;
    public UnitTrait faction;
    public UnitTrait special;

    public Rank rank;

    private int Level;
    private float exp;

    private float currentPower;
    private float Power;
    private float currentMagic;
    private float Magic;
    private float currentDefense;
    private float Defense;

    private float currentStamina;
    private float maxStamina;  //Only used in combat
    private float Stamina;  //Persistent Stat outside of combat

    private float currentSpeed;
    private float Speed;

    private float currentCrit;
    private float Crit;

    private float currentCritBoost;
    private float CritBoost;
    private float baseCritBoost = 2; //IE. Crit damage, but more than just damage can be effected, standard for all characters

    private int baseTroopCount;
    private int currentTroopCount;
    private int maxTroopCount; //With buffs, we can go past the normal troop count during combat
    private int TroopCount;

    //Leadership Stats, these will be within the range of 1-99. In some cases can even affect combat.
    private int leadership;
    private int engineering;
    private int magic;
    private int economy;
    private int agriculture;

    private float baseDefReduction = 0.25f;
    private float upkeep;

    //SKILLS
    //We create a new instance of the skill, based on the ID
    public Skill movementSkill;
    public Skill skill1;
    public Skill skill2;
    public Skill skill3;
    public Skill skill4;

    public Action<UnitDataStore> OnBarUpdated;
    public UnitDataStore(Unit unitBase)
    {
        this.unitBase = unitBase;
        prefab = unitBase.model;

        unitClass = unitBase.unitClass;
        faction = unitBase.faction;
        special = unitBase.special;
        //controller.data = this; //Set controller data reference

        //Stats
        unitName = unitBase.unitName;
        rank = unitBase.rank;

        baseTroopCount = unitBase.baseTroopCount;
        maxTroopCount = unitBase.baseTroopCount;
        currentTroopCount = maxTroopCount;

        maxStamina = unitBase.baseStamina;
        currentStamina = maxStamina;

        currentPower = unitBase.basePower;
        currentMagic = unitBase.baseMagic;
        currentDefense = unitBase.baseDefense;
        currentSpeed = unitBase.baseSpeed;
        currentCrit = unitBase.baseCrit;
        //CritBoost = baseCritBoost;

        //InitSkills();
        //movementSkill = unitBase.movementSkill;
        //movementCD = movementSkill.baseCooldown;

    }

    //Making a copy
    public UnitDataStore(UnitDataStore data)
    {
        //Set the Stamina
        Stamina = data.Stamina;
        maxStamina = Stamina;
        currentStamina = maxStamina;

        //Stats
        unitName = data.unitName;
        faction = data.faction;
        rank = data.rank;

        maxTroopCount = data.maxTroopCount;
        currentTroopCount = maxTroopCount;

        maxStamina = data.Stamina;
        currentStamina = maxStamina;

        currentPower = data.currentPower;
        currentMagic = data.currentMagic;
        currentDefense = data.currentDefense;
        currentSpeed = data.currentSpeed;
        currentCrit = data.currentCrit;
    }
    public void FindSkills()
    {
        skill1 = FindSkill(unitBase.skill1_ID);
        skill2 = FindSkill(unitBase.skill2_ID);
        skill3 = FindSkill(unitBase.skill3_ID);
        skill4 = FindSkill(unitBase.skill4_ID);        
        //The Movement Skill is preset with the unit's class
        movementSkill = FindSkill(unitBase.GetClass().movementSkill_ID);
    }

    /*
     * Setup the skill instances
     */
    public void InitSkills() {
        if (skill1 != null) { 
            skill1.Init(this, controller);
            controller.maxRange = skill1.maxRange;
        }
        if (skill2 != null) { 
            skill2.Init(this, controller);
            if (skill2.maxRange > controller.maxRange) controller.maxRange = skill2.maxRange;
        }
        if (skill3 != null) { 
            skill3.Init(this, controller);
            if (skill3.maxRange > controller.maxRange) controller.maxRange = skill3.maxRange;
        }

        if (skill4 != null) {
            skill4.Init(this, controller);
            if (skill4.maxRange > controller.maxRange) controller.maxRange = skill4.maxRange;
        }

        if (movementSkill != null) movementSkill.Init(this, controller);

    }

    /*
     * We can determine the IDs for Skills here
     */
    public Skill FindSkill(int ID)
    {
        switch (ID)
        {
            case 1: return new MovementAdvanceSkill();
            case 2: return new MovementSlowWalkSkill();
            case 3: return new MoveIntoRangeSkill();
            case 4: return new MovementEvasiveSkill();
            case 100: return new ClashSkill();
            case 101: return new FistsOfFurySkill();
            case 102: return new VolleySkill();
            case 103: return new RapidFireVolleySkill();
            case 104: return new ThrowingDaggersSkill();
            case 200: return new SelfRecoverySkill();
            case 201: return new SingleRecoverySkill();
            case 202: return new WideRecoverySkill();
            case 203: return new DivineRecoverySkill();
            case 300: return new ExplosionSkill();
            case 301: return new ElectroBoltSkill();
            case 302: return new HolyBoltSkill();
            case 400: return new ShieldWallSkill();
            case 401: return new PumpedUpSkill();
        }
        return null;
    }

    public string GetName() { return unitName; }

    public bool IsInjured()
    {
        if (currentTroopCount < maxTroopCount) return true;
        else return false;
    }

    public void StartListening()
    {
        Timer.OnSecondPassed += SecondPassed;
    }

    public void StopListening()
    {
        Timer.OnSecondPassed -= SecondPassed;
    }

    public void ResetCooldowns()
    {

        if (skill1 != null) skill1.ResetCD();
        if (skill2 != null) skill2.ResetCD();
        if (skill3 != null) skill3.ResetCD();
        if (skill4 != null) skill4.ResetCD();
        if (movementSkill != null) movementSkill.ResetCD();
    }

    void SecondPassed()
    {
        if (skill1 != null) skill1.SecondPassed();
        if (skill2 != null) skill2.SecondPassed();
        if (skill3 != null) skill3.SecondPassed();
        if (skill4 != null) skill4.SecondPassed();
        if (movementSkill != null) movementSkill.SecondPassed(); 
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
    public float GetExp() { return exp; }
    public void SetExp(float value) { exp = value; }

    public float GetCurrentPower() { return currentPower; }
    public void SetCurrentPower(float value) {
        if (value < 0) value = 0;
        currentPower = value;
    }
    public float GetPower() { return Power; }
    public void SetPower(float value) { Power = value; }

    public float GetCurrentMagic() { return currentMagic; }
    public void SetCurrentMagic(float value) { currentMagic = value; }
    public float GetMagic() { return Magic; }
    public void SetMagic(float value) { Magic = value; }

    public float GetCurrentDefense() { return currentDefense; }
    public void SetCurrentDefense(float value) { currentDefense = value; }
    public float GetDefense() { return Defense; }
    public void SetDefense(float value) { Defense = value; }

    public float GetCurrentSpeed() { return currentSpeed; }
    public void SetCurrentSpeed(float value) { currentSpeed = value; }
    public float GetSpeed() { return Speed; }
    public void SetSpeed(float value) { Speed = value; }

    public float GetCurrentCrit() { return currentCrit; }
    public void SetCurrentCrit(float value) { currentCrit = value; }
    public float GetCrit() { return Crit; }
    public void SetCrit(float value) { Crit = value; }

    public float GetCurrentCritBoost() { return currentCritBoost; }
    public void SetCurrentCritBoost(float value) { currentCritBoost = value; }
    public float GetCritBoost() { return CritBoost; }
    public void SetCritBoost(float value) { CritBoost = value; }

    public int GetBaseTroopCount() { return baseTroopCount; }

    //TROOP COUNT
    public int GetCurrentTroopCount() { return currentTroopCount; }
    public void SetCurrentTroopCount(int value) {
        if(value > maxTroopCount)
        {
            currentTroopCount = maxTroopCount; // Cannot go past max
        }
        else
        {
            currentTroopCount = value;
            
        }

        OnBarUpdated?.Invoke(this);

        if (value <= 0) controller.SetState("DEAD");
    }
    public int GetMaxTroopCount() { return maxTroopCount; }
    public void SetMaxTroopCount(int value) { maxTroopCount = value; OnBarUpdated?.Invoke(this); }
    public int GetTroopCount() { return TroopCount; }
    public void SetTroopCount(int value) { TroopCount = value; }

    //STAMINA
    public float GetCurrentStamina() { return currentStamina; }
    public void SetCurrentStamina(float value)
    {
        if (value > maxStamina)
        {
            currentStamina = maxStamina; // Cannot go past max
        }
        else
        {
            currentStamina = value;
        }

        OnBarUpdated?.Invoke(this);
    }
    public float GetMaxStamina() { return maxStamina; }
    public void SetMaxStamina(float value) { maxStamina = value; OnBarUpdated?.Invoke(this); }
    public float GetStamina() { return Stamina; }
    public void SetStamina(float value) { Stamina = value; }

    public float GetBaseDefReduction()
    {
        return baseDefReduction;
    }

    //Compare based on Rank and then classType
    public int CompareTo(UnitDataStore other)
    {
        /*
         * The method returns 0 if the data is equal to the other data. A value less than 0 is returned if the data is 
         * less than the other data (less characters) 
         * and a value greater than 0 if the data is greater than the other data (more characters).
         */

        int rankCompare = other.GetRank().CompareTo(GetRank());
        int classCompare = unitClass.traitName.CompareTo(other.unitClass.traitName);
        int nameCompare = unitName.CompareTo(other.unitName);

        if (rankCompare == 0 && classCompare == 0 && nameCompare == 0) return 0; //Completely equal
        if (rankCompare == 0 && classCompare == 0 && nameCompare > 0) return 1;
        if (rankCompare == 0 && classCompare == 0 && nameCompare < 0) return -1;
        if (rankCompare == 0 && classCompare < 0) return -1;
        if (rankCompare == 0 && classCompare > 0) return 1;

        return rankCompare;
    }
}
