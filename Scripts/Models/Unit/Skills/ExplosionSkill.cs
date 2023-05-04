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
public class ExplosionSkill : Skill
{
    float staminaResult;
    UnitController enemyTarget;

    public ExplosionSkill()
    {
        maxRange = 6;
        minRange = 3;
        effect = Resources.Load("Effects/CFX_Explosion_B_Smoke+Text") as GameObject;
        skillName = "Explosion";
        description = "";

        baseCooldown = 7;
        currentCooldown = baseCooldown;
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
        ResetCD();
        isRunning = true; // Indicate that the skill is calculating;
        staminaResult = data.GetCurrentStamina() - currentStaminaCost;
        data.SetCurrentStamina(staminaResult);

        //Have the unitcontroller play the attack animation
        controller.PlayAnim("isAttacking", .45f, this, enemyTarget.Location);
    
        //Reset the state, changed, we reset after the animation is done in coroutine instead
        //controller.SetState("IDLE");
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        Vector3 pos = enemyTarget.transform.position;
        pos.y += 8;
        enemyTarget.PlayEffect(effect, pos, 4);

        //Calculate the damage done
        CalculateDamage(controller.data, enemyTarget.data, false);

        //Calculate the damage done to nearby allies of the enemy unit as well
        List<UnitController> nearbyEnemies = new List<UnitController>(enemyTarget.path.GetAllAdjacent(1, true));
        foreach (UnitController e in nearbyEnemies)
        {
            CalculateDamage(controller.data, e.data, true);
        }
        //play sound
    }

    /*
     * This is an area of effect spell, so it affects surrounding HexCells as well
     * if it is marked as splash damage, we'll apply the weaker numbers
     */
    public void CalculateDamage(UnitDataStore ally, UnitDataStore enemy, bool isSplash)
    {
        Color color = Color.white;
        Vector3 position = enemy.controller.transform.position;
        position.y += 10;
        position.x += (float)0.5;

        float lowerBound;
        float upperBound;
        //Base damage 
        if (!isSplash)
        {
            lowerBound = (ally.GetCurrentTroopCount() / 3);
            upperBound = (ally.GetCurrentTroopCount() / 2);
        }
        else
        {
            lowerBound = (ally.GetCurrentTroopCount() / 5);
            upperBound = (ally.GetCurrentTroopCount() / 4);
        }
        

        //Setup power vs defense modifier
        float powerVsDefenseMult = (ally.GetCurrentPower() - enemy.GetCurrentDefense()) * 3;

        //Setup troop count modifier
        float tcCompareMult = (ally.GetCurrentTroopCount() - enemy.GetCurrentTroopCount()) * 0.2f;

        //Apply Modifiers
        lowerBound = lowerBound + powerVsDefenseMult + tcCompareMult;
        upperBound = upperBound + powerVsDefenseMult + tcCompareMult;

        int damageData = (int)UnityEngine.Random.Range(lowerBound, upperBound);

        if (damageData < 0) damageData = 0; //We don't go below zero

        //Damage to Center
        enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);

        //Display Data
        DamageGenerator.gen.CreatePopup(position, damageData.ToString(), color);
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

    public override void GetController(UnitController controller)
    {
        this.controller = controller;
    }

    public override bool IsSkillRunning()
    {
        return isRunning;
    }
    public override void Resolve()
    {

    }
}
