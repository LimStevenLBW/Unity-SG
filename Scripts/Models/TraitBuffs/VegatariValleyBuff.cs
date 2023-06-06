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
        /*
        UnitDataStore data = controller.data;

        string factionName = data.faction.traitName;
        int currentTc = data.GetCurrentTroopCount();

        if (traitLevel == 1 && factionName == "Vegatari") data.SetCurrentTroopCount(currentTc + 25);
        else if (traitLevel >= 2 && factionName == "Vegatari") data.SetCurrentTroopCount(currentTc + 50);
        else if (traitLevel >= 3 && factionName == "Vegatari") data.SetCurrentTroopCount(currentTc + 75);
        */
    }
    public override void ApplyEffectOnCombatEnd(UnitManager manager, UnitController controller)
    {
        //Do nothing
    }


    public override void ApplyEffectOnDeath(UnitManager manager, UnitController controller)
    {
        UnitDataStore data = controller.data;

        if (data.faction.traitName != "Vegatari") return;

        List<UnitController> allies = controller.GetAllies();
        foreach (UnitController ally in allies)
        {
            int healingAmount = 0;
            int currentTroops = ally.data.GetCurrentTroopCount();
            if (traitLevel == 0) return;
            else if (traitLevel == 1) healingAmount = 35;
            else if (traitLevel == 2) healingAmount = 50;
            else if (traitLevel == 3) healingAmount = 75;

            //Apply the heal to all allies
            ally.data.SetCurrentTroopCount(currentTroops + healingAmount);

            Vector3 position = ally.transform.position;
            position.y += 10;
            position.x += (float)0.5;
            DamageGenerator.gen.CreatePopup(position, healingAmount.ToString(), Color.green);
        }
    }


    public override void ClearEffect(UnitManager manager, UnitController controller)
    {

    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "When a Vegatari dies, its allies heal for 35 troops";
        else if (traitLevel == 2) return "When a Vegatari dies, its allies heal for 50 troops";
        else if (traitLevel == 2) return "When a Vegatari dies, its allies heal for 75 troops";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
