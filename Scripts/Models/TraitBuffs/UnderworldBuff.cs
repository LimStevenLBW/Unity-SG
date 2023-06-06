using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderworldBuff : TraitBuff
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

        if (traitLevel == 1 && className == "Archer") data.SetCurrentPower(power + 10);
        else if (traitLevel >= 2 && className == "Archer") data.SetCurrentPower(power + 35);
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
        else if (traitLevel == 1) return "No Effect";
        else if (traitLevel == 2) return "Revive a random unit at the end of each combat";
        else if (traitLevel == 3) return "Revive two random units at the end of each combat";


        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
