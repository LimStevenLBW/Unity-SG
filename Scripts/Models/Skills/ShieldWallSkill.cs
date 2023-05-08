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
public class ShieldWallSkill : Skill
{
    float staminaResult;
    private AudioClip hitSFX;
    private int duration;
    private float defenseModifier;
    private bool auraActive;
    public ShieldWallSkill()
    {
        minRange = 0;
        maxRange = 0;
        effect = Resources.Load("Effects/Defense Aura Custom") as GameObject;
        hitSFX = (AudioClip)Resources.Load("Sounds/undertale/power up 2");
        skillName = "Shield Wall";
        description = "Nothing on Earth can make you fall.";

        auraActive = false;
        duration = 5;
        baseCooldown = 3;
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
        if (auraActive) return false;

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
        auraActive = true;
        //We don't reset aura cooldowns until the previous one ends
        isRunning = true; // Indicate that the skill is calculating;

        staminaResult = data.GetCurrentStamina() - currentStaminaCost;
        data.SetCurrentStamina(staminaResult);

        //Have the unitcontroller play the attack animation(for now)
        controller.PlayAnim("isCasting", .55f, this);
        
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        Vector3 pos = controller.transform.position;
        pos.y = 0;
        controller.AddAura(effect, pos, this, duration);
        

        //Calculate the healing done
        CalculateEffect(data);
        Director.Instance.PlaySound(hitSFX);

    }

    public void CalculateEffect(UnitDataStore data)
    {
        Vector3 position = controller.transform.position;
        position.y += 10;
        position.x += (float)0.5; 

        //Setup DEF modifier
        defenseModifier = 20 + (data.GetCurrentDefense() / 2);

        data.SetCurrentDefense(data.GetCurrentDefense() + defenseModifier);

        //Display Data
        DamageGenerator.gen.CreatePopup(position, "DEF UP", Color.blue);
        //Terminate
        isRunning = false;
    }

    public override void EffectDestroyed()
    {
        auraActive = false;
        ResetCD();
        data.SetCurrentDefense(data.GetCurrentDefense() - defenseModifier);
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
}
