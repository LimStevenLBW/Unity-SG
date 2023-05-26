using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryBuff : TraitBuff
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

        string className = data.unitClass.traitName;
        float power = data.GetCurrentPower();

        if (traitLevel == 1 && className == "Infantry") data.SetCurrentPower(power + 5);
        else if (traitLevel >= 2 && className == "Infantry") data.SetCurrentPower(power + 10);
        else if (traitLevel >= 3 && className == "Infantry") data.SetCurrentPower(power + 15);
        else if (traitLevel >= 4 && className == "Infantry") data.SetCurrentPower(power + 20);
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
        if (traitLevel == 1 && className == "Infantry") data.SetCurrentPower(power - 5);
        else if (traitLevel >= 2 && className == "Infantry") data.SetCurrentPower(power - 10);
        else if (traitLevel >= 2 && className == "Infantry") data.SetCurrentPower(power - 15);
        else if (traitLevel >= 2 && className == "Infantry") data.SetCurrentPower(power - 20);
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Infantry gain 5 POW";
        else if (traitLevel == 2) return "Infantry gain 10 POW";
        else if (traitLevel == 3) return "Infantry gain 15 POW";
        else if (traitLevel == 4) return "Infantry gain 20 POW";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
