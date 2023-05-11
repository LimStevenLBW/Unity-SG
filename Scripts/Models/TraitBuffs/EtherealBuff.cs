using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherealBuff : TraitBuff
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
        else if (traitLevel == 1) return "Ethereal units take severely increased damage from holy magic";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
