using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "March", menuName = "Character/Skill/MarchSkill")]
public class MarchSkill : Skill
{
    double staminaResult;

    public override void Initialize(Unit unit, UnitController controller, UnitManager manager)
    {
        this.unit = unit;
        this.controller = controller;
        this.manager = manager;

        cooldown = baseCooldown;
        staminaCost = baseStaminaCost;
        
    }

    public override void StartListening()
    {
        Timer.OnSecondPassed += SecondPassed;
    }

    public override void SecondPassed()
    {
        if(currentCooldown > 0) currentCooldown -= 1;

        if (currentCooldown == 0)
        {
            Debug.Log(skillName + " is Available");
        }
        else
        {
            Debug.Log(currentCooldown);
        }
    }

    public override bool IsAvailable()
    {
        //If we still have stamina
        //Calculate how much stamina we would have IF we were to do the move

        staminaResult = unit.GetCurrentStamina() - currentStaminaCost;

        //If off cooldown and stamina is available
        if (currentCooldown <= 0 && staminaResult >= 0)
        {
            return true;
        }
        return false;

    }

    public override void DoSkill()
    {
        //First, find the distance of the path to the nearest enemy
        int distance = controller.path.FindPathToNearestEnemy();
      
        //If we need to move
        if(distance > 0)
        {
            //Update the stamina
            unit.SetCurrentStamina(staminaResult);

            //Have the controller move one cell along that path
            controller.path.DoMove(controller);

            //Once complete, reset the CDR
            ResetCD();
        }
        
        //Otherwise do nothing 
    }

    public override void Reset()
    {
        currentCooldown = cooldown;
        currentStaminaCost = staminaCost;
    }

    public override void ResetCD()
    {
        currentCooldown = cooldown;
    }

    public override void ResetAC()
    {
        currentStaminaCost = staminaCost;
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

    public override void GetUnit(Unit unit)
    {
       this.unit = unit;
    }
}
