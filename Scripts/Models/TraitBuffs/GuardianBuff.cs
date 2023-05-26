using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianBuff : TraitBuff
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
        float defense = data.GetCurrentDefense();

        if (traitLevel == 1 && className == "Guardian") data.SetCurrentDefense(defense + 10);
        else if (traitLevel >= 2 && className == "Guardian") data.SetCurrentDefense(defense + 20);
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
        float defense = data.GetCurrentDefense();
        if (traitLevel == 1 && className == "Guardian") data.SetCurrentDefense(defense - 10);
        else if (traitLevel >= 2 && className == "Guardian") data.SetCurrentDefense(defense - 20);
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Guardians gain 10 DEF";
        else if (traitLevel == 2) return "Guardians gain 20 DEF";
        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
