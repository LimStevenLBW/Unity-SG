using UnityEngine;

/* Defines character/class skills 
 * which determine AI actions during combat
 */
public abstract class Skill
{
    public string skillName;
    public string description;

    public double baseCooldown;
    public double currentCooldown;
    public double baseStaminaCost;
    public double currentStaminaCost;

    internal GameObject effect;
    internal UnitController controller;
    internal UnitDataStore data;

    //Initialize cooldown and action costs
    public abstract void Init(UnitDataStore data, UnitController controller);
    public abstract void SecondPassed();
    public abstract void DoSkill();

    public abstract void HandleAnimExtra();
    public abstract bool IsAvailable();
    public abstract void Reset();

    public abstract void ResetCD();
    public abstract void ResetAC();

    public abstract string GetSkillName();

    public abstract string GetDescription();

    public abstract void GetController(UnitController controller);
}
