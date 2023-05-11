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

    public override void ApplyEffect()
    {
        throw new System.NotImplementedException();
    }

    public override string GetEffectText()
    {
        if (traitLevel == 0) return "";
        else if (traitLevel == 1) return "Your troops have 10% additional crit";
        else if (traitLevel == 2) return "Your troops have 20% additional crit";
        else if (traitLevel == 3) return "Your troops have 30% additional crit";
        else if (traitLevel == 4) return "Your troops have 40% additional crit";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
