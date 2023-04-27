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
public class RecoverySkill : Skill
{
    float staminaResult;

    public RecoverySkill()
    {
        minRange = 0;
        maxRange = 0;
        effect = Resources.Load("Effects/Healing circle") as GameObject;
        skillName = "Recovery";
        description = "Heal your people, so they can get hurt again.";

        baseCooldown = 10;
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
          
          //Don't bother to heal if our health is full
          if(data.GetCurrentTroopCount() != data.GetMaxTroopCount())
          {
                return true;
          }

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
        Vector3 pos = controller.transform.position;
        pos.y = 0;
        controller.PlayEffect(effect, pos, 2);

        //Calculate the damage done
        CalculateHealing(controller.data);
        //play sound
    }

    public void CalculateHealing(UnitDataStore thisGuy)
    {
       
        Color color = Color.white;
        Vector3 position = controller.transform.position;
        position.y += 10;
        position.x += (float)0.5; 

        //Base healing
        float lowerBound = (thisGuy.GetCurrentTroopCount() / 5);
        float upperBound = (thisGuy.GetCurrentTroopCount() / 4);


        //Setup magic modifier
        float magicModifier = (thisGuy.GetCurrentMagic()) + (thisGuy.GetCurrentMagic() * .05f);
        lowerBound += magicModifier;
        upperBound += magicModifier;

        int result = (int)UnityEngine.Random.Range(lowerBound, upperBound);

        data.SetCurrentTroopCount(data.GetCurrentTroopCount() + result);

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

    public override void GetController(UnitController controller)
    {
        this.controller = controller;
    }

    public override bool IsSkillRunning()
    {
        return isRunning;
    }
}
