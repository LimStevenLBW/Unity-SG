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
public class SingleRecoverySkill : Skill
{
    float staminaResult;
    private AudioClip hitSFX;
    public SingleRecoverySkill()
    {
        minRange = 0;
        maxRange = 4;
        effect = Resources.Load("Effects/Healing circle") as GameObject;
        hitSFX = (AudioClip)Resources.Load("Sounds/undertale/starfalling");
        skillName = "Single Recovery";
        description = "Heal your injured, so they can get injured again";

        baseCooldown = 3;
        currentCooldown = baseCooldown;
        baseStaminaCost = 5;
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
        //If we still have stamina
        //Calculate how much stamina we would have IF we were to do the move
        float staminaResult = data.GetCurrentStamina() - currentStaminaCost;

        UnitController enemyTarget;
        
        //Check enemy range
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

        int lowestHp = 99999;
        UnitController allyToHeal = null;
        foreach(UnitController ally in allies)
        {
            if (ally.GetState() != "DEAD" && ally.data.IsInjured())
            {
                int troops = ally.data.GetCurrentTroopCount();
                if(troops < lowestHp)
                {
                    lowestHp = troops;
                    allyToHeal = ally;

                }
               
            }
        }

        if (allyToHeal == null) allyToHeal = controller; //Just heal yourself if there is no one to heal

        Vector3 pos = allyToHeal.transform.position;
        pos.y = 0;

        Director.Instance.PlaySound(hitSFX);
        //allyToHeal.PlayEffect(effect, pos, 2);
        allyToHeal.AddAura(effect, pos, this, 2);
        CalculateHealing(allyToHeal);

    }

    public void CalculateHealing(UnitController ally)
    {
        Vector3 position = ally.transform.position;
        position.y += 10;
        position.x += (float)0.5; 

        //Base healing
        float lowerBound = (data.GetCurrentTroopCount() / 25);
        float upperBound = (data.GetCurrentTroopCount() / 20);


        //Setup magic modifier
        float magicModifier = data.GetCurrentMagic() * 1.5f;
        lowerBound += magicModifier;
        upperBound += magicModifier;

        int result = (int)UnityEngine.Random.Range(lowerBound, upperBound);

        float critValue = data.GetCurrentCrit() * 100;
        float critCheck = UnityEngine.Random.Range(0, 100);

        if (critCheck <= critValue)
        { //Successful crit

            result = (int)(result * 1.5f);
            ally.data.SetCurrentTroopCount(ally.data.GetCurrentTroopCount() + result);
            DamageGenerator.gen.CreatePopup(position, result.ToString() + "!", Color.green);
        }
        else
        { //Run normally

            ally.data.SetCurrentTroopCount(ally.data.GetCurrentTroopCount() + result);
            DamageGenerator.gen.CreatePopup(position, result.ToString(), Color.green);
        }

      
        //Display Data

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
    public override void Resolve()
    {

    }

    public override void EffectDestroyed()
    {
        //Nothing
    }
}
