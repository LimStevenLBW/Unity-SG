using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBuff : TraitBuff
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void ApplyEffect(UnitManager manager, UnitController controller)
    {
        //Do nothing
    }

    public override void ApplyEffectOnDeath(UnitManager manager, UnitController controller)
    {
        //Do nothing
    }

    public override void ApplyEffectOnCombatEnd(UnitManager manager, UnitController controller)
    {
        UnitDataStore data = controller.data;
       

        Vector3 position = controller.transform.position;
        position.y += 10;
        position.x += (float)0.5;

        int staminaHealing = (int)(data.GetMaxStamina() - data.GetCurrentStamina());
        data.SetCurrentStamina(data.GetMaxStamina());

        if(staminaHealing > 0) DamageGenerator.gen.CreatePopup(position, staminaHealing.ToString(), Color.magenta);
        
    }

    public override void ClearEffect(UnitManager manager, UnitController controller)
    {
        //Not needed here since its buff is applied once on unit death
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "At the end of combat, all units recover their stamina";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
