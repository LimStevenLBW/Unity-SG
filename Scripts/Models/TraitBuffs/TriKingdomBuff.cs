using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriKingdomBuff : TraitBuff
{
    public override void ApplyEffect(UnitManager manager, UnitController controller)
    {
        UnitDataStore data = controller.data;

        float crit = data.GetCurrentCrit();

        if (traitLevel == 1) data.SetCurrentCrit(crit + .1f);
        else if (traitLevel >= 2) data.SetCurrentCrit(crit + .2f);
        else if (traitLevel >= 3) data.SetCurrentCrit(crit + .3f);
        else if (traitLevel >= 4) data.SetCurrentCrit(crit + .4f);
        else if (traitLevel >= 5) data.SetCurrentCrit(crit + .5f);
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
        UnitDataStore data = controller.data;
        float crit = data.GetCurrentCrit();
        if (traitLevel == 1) data.SetCurrentCrit(crit - .1f);
        else if (traitLevel >= 2) data.SetCurrentCrit(crit - .2f);
        else if (traitLevel >= 3) data.SetCurrentCrit(crit - .3f);
        else if (traitLevel >= 4) data.SetCurrentCrit(crit - .4f);
        else if (traitLevel >= 5) data.SetCurrentCrit(crit - .5f);
    }

    
    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Your troops gain 10% crit";
        else if (traitLevel == 2) return "Your Troops gain 20% crit";
        else if (traitLevel == 3) return "Your Troops gain 30% crit";
        else if (traitLevel == 4) return "Your Troops gain 40% crit";
        else if (traitLevel == 5) return "Your Troops gain 50% crit";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
