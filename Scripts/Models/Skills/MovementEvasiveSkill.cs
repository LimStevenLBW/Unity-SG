using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class MovementEvasiveSkill : Skill
{
    float staminaResult;
    UnitController enemyTarget;

    public MovementEvasiveSkill()
    {
        skillName = "MovementEvasive";
        description = "Float like a butterfly";

        baseCooldown = 1f;
        currentCooldown = 0;
        baseStaminaCost = 2;
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

        enemyTarget = null;
        //If we have a target and that target is within range, continue
        enemyTarget = controller.path.GetNearestEnemy();
        if (enemyTarget == null) return false;

        if (controller.path.distanceToNearestEnemy == 1) return true; //If the enemy is directly in front, we need to escape

        if (controller.path.distanceToNearestEnemy <= controller.maxRange) return false; //Else we do nothing

        //If we have enough stamina and if it is off cooldown, then the move is available
        if (staminaResult >= 0 && currentCooldown <= 0)
        {
            //if (controller == null) Debug.Log("controller null");
            //if (controller.path == null) Debug.Log("path null");
            bool notTrapped = !controller.path.IsThisUnitSurrounded(); //If this unit is surrounded by allies for example, it won't be able to move
            if (notTrapped) return true;

        }
        return false;
    }

    public override void DoSkill()
    {
        int distance;

        if (controller.path.distanceToNearestEnemy == 1)
        {
            controller.SetTravelSpeed(12);
            controller.SetRotationSpeed(720f);
            distance = controller.path.EscapeFrom(enemyTarget);
        }
        else
        {
            controller.SetTravelSpeed(4);
            controller.SetRotationSpeed(360f);
            distance = controller.path.FindPathToNearestEnemy();
        }

        //If we need to move, although, there should have been a check already to protect from this
        if (distance > 0)
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
