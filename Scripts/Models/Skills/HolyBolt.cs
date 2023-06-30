using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * Basic Ranged Magic
 * uses the isAttacking bool
 */
public class HolyBoltSkill : Skill
{
    private float staminaResult;
    private UnitController enemyTarget;
    private AudioClip hitSFX;

    public HolyBoltSkill()
    {
        maxRange = 5; // The max range that this skill can be used
       // minRange = 3;
        effect = Resources.Load("Effects/holy_bolt") as GameObject;
        hitSFX = (AudioClip) Resources.Load("Sounds/undertale/impact big");
        skillName = "Holy Bolt";
        description = "A weak mid-ranged magic attack that reduces the enemy's POW";

        baseCooldown = 2.5f;
        currentCooldown = 1f; //starting cooldown is reduced
        baseStaminaCost = 5;
        currentStaminaCost = baseStaminaCost;
        isRunning = false;
    }

    public override void Init(UnitDataStore data, UnitController controller)
    {
        this.data = data;
        this.controller = controller;
    }

    public override void SecondPassed()
    {
        //Subtract one from the cooldown
        if(currentCooldown > 0) currentCooldown -= 1;

    }

    //This is an attacking skill, we need a single valid target
    public override bool IsAvailable()
    {
        //If we still have stamina
        //Calculate how much stamina we would have IF we were to do the move
        double staminaResult = data.GetCurrentStamina() - currentStaminaCost;

        //If we have a target and that target is within range, continue
        enemyTarget = controller.path.GetNearestEnemy();
        if (enemyTarget == null) return false;
        if (controller.path.distanceToNearestEnemy > maxRange) return false;

        //If we have enough stamina and if it is off cooldown, check for a target
        if (staminaResult >= 0 && currentCooldown <= 0)
        {
            return true;
        }

        return false;
       
    }

    public override void DoSkill()
    {
        ResetCD();
        enemyTarget = null;

        enemyTarget = controller.path.GetNearestEnemy();

        if (enemyTarget == null) //Abort using the skill
        {
            controller.SetState("IDLE");
            return;
        }

        isRunning = true; // Indicate that the skill is calculating;
        staminaResult = data.GetCurrentStamina() - currentStaminaCost;
        data.SetCurrentStamina(staminaResult);

        //Have the unitcontroller play the attack animation
        controller.PlayAnim("isAttacking", .75f, this, enemyTarget.Location);
    
        //Reset the state, changed, we reset after the animation is done in coroutine instead
        //controller.SetState("IDLE");
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        Vector3 pos = enemyTarget.transform.position;

        enemyTarget.PlayEffect(effect, pos, 4);

        //Calculate the damage done
        CalculateDamage(controller.data, enemyTarget.data, false);

        Director.Instance.PlaySound(hitSFX);
    }

    /*
     * This is an area of effect spell, so it affects surrounding HexCells as well
     * if it is marked as splash damage, we'll apply the weaker numbers
     */
    public void CalculateDamage(UnitDataStore data, UnitDataStore enemy, bool isSplash)
    {
        Color color = Color.white;
        Vector3 position = enemy.controller.transform.position;
        position.y += 10;
        position.x += (float)0.5;

        float lowerBound;
        float upperBound;
        //Base damage 
    
        lowerBound = (data.GetMaxTroopCount() / 15);
        upperBound = (data.GetMaxTroopCount() / 10);

        //Setup magic modifier
        float magicModifier = data.GetCurrentMagic() * 1f;

        //Setup troop count modifier
        float tcCompareMult = (data.GetCurrentTroopCount() - enemy.GetCurrentTroopCount()) * 0.05f;

        //Apply Modifiers
        lowerBound = lowerBound + magicModifier + tcCompareMult;
        upperBound = upperBound + magicModifier + tcCompareMult;

        int damageData = (int)UnityEngine.Random.Range(lowerBound, upperBound);

        //Apply Defense Reductions
        float defValueReduction = (enemy.GetCurrentDefense() / 200) + data.GetBaseDefReduction();
        //Debug.Log("Base Damage = " + damageData + ", Damage Reduce Multiplier = " + damageData * defValueReduction);
        damageData -= (int)(damageData * defValueReduction);
       // Debug.Log("Actual Damage = " + damageData);
        if (damageData < 0) damageData = 0; //We don't go below zero

        float critValue = data.GetCurrentCrit() * 100;
        float critCheck = UnityEngine.Random.Range(0, 100);

        if (critCheck <= critValue)
        { //Successful crit

            damageData = (int)(damageData * 1.5f);
            enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);
            enemy.SetCurrentPower(enemy.GetCurrentPower() - 2); //Debuff the enemy
            DamageGenerator.gen.CreatePopup(position, damageData.ToString() + "!", Color.red);
        }
        else
        { //Run normally

            enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);
            enemy.SetCurrentPower(enemy.GetCurrentPower() - 1); //Debuff the enemy
            DamageGenerator.gen.CreatePopup(position, damageData.ToString(), color);
        }
        //Terminate
        isRunning = false;

    }

    public override void Reset()
    {
        
    }

    public override void ResetCD()
    {
        currentCooldown = baseCooldown;
    }

    public override void ResetAC()
    {
      
    }

    public override string GetSkillName()
    {
        return  skillName;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override bool IsSkillRunning()
    {
        return isRunning;
    }
    public override void Resolve()
    {

    }

    public override void EffectDestroyed()
    {
        throw new NotImplementedException();
    }
}
