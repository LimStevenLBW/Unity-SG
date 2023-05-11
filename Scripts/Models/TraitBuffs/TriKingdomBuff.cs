using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriKingdomBuff : TraitBuff
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
        else if (traitLevel == 1) return "Troops start combat with 20% fervor";
        else if (traitLevel == 2) return "Troops start combat with 30% fervor";
        else if (traitLevel == 3) return "Troops start combat with 40% fervor";
        else if (traitLevel == 4) return "Troops start combat with 50% fervor";
        else if (traitLevel == 5) return "Troops start combat with 60% fervor";

        return effectText;
    }

    public override void SetTraitLevel(int traitLevel)
    {
        this.traitLevel = traitLevel;
    }

}
