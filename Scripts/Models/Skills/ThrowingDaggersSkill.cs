using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * The most basic attack
 * uses the isAttacking bool
 */
public class ThrowingDaggersSkill : Skill
{
    float staminaResult;
    UnitController enemyTarget;
    private AudioClip fireSFX;
    private AudioClip hitSFX;
    public ThrowingDaggersSkill()
    {
        maxRange = 3;
        //minRange = 1;
        effect = Resources.Load("Effects/CFX_Hit_C White") as GameObject;
        projectile = Resources.Load("Weapons/Dagger_2") as GameObject;

        fireSFX = (AudioClip)Resources.Load("Sounds/mixkit-arrow");
        hitSFX = (AudioClip)Resources.Load("Sounds/mixkit-short-explosion");
        skillName = "Throwing Daggers";
        description = "Effective even without numbers";

        baseCooldown = 1.5f;
        currentCooldown = 1;
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
        controller.PlayAnim("isAttacking", .55f, this, enemyTarget.Location);
        //Debug.Log("started Attack anim");
    }

    //Plays after the animation timing, spawn and fire a projectile
    public override void HandleAnimExtra()
    {
        // Debug.Log("calculating dmg");
        Vector3 startingPos = controller.transform.position;
        startingPos.y += 7;

     
        GameObject copy = UnityEngine.Object.Instantiate(projectile, startingPos, controller.transform.rotation) as GameObject;
        copy.AddComponent<Projectile>();

        Projectile proj = copy.GetComponent<Projectile>();
        proj.target = enemyTarget;
        proj.speed = 300;
        proj.skill = this;

        Director.Instance.PlaySound(fireSFX);

    }
    public override void Resolve()
    {
        if (enemyTarget.GetState() == "DEAD") { isRunning = false; return; }
        Vector3 pos = enemyTarget.transform.position;
        pos.y += 8;
        enemyTarget.PlayEffect(effect, pos, 1.5f);

        //Calculate the damage done
        CalculateDamage(controller.data, enemyTarget.data);
        Director.Instance.PlaySound(hitSFX);

    }

    public void CalculateDamage(UnitDataStore data, UnitDataStore enemy)
    {
        Color color = Color.white;
        Vector3 position = enemyTarget.transform.position;
        position.y += 10;
        position.x += (float)0.5;

        //Base damage is based on max troop count, should help ensure a stable damage range
        float lowerBound = (data.GetMaxTroopCount() / 17);
        float upperBound = (data.GetMaxTroopCount() / 12);

        //Setup power modifier
        float powerModifier = data.GetCurrentPower() * 2.5f;

        //Setup base count modifier, a small debuff or buff based on the current health comparison
        //float tcCompareMult = (data.GetCurrentTroopCount() - enemy.GetCurrentTroopCount()) * 0.05f;

        lowerBound = lowerBound + powerModifier;
        upperBound = upperBound + powerModifier;

        int damageData = (int)UnityEngine.Random.Range(lowerBound, upperBound);
        //Apply Defense Reductions

        float defValueReduction = (enemy.GetCurrentDefense() / 200) + data.GetBaseDefReduction();
        damageData -= (int)(damageData * defValueReduction);

        if (damageData < 0) damageData = 0; //We don't go below zero

        float critValue = data.GetCurrentCrit() * 100;
        float critCheck = UnityEngine.Random.Range(0, 100);

        if (critCheck <= critValue)
        { //Successful crit
            damageData = (int)(damageData * 1.5f);
            enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);
            DamageGenerator.gen.CreatePopup(position, damageData.ToString() + "!", Color.red);
        }
        else
        { //Run normally

            enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);
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
    public int GetMaxRange()
    {
        return maxRange;
    }

    public override void EffectDestroyed()
    {
        throw new NotImplementedException();
    }
}
