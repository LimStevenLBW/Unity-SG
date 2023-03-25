using UnityEngine;

/* Defines character/class skills 
 * which determine AI actions during combat
 */
public abstract class Skill: ScriptableObject
{
    public string skillName;
    public string description;
    public double baseCooldown;
    public double baseStaminaCost;

    internal double cooldown;
    internal double staminaCost; //persistent amount outside of combat

    internal double currentCooldown;
    internal double currentStaminaCost; //used during combat

    
    internal UnitManager manager;
    internal UnitController controller;
    internal Unit unit;

    //Initialize cooldown and action costs
    public abstract void Initialize(Unit unit, UnitController controller, UnitManager manager);
    public abstract void StartListening();
    public abstract void SecondPassed();
    public abstract void DoSkill();

    public abstract bool IsAvailable();

    public abstract void Reset();

    public abstract void ResetCD();
    public abstract void ResetAC();

    public abstract string GetSkillName();

    public abstract string GetDescription();

    public abstract void GetController(UnitController controller);

    public abstract void GetUnit(Unit unit);
}
