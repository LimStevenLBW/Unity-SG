using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "March", menuName = "Character/Skill/MarchSkill")]
public class MarchSkill : Skill
{
    public override void InitBaseFields()
    {
        cooldown = baseCooldown;
        actionCost = baseActionCost;
    }

    public override void StartListening()
    {
        Timer.OnSecondPassed += SecondPassed;
    }

    public override void SecondPassed()
    {
        currentCooldown -= 1;
        if(currentCooldown == 0)
        {
            Debug.Log(skillName + " is Available");
        }
    }

    public override bool IsAvailable()
    {
        if (currentCooldown <= 0) return true;

        return false;
    }

    public override void DoSkill()
    {
        ResetCD();
        Debug.Log("MOVING");
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


}


