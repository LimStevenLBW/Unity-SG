using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdibleBuff : TraitBuff
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
      //Do nothing
    }
    public override void ApplyEffectOnCombatEnd(UnitManager manager, UnitController controller)
    {
      //Do nothing
    }

    public override void ApplyEffectOnDeath(UnitManager manager, UnitController controller)
    {
        UnitDataStore data = controller.data;

        if (data.special.traitName != "Edible") return;

        List<UnitController> allies = controller.GetAllies();
        foreach (UnitController ally in allies)
        {
            int healingAmount = 0;
            int currentTroops = ally.data.GetCurrentTroopCount();
            if (traitLevel == 0) return;
            else if (traitLevel == 1) healingAmount = 50;
            else if (traitLevel == 2) healingAmount = 100;

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
        //Not needed here since its buff is applied once on unit death
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "When an Edible dies, its allies heal for 50 troops";
        else if (traitLevel == 2) return "When an Edible dies, its allies heal for 100 troops";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
