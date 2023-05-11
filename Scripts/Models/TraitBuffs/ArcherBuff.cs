using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBuff : TraitBuff
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
        else if (traitLevel == 1) return "Menaces gain 50 extra troops"; if (traitLevel == 1) return "Archers gain 10 POW";
        else if (traitLevel == 1) return "Menaces gain 50 extra troops"; if (traitLevel == 2) return "Archers gain 35 POW";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
