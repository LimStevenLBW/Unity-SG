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
    double staminaResult;
    UnitController enemyTarget;

    public EngageSkill()
    {
        effect = Resources.Load("Effects/CFXR4 Sword Hit PLAIN (Cross)") as GameObject;
        skillName = "Engage";
        description = "Fortune favors the bold";

        baseCooldown = 4;
        currentCooldown = baseCooldown;
        baseStaminaCost = 10;
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
    
        //Reset the state
        controller.SetState("IDLE");
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        Vector3 position = enemyTarget.transform.position;
        position.y += 10;
        position.x += (float)0.5;

        double damageData = 500;

        Color color = Color.white;

        DamageGenerator.gen.CreatePopup(position, damageData.ToString(), color);
        enemyTarget.PlayEffect(effect);
        //play sound
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
