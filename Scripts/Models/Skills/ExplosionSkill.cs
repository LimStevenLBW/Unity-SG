using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 * Basic Ranged Magic
 * uses the isAttacking bool
 */
public class ExplosionSkill : Skill
{
    float staminaResult;
    UnitController enemyTarget;
    private AudioClip hitSFX;
    public ExplosionSkill()
    {
        maxRange = 5;
       // minRange = 3;
        effect = Resources.Load("Effects/CFX_Explosion_B_Smoke+Text") as GameObject;
        hitSFX = (AudioClip)Resources.Load("Sounds/undertale/impact big 2");
        skillName = "Explosion";
        description = "";

        baseCooldown = 9;
        currentCooldown = baseCooldown;
        baseStaminaCost = 25;
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
        double staminaResult = data.GetCurrentStamina() - currentStaminaCost;

        //If we have a target and that target is within range, continue
        enemyTarget = controller.path.GetNearestEnemy();
        if (enemyTarget == null) return false;
        if (controller.path.distanceToNearestEnemy > maxRange) return false;


        //If we have enough stamina and if it is off cooldown, check for a target
        if (staminaResult >= 0 && currentCooldown <= 0)
        {
            return true;
        }
        return false;

       
    }

    public override void DoSkill()
    {
        ResetCD();
        enemyTarget = null;

        enemyTarget = controller.path.GetNearestEnemy();

        if (enemyTarget == null) //Abort using the skill
        {
            controller.SetState("IDLE");
            return;
        }

        isRunning = true; // Indicate that the skill is calculating;
        staminaResult = data.GetCurrentStamina() - currentStaminaCost;
        data.SetCurrentStamina(staminaResult);

        //Have the unitcontroller play the attack animation
        controller.PlayAnim("isAttacking", .75f, this, enemyTarget.Location);
    
        //Reset the state, changed, we reset after the animation is done in coroutine instead
        //controller.SetState("IDLE");
    }

    //Plays after the animation timing
    public override void HandleAnimExtra()
    {
        Vector3 pos = enemyTarget.transform.position;
        pos.y += 8;
        enemyTarget.PlayEffect(effect, pos, 4);

        //Calculate the damage done
        CalculateDamage(controller.data, enemyTarget.data, false);
        Director.Instance.PlaySound(hitSFX);

        //Calculate the damage done to nearby allies of the enemy unit as well
        List<UnitController> nearbyEnemies = new List<UnitController>(enemyTarget.path.GetAllAdjacent(1, true));
        foreach (UnitController e in nearbyEnemies)
        {
            CalculateDamage(controller.data, e.data, true);
        }
        //play sound
    }

    /*
     * This is an area of effect spell, so it affects surrounding HexCells as well
     * if it is marked as splash damage, we'll apply the weaker numbers
     */
    public void CalculateDamage(UnitDataStore ally, UnitDataStore enemy, bool isSplash)
    {
        Color color = Color.white;
        Vector3 position = enemy.controller.transform.position;
        position.y += 10;
        position.x += (float)0.5;

        float lowerBound;
        float upperBound;
        //Base damage 
        if (!isSplash)
        {
            lowerBound = (ally.GetMaxTroopCount() / 5);
            upperBound = (ally.GetMaxTroopCount() / 3);
        }
        else
        {
            lowerBound = (ally.GetMaxTroopCount() / 10);
            upperBound = (ally.GetMaxTroopCount() / 7);
        }

        //Setup magic modifier
        float magicModifier = data.GetCurrentMagic() * 2;

        //Setup troop count modifier
        float tcCompareMult = (ally.GetCurrentTroopCount() - enemy.GetCurrentTroopCount()) * 0.05f;

        //Apply Modifiers
        lowerBound = lowerBound + magicModifier + tcCompareMult;
        upperBound = upperBound + magicModifier + tcCompareMult;

        int damageData = (int)UnityEngine.Random.Range(lowerBound, upperBound);
        
        //Apply Defense Reductions
        float defValueReduction = (enemy.GetCurrentDefense() / 200) + data.GetBaseDefReduction();
        damageData -= (int)(damageData * defValueReduction);

        if (damageData < 0) damageData = 0; //We don't go below zero

        float critValue = data.GetCurrentCrit() * 100;
        float critCheck = UnityEngine.Random.Range(0, 100);

        if (critCheck <= critValue)
        { //Successful crit

            damageData = (int)(damageData * 1.5f);
            enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);
            DamageGenerator.gen.CreatePopup(position, damageData.ToString() + "!", Color.red);
        }
        else
        { //Run normally

            enemy.SetCurrentTroopCount(enemy.GetCurrentTroopCount() - damageData);
            DamageGenerator.gen.CreatePopup(position, damageData.ToString(), color);
        }
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
        throw new NotImplementedException();
    }
}
