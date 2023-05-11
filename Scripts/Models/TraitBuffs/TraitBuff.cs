using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Effects tied to fulfilling team trait requirements
public abstract class TraitBuff: MonoBehaviour
{
    public int traitLevel;

    public string effectText;


    public abstract void ApplyEffect();

    public abstract void SetTraitLevel(int traitLevel);

    public abstract string GetEffectText();
}
