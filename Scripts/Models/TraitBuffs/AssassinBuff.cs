using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinBuff : TraitBuff
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

        if (traitLevel >= 2 && className == "Assassin") data.SetCurrentPower(power + 10);
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
        if (traitLevel >= 2 && className == "Assassin") data.SetCurrentPower(power - 10);
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Assassins are evasive and hard to touch";
        else if (traitLevel == 2) return "Assassins gain 10 POW";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
