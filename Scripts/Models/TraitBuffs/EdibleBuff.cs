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

    public override void ApplyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "When an Edible dies, its allies heal for 10% of its max troop count";
        else if (traitLevel == 2) return "When an Edible dies, its allies heal for 25% of its max troop count";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
