using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegatariValleyBuff : TraitBuff
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

        string factionName = data.faction.traitName;
        int currentTc = data.GetCurrentTroopCount();

        if (traitLevel == 1 && factionName == "Vegatari") data.SetCurrentTroopCount(currentTc + 25);
        else if (traitLevel >= 2 && factionName == "Vegatari") data.SetCurrentTroopCount(currentTc + 50);
        else if (traitLevel >= 3 && factionName == "Vegatari") data.SetCurrentTroopCount(currentTc + 75);
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
        else if (traitLevel == 1) return "All troops recover 25 troops each round";
        else if (traitLevel == 2) return "All troops recover 50 troops each round";
        else if (traitLevel == 3) return "All troops recover 75 troops each round";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
