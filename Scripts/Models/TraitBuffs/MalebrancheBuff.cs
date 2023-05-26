using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalebrancheBuff : TraitBuff
{
    private bool isBuffApplied = false;
    private AudioClip buffSFX = (AudioClip)Resources.Load("Sounds/undertale/power up 2");

    public override void ApplyEffect(UnitManager manager, UnitController controller)
    {
        UnitDataStore data = controller.data;
        string factionName = data.faction.traitName;

        if (traitLevel == 1 && factionName == "Malebranche")
        {
            //buff the first controller processed
            if(isBuffApplied == false && controller.isJuggernaut == false)
            {
                Vector3 scale = controller.transform.localScale;
                controller.transform.localScale = new Vector3(scale.x + 4, scale.y + 4, scale.z + 4);

                float power = data.GetCurrentPower();
                float magic = data.GetCurrentMagic();
                float defense = data.GetCurrentDefense();
                data.SetCurrentPower(power + 25);
                data.SetCurrentMagic(magic + 25);
                data.SetCurrentDefense(defense + 25);

                controller.isJuggernaut = true;

                isBuffApplied = true;

                Vector3 position = controller.transform.position;
                position.y += 10;
                position.x += (float)0.5;
                DamageGenerator.gen.CreatePopup(position, "JUGGERNAUT", Color.red);
                Director.Instance.PlaySound(buffSFX);
            }
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
        isBuffApplied = false;
    }


    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "A malebranche becomes a juggernaut, boosting their POW, MGK DEF by 25";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
