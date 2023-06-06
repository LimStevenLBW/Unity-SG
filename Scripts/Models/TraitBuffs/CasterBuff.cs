using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterBuff : TraitBuff
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
        float magic = data.GetCurrentMagic();

        if (traitLevel == 1 && className == "Caster") data.SetCurrentMagic(magic + 10);
        else if (traitLevel >= 2 && className == "Caster") data.SetCurrentMagic(magic + 25);
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
        float magic = data.GetCurrentMagic();
        if (traitLevel == 1 && className == "Caster") data.SetCurrentMagic(magic - 10);
        else if (traitLevel >= 2 && className == "Caster") data.SetCurrentMagic(magic - 25);
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Casters gain 10 MGK";
        else if (traitLevel == 2) return "Casters gain 25 MGK";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
