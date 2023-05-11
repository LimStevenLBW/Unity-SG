using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfantryBuff : TraitBuff
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
        else if (traitLevel == 1) return "Infantry gain 5 POW";
        else if (traitLevel == 2) return "Infantry gain 10 POW";
        else if (traitLevel == 2) return "Infantry gain 15 POW";
        else if (traitLevel == 2) return "Infantry gain 20 POW";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
