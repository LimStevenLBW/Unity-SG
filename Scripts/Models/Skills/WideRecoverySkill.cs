﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * 
 */
public class WideRecoverySkill : Skill
{
    float staminaResult;
    private AudioClip hitSFX;
    public WideRecoverySkill()
    {
        minRange = 0;
        maxRange = 4;
        effect = Resources.Load("Effects/Healing circle") as GameObject;
        hitSFX = (AudioClip)Resources.Load("Sounds/undertale/starfalling");
        skillName = "Wide Recovery";
        description = "We come with great healthcare!";

        baseCooldown = 12;
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

        UnitController enemyTarget;
        //If we have a target and that target is within range, continue
        enemyTarget = controller.path.GetNearestEnemy();
        if (enemyTarget == null) return false;
        if (controller.path.distanceToNearestEnemy > maxRange) return false;

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
        controller.PlayAnim("isAttacking", .65f, this);
    
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

        Director.Instance.PlaySound(hitSFX);

    }

    public override void Resolve()
    {

    }

    public void CalculateHealing(UnitController ally)
    { 
        Vector3 position = ally.transform.position;
        position.y += 10;
        position.x += (float)0.5; 

        //Base healing
        float lowerBound = (data.GetCurrentTroopCount() / 30);
        float upperBound = (data.GetCurrentTroopCount() / 20);

        //Setup magic modifier
        float magicModifier = data.GetCurrentMagic() * 2;
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

    public override bool IsSkillRunning()
    {
        return isRunning;
    }

    public override void EffectDestroyed()
    {
        throw new NotImplementedException();
    }
}