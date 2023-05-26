using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyndicateBuff : TraitBuff
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

        float stamina = data.GetCurrentStamina();

        if (traitLevel == 1) data.SetCurrentStamina(stamina + 10);
        else if (traitLevel >= 2) data.SetCurrentStamina(stamina + 20);
        else if (traitLevel >= 2) data.SetCurrentStamina(stamina + 30);
        else if (traitLevel >= 2) data.SetCurrentStamina(stamina + 40);
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
        else if (traitLevel == 1) return "Your troops recover 10 stamina at the start of combat";
        else if (traitLevel == 2) return "Your troops recover 20 stamina at the start of combat";
        else if (traitLevel == 3) return "Your troops recover 30 stamina at the start of combat";
        else if (traitLevel == 4) return "Your troops recover 40 stamina at the start of combat";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
