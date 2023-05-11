using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderworldBuff : TraitBuff
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
        else if (traitLevel == 1) return "Revive a random unit at the end of each combat with 1/2 of their troops";
        else if (traitLevel == 2) return "Revive a random unit at the end of each combat";
        else if (traitLevel == 3) return "Revive two random units at the end of each combat";


        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
