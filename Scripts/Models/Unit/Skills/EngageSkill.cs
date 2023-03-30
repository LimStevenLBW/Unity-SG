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

    public EngageSkill()
    {
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

    public override bool IsAvailable()
    {
        //If we still have stamina
        //Calculate how much stamina we would have IF we were to do the move
        double staminaResult = data.GetCurrentStamina() - currentStaminaCost;

        //If we have enough stamina and if it is off cooldown, then the move is available
        if (staminaResult >= 0 && currentCooldown <= 0)
        {
            bool nearbyEnemy = controller.path.IsThereAdjacentEnemy(); //We also need a target
            if (nearbyEnemy) return true;
        }
        return false;

       
    }

    public override void DoSkill()
    {

        staminaResult = data.GetCurrentStamina() - currentStaminaCost;
        data.SetCurrentStamina(staminaResult);
      
        //Have the unitcontroller play the attack animation
        controller.PlayAnim("isAttacking", .45f, this);
       
        //Once complete, reset the CDR
        ResetCD();
    
        //Reset the state
        controller.SetState("IDLE");
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        Vector3 position = controller.transform.position;
        position.y += 5;
        position.x += (float)0.5;
        double damageData = 500;
        Color color = Color.green;
        DamageGenerator.gen.CreatePopup(position, damageData.ToString(), color);
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
