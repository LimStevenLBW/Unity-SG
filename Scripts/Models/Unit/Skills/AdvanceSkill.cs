using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AdvanceSkill : Skill
{
    float staminaResult;
    
    public AdvanceSkill()
    {
        skillName = "Advance";
        description = "Fire within range";

        baseCooldown = 1.0f;
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

    public override bool IsAvailable()
    {
        //If we still have stamina, calculate how much stamina we would have IF we were to do the move
        float staminaResult = data.GetCurrentStamina() - currentStaminaCost;

        //If we have enough stamina and if it is off cooldown, then the move is available
        if (staminaResult >= 0 && currentCooldown <= 0)
        {
            bool nearbyEnemy = controller.path.IsThereAdjacentEnemy(); //if there no nearby enemy, we can do the move
            if (!nearbyEnemy) return true;

        }
        return false;
    }

    public override void DoSkill()
    {
        ResetCD();
        isRunning = true; // Indicate that the skill is calculating;

        //First, find the distance of the path to the nearest enemy
        int distance = controller.path.FindPathToNearestEnemy();

        //If we need to move
        if (distance > 0)
        {
            //Update the stamina
            staminaResult = data.GetCurrentStamina() - currentStaminaCost;
            data.SetCurrentStamina(staminaResult);

            //Have the controller move one cell along that path
            controller.path.DoMove(controller, 1, this);

            //Terminate
            isRunning = false;

        }

    }

    public override void HandleAnimExtra()
    {

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
