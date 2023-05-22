using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalebrancheBuff : TraitBuff
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
        else if (traitLevel == 1) return "An inferno sweeps the field when combat ends, damaging all enemies";
        else if (traitLevel == 2) return "An inferno sweeps the field when combat starts, damaging all enemies";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}