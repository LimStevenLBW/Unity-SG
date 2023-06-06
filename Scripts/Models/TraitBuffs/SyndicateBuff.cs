using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyndicateBuff : TraitBuff
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
        UnitDataStore data = controller.data;

        float stamina = data.GetCurrentStamina();
        int troops = data.GetCurrentTroopCount();
        if (traitLevel == 1) { 
            data.SetCurrentStamina(stamina + 10);
            data.SetCurrentTroopCount(troops + 10);
        }
        else if (traitLevel >= 2) {
            data.SetCurrentStamina(stamina + 20);
            data.SetCurrentTroopCount(troops + 20);
        }
        else if (traitLevel >= 3)
        {
            data.SetCurrentStamina(stamina + 30);
            data.SetCurrentTroopCount(troops + 30);
        }
        else if (traitLevel >= 4)
        {
            data.SetCurrentStamina(stamina + 40);
            data.SetCurrentTroopCount(troops + 40);
        }
    }
    public override void ApplyEffectOnCombatEnd(UnitManager manager, UnitController controller)
    {
        //Do nothing
    }


    public override void ApplyEffectOnDeath(UnitManager manager, UnitController controller)
    {
        // Do nothing
    }


    public override void ClearEffect(UnitManager manager, UnitController controller)
    {

    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Sturdy units recover 10 troops and stamina each round";
        else if (traitLevel == 2) return "Sturdy units recover 20 troops and stamina each round";
        else if (traitLevel == 3) return "Sturdy units recover 30 troops and stamina each round";
        else if (traitLevel == 4) return "Sturdy units recover 40 troops and stamina each round";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
