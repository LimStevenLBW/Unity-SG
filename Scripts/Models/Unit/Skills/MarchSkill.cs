using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MarchSkill : Skill
{
    double staminaResult;

    public MarchSkill()
    {
        skillName = "March";
        description = "Going for a stroll";

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

    public override void DoSkill()
    {

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
                controller.path.DoMove(controller);

                //Once complete, reset the CDR
                ResetCD();

            }
        }
        else
        {
            //Do Nothing, don't need to move after all
        }

        //Reset the state
        controller.SetState("IDLE");
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
