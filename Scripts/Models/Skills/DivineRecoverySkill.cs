using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * 
 */
public class DivineRecoverySkill : Skill
{
    float staminaResult;

    public DivineRecoverySkill()
    {
        minRange = 0;
        maxRange = 0;
        effect = Resources.Load("Effects/Healing circle") as GameObject;
        skillName = "Wide Recovery";
        description = "Who needs medicine when you have religion";

        baseCooldown = 16;
        currentCooldown = baseCooldown;
        baseStaminaCost = 0;
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

    //This is an attacking skill, we need a single valid target
    public override bool IsAvailable()
    {
        //If we still have stamina
        //Calculate how much stamina we would have IF we were to do the move
        float staminaResult = data.GetCurrentStamina() - currentStaminaCost;

        //If we have enough stamina and if it is off cooldown
        if (staminaResult >= 0 && currentCooldown <= 0)
        {

            return true;

        }
        return false;
       
    }

    public override void DoSkill()
    {
        ResetCD();
        isRunning = true; // Indicate that the skill is calculating;

        staminaResult = data.GetCurrentStamina() - currentStaminaCost;
        data.SetCurrentStamina(staminaResult);

        //Have the unitcontroller play the attack animation(for now)
        controller.PlayAnim("isAttacking", .45f, this);
    
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        List<UnitController> allies = controller.GetAllies();

        foreach(UnitController ally in allies)
        {
            Vector3 pos = ally.transform.position;
            pos.y = 0;
            if (ally.GetState() != "DEAD")
            {
                ally.PlayEffect(effect, pos, 2);
                CalculateHealing(ally);
            }
        }

    }

    public void CalculateHealing(UnitController ally)
    {
        Color color = Color.white;
        Vector3 position = ally.transform.position;
        position.y += 10;
        position.x += (float)0.5; 

        //Base healing
        float lowerBound = (data.GetCurrentTroopCount() / 3);
        float upperBound = (data.GetCurrentTroopCount());


        //Setup magic modifier
        float magicModifier = (data.GetCurrentMagic()) + (data.GetCurrentMagic() * .15f);
        lowerBound += magicModifier;
        upperBound += magicModifier;

        int result = (int)UnityEngine.Random.Range(lowerBound, upperBound);

        ally.data.SetCurrentTroopCount(data.GetCurrentTroopCount() + result);

        //Display Data
        DamageGenerator.gen.CreatePopup(position, result.ToString(), Color.green);
        //Terminate
        isRunning = false;
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

    public override void GetController(UnitController ally)
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
