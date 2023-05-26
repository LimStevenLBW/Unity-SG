using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Effects tied to fulfilling team trait requirements
public abstract class TraitBuff
{
    public int traitLevel;

    public string effectText;

    public abstract void ApplyEffect(UnitManager manager, UnitController controller);
    public abstract void ApplyEffectOnDeath(UnitManager manager, UnitController controller);
    public abstract void ApplyEffectOnCombatEnd(UnitManager manager, UnitController controller);
    public abstract void ClearEffect(UnitManager manager, UnitController controller);

    public abstract void SetTraitLevel(int traitLevel);

    public abstract string GetEffectText();
}
