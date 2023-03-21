using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "March", menuName = "Character/Skill/MarchSkill")]
public class MarchSkill : Skill
{
    public override void Initialize(Unit unit, UnitController controller, UnitManager manager)
    {
        this.unit = unit;
        this.controller = controller;
        this.manager = manager;

        cooldown = baseCooldown;
        actionCost = baseActionCost;
        
    }

    public override void StartListening()
    {
        Timer.OnSecondPassed += SecondPassed;
    }

    public override void SecondPassed()
    {
        currentCooldown -= 3;
        if(currentCooldown == 0)
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
        if (currentCooldown <= 0) return true;

        return false;
    }

    public override void DoSkill()
    {
        //First, find the nearest enemy
        manager.FindNearestEnemy(controller);
        //Spend an action point
        //Move one space towards the enemy
        //play the animation until that walk is done

        ResetCD();
    }

    public override void Reset()
    {
        currentCooldown = cooldown;
        currentActionCost = actionCost;
    }

    public override void ResetCD()
    {
        currentCooldown = cooldown;
    }

    public override void ResetAC()
    {
        currentActionCost = actionCost;
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
