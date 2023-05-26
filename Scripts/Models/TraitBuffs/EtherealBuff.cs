using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherealBuff : TraitBuff
{


    public override void ApplyEffect(UnitManager manager, UnitController controller)
    {
        //Do nothing
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
        string className = data.unitClass.traitName;
        float power = data.GetCurrentPower();
        if (traitLevel == 1 && className == "Archer") data.SetCurrentPower(power - 10);
        else if (traitLevel >= 2 && className == "Archer") data.SetCurrentPower(power - 35);
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Ethereal units take severely increased damage from holy magic";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
