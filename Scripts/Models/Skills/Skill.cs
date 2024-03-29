using UnityEngine;

/* Defines character/class skills 
 * which determine AI actions during combat
 */
public abstract class Skill
{
    public enum Type
    {
        BASIC,
        SPECIAL,
        SUPER
    }

    internal bool isRunning;
    internal int maxRange;
    internal int minRange;
    public string skillName;
    public Type skillType;
    public string description;
    
    public float baseCooldown;
    public float currentCooldown;
    public float baseStaminaCost;
    public float currentStaminaCost;

    internal GameObject effect;
    internal GameObject projectile;
    internal UnitController controller;
    internal UnitDataStore data;

    //Initialize cooldown and action costs
    public abstract void Init(UnitDataStore data, UnitController controller);
    public abstract void SecondPassed();
    public abstract void DoSkill();

    public abstract void HandleAnimExtra();
    public abstract void EffectDestroyed();
    public abstract void Resolve();
    public abstract bool IsAvailable();
    public abstract void Reset();

    public abstract void ResetCD();
    public abstract void ResetAC();

    public abstract string GetSkillName();

    public abstract string GetDescription();

    public abstract bool IsSkillRunning();
}
