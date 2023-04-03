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
public class EngageSkill : Skill
{
    float staminaResult;
    UnitController enemyTarget;

    public EngageSkill()
    {
        effect = Resources.Load("Effects/CFXR4 Sword Hit PLAIN (Cross)") as GameObject;
        skillName = "Engage";
        description = "A simple infantry attack. Much more effective with number advantage";

        baseCooldown = 2;
        currentCooldown = baseCooldown;
        baseStaminaCost = 5;
        currentStaminaCost = baseStaminaCost;
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

        //If we have enough stamina and if it is off cooldown, check for a target
        if (staminaResult >= 0 && currentCooldown <= 0)
        {
            enemyTarget  = controller.path.GetAdjacentEnemy(); //set the target, mark the move as available
            if (enemyTarget) return true;
        }
        return false;

       
    }

    public override void DoSkill()
    {

        staminaResult = data.GetCurrentStamina() - currentStaminaCost;
        data.SetCurrentStamina(staminaResult);

        //Have the unitcontroller play the attack animation
        controller.PlayAnim("isAttacking", .45f, this, enemyTarget.Location);

        //Once complete, reset the CDR
        ResetCD();
    
        //Reset the state, changed, we reset after the animation is done in coroutine instead
        //controller.SetState("IDLE");
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        enemyTarget.PlayEffect(effect);

        //Calculate the damage done
        CalculateDamage(controller.data, enemyTarget.data);
        //play sound
    }

    public void CalculateDamage(UnitDataStore ally, UnitDataStore enemy)
    {
        Color color = Color.white;
        Vector3 position = enemyTarget.transform.position;
        position.y += 10;
        position.x += (float)0.5;


        //Base damage 
        float lowerBound = (ally.GetCurrentTroopCount() / 4);
        float upperBound = (ally.GetCurrentTroopCount() / 3);

        //Setup power vs defense modifier
        float powerVsDefenseMult = (ally.GetCurrentPower() - enemy.GetCurrentDefense()) * 3;

        //Setup troop count modifier
        float tcCompareMult = (ally.GetCurrentTroopCount() - enemy.GetCurrentTroopCount()) * 0.2f;

        //Apply Modifiers
        lowerBound = lowerBound + powerVsDefenseMult + tcCompareMult;
        upperBound = upperBound + powerVsDefenseMult + tcCompareMult;

        int damageData = (int)UnityEngine.Random.Range(lowerBound, upperBound);

        if (damageData < 0) damageData = 0; //We don't go below zero
        enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);

        //Display Data
        DamageGenerator.gen.CreatePopup(position, damageData.ToString(), color);
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

    public override void GetController(UnitController controller)
    {
        this.controller = controller;
    }

}
