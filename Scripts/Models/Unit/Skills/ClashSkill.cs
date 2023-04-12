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
public class ClashSkill : Skill
{
    float staminaResult;
    UnitController enemyTarget;

    public ClashSkill()
    {
        maxRange = 1;
        minRange = 1;
        effect = Resources.Load("Effects/CFX_Hit_C White") as GameObject;
        skillName = "Clash";
        description = "A simple infantry attack. Much more effective with number advantage";

        baseCooldown = 2;
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
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        Vector3 pos = enemyTarget.transform.position;
        pos.y += 8;
        enemyTarget.PlayEffect(effect, pos, 1.5f);

        //Calculate the damage done
        CalculateDamage(controller.data, enemyTarget.data);
        //play sound
    }

    public void CalculateDamage(UnitDataStore thisGuy, UnitDataStore enemy)
    {
        Color color = Color.white;
        Vector3 position = enemyTarget.transform.position;
        position.y += 10;
        position.x += (float)0.5;

        //Base damage is based on max troop count, should help ensure a stable damage range
        float lowerBound = (thisGuy.GetMaxTroopCount() / 15);
        float upperBound = (thisGuy.GetMaxTroopCount() / 10);

        //Setup base count modifier, a small debuff or buff based on the current health comparison
        float baseModifier = (thisGuy.GetCurrentTroopCount() - enemy.GetCurrentTroopCount()) * 0.05f;

        //Apply Base Modifier
        lowerBound += baseModifier;
        upperBound += baseModifier;

        //Apply Power vs Defense Modifiers
        float lowerModifier = (thisGuy.GetCurrentPower() - enemy.GetCurrentDefense()) * lowerBound * 0.1f;
        float upperModifier = (thisGuy.GetCurrentPower() - enemy.GetCurrentDefense()) * upperBound * 0.1f;

        lowerBound += lowerModifier;
        upperBound += upperModifier;

        int damageData = (int)UnityEngine.Random.Range(lowerBound, upperBound);

        if (damageData < 0) damageData = 0; //We don't go below zero
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
}
