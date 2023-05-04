using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//CURRENTLY BUGGED WHEN TWO UNITS THAT CAN CHARGE GO AT EACH OTHER
public class ChargeSkill : Skill
{
    float staminaResult;
    
    public ChargeSkill()
    {
        skillName = "Charge!!";
        description = "I said CHARGE god damn it!";

        baseCooldown = 1f;
        currentCooldown = baseCooldown;
        baseStaminaCost = 10;
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
        return false; //unused currently
    }

    public override void DoSkill()
    {
        ResetCD();
        isRunning = true; // Indicate that the skill is calculating;
        bool nearbyEnemy = controller.path.IsThereAdjacentEnemy();

        //Debug.Log("Unit: " + data.GetName() + " knows pathfinding is " + manager.PATHFINDING_IN_USE);

        //There is also no need to move if we already have a nearby unit
        if (!nearbyEnemy)
        {
            //First, find the distance of the path to the nearest enemy
            int distance = controller.path.FindPathToNearestEnemy();

            //If we need to move
            if (distance > 0)
            {
                //Update the stamina
                staminaResult = data.GetCurrentStamina() - currentStaminaCost;
                data.SetCurrentStamina(staminaResult);

                //Have the controller move one cell along that path
                controller.path.DoMove(controller, 4, this);

                //Terminate
                isRunning = false;

            }
        }
        else
        {
            //Do Nothing, don't need to move after all
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
    public override void Resolve()
    {

    }
}
