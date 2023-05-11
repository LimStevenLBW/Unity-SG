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

    public override void ApplyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Start combat with an extra MushNub";
        else if (traitLevel == 2) return "Start combat with an evolved MushNub";
        else if (traitLevel == 3) return "Start combat with a MushNub Hulk";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
