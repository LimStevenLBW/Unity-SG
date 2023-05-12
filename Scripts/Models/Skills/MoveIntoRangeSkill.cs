using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


//NOT USED CURRENTLY, TODO, UPDATE FIND ENEMY WITHIN RANGE TO CALCULATE WITHOUT PATHING FUNCTIONS
public class MoveIntoRangeSkill : Skill
{
    float staminaResult;

    public MoveIntoRangeSkill()
    {
        skillName = "Move into range";
        description = "Fire within range";

        baseCooldown = 1;
        currentCooldown = 0;
        baseStaminaCost = 1;
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

        UnitController enemyTarget;
        //If we have a target and that target is within range, continue
        enemyTarget = controller.path.GetNearestEnemy();
        if (enemyTarget == null) return false;
        //Debug.Log(controller.path.distanceToNearestEnemy);
        if (controller.path.distanceToNearestEnemy < controller.maxRange) return false;

        //If we have enough stamina and if it is off cooldown, then the move is available
        if (staminaResult >= 0 && currentCooldown <= 0)
        {
            //if (controller == null) Debug.Log("controller null");
            //if (controller.path == null) Debug.Log("path null");
            bool noNearbyEnemy = !controller.path.IsThereAdjacentEnemy(); //if there no nearby enemy, we should do the move
            bool notTrapped = !controller.path.IsThisUnitSurrounded(); //If this unit is surrounded by allies for example, it won't be able to move
            if (noNearbyEnemy && notTrapped) return true;

        }
        return false;
    }

    public override void DoSkill()
    {
        
        //First, find the distance of the path to the nearest enemy
        int distance = controller.path.FindPathToNearestEnemy();

        //If we need to move, although, there should have been a check already to protect from this
        if (distance > 1)
        {
            ResetCD();

            //Update the stamina
            staminaResult = data.GetCurrentStamina() - currentStaminaCost;
            data.SetCurrentStamina(staminaResult);

            isRunning = true; // Indicate that the skill is calculating;

            //Have the controller move one cell along that path
            controller.path.DoMove(controller, 1, this);

            //Terminate
            isRunning = false;

        }
        else
        {

            ResetCD();
            controller.path.ClearPath();
            //Movement failed, cancel
            controller.SetState("IDLE");
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
