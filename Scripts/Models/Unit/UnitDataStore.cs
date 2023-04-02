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
    public bool wasUpdated = false;

    private Unit unitBase;
    public string unitName;
    public string faction;
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

    private int currentTroopCount;
    private int maxTroopCount; //With buffs, we can go past the normal troop count during combat
    private int TroopCount;

    //Leadership Stats, these will be within the range of 1-99. In some cases can even affect combat.
    private int leadership;
    private int engineering;
    private int magic;
    private int economy;
    private int agriculture;

    private float upkeep;

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
        currentTroopCount = maxTroopCount;
        maxStamina = unitBase.baseStamina;
        currentStamina = maxStamina;

        currentPower = unitBase.basePower;
        currentMagic = unitBase.baseMagic;
        currentDefense = unitBase.baseDefense;
        currentSpeed = unitBase.baseSpeed;
        currentCrit = unitBase.baseCrit;
        //CritBoost = baseCritBoost;

        InitSkills();
        //movementSkill = unitBase.movementSkill;
        //movementCD = movementSkill.baseCooldown;
    }


    /*
     * Setup the skill instances
     */
    public void InitSkills() {
        movementSkill = unitBase.GetSkill(unitBase.movementSkill_ID);
        if(movementSkill != null) movementSkill.Init(this, controller);

        skill1 = unitBase.GetSkill(unitBase.skill1_ID);
        if (skill1 != null) skill1.Init(this, controller);
    }

    public string GetName() { return unitName; }

    public void StartListening()
    {
        Timer.OnSecondPassed += SecondPassed;
    }

    void SecondPassed()
    {
        if (movementSkill != null) movementSkill.SecondPassed();
        if (skill1 != null) skill1.SecondPassed();
    }

    public bool IsMovementAvailable()
    {
        if (movementSkill == null) return false;

       //If we still have stamina
       //Calculate how much stamina we would have IF we were to do the move
       float staminaResult = currentStamina - movementSkill.currentStaminaCost;

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
    public float GetExp() { return exp; }
    public void SetExp(float value) { exp = value; }

    public float GetCurrentPower() { return currentPower; }
    public void SetCurrentPower(float value) { currentPower = value; }
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

    public float GetCurrentStamina() { return currentStamina; }
    public void SetCurrentStamina(float value) { currentStamina = value; }
    public float GetMaxStamina() { return maxStamina; }
    public void SetMaxStamina(float value) { maxStamina = value; }
    public float GetStamina() { return Stamina; }
    public void SetStamina(float value) { Stamina = value; }

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

    public int GetCurrentTroopCount() { return currentTroopCount; }
    public void SetCurrentTroopCount(int value) { currentTroopCount = value; }
    public int GetMaxTroopCount() { return maxTroopCount; }
    public void SetMaxTroopCount(int value) { maxTroopCount = value; }
    public int GetTroopCount() { return TroopCount; }
    public void SetTroopCount(int value) { TroopCount = value; }
}
