using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenaceBuff : TraitBuff
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

        string specialName = data.special.traitName;
        float power = data.GetCurrentPower();

        if (traitLevel == 1 && specialName == "Menace") data.SetCurrentPower(power + 5);
        else if (traitLevel >= 2 && specialName == "Menace") data.SetCurrentPower(power + 10);
        else if (traitLevel >= 3 && specialName == "Menace") data.SetCurrentPower(power + 15);
        else if (traitLevel >= 4 && specialName == "Menace") data.SetCurrentPower(power + 20);

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
        // Do nothing
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if(traitLevel == 1) return "Menaces gain 5 POW each round";
        else if (traitLevel == 2) return "Menaces gain 10 POW each round";
        else if (traitLevel == 3) return "Menaces gain 15 POW each round";
        else if (traitLevel == 4) return "Menaces gain 20 POW each round";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
