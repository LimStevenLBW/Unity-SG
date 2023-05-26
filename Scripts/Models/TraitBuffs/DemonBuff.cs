using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBuff : TraitBuff
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

        string traitName = data.special.traitName;
        float power = data.GetCurrentPower();
        float magic = data.GetCurrentMagic();
        float defense = data.GetCurrentDefense();

        if (traitLevel == 1 && traitName == "Demon")
        {
            data.SetCurrentPower(power + 4);
            data.SetCurrentMagic(magic + 4);
            data.SetCurrentDefense(defense + 4);
        }
        else if (traitLevel >= 2 && traitName == "Demon")
        {
            data.SetCurrentPower(power + 6);
            data.SetCurrentMagic(magic + 6);
            data.SetCurrentDefense(defense + 6);
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
        UnitDataStore data = controller.data;

        string traitName = data.faction.traitName;
        float power = data.GetCurrentPower();
        float magic = data.GetCurrentMagic();
        float defense = data.GetCurrentDefense();

        if (traitLevel == 1 && traitName == "Demon")
        {
            data.SetCurrentPower(power - 4);
            data.SetCurrentMagic(magic - 4);
            data.SetCurrentDefense(defense - 4);
        }
        else if (traitLevel >= 2 && traitName == "Demon")
        {
            data.SetCurrentPower(power - 6);
            data.SetCurrentMagic(magic - 6);
            data.SetCurrentDefense(defense - 6);
        }
    }

    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "All Demons gain 4 POW, MGK, and DEF";
        else if (traitLevel == 2) return "All Demons gain 6 POW, MGK, and DEF";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
