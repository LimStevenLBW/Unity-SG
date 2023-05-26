using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitBuffDataStore : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI requirement;
    [SerializeField] private TextMeshProUGUI traitName;
    [SerializeField] private TextMeshProUGUI traitEffect;

    private UnitTrait trait;
    private TraitBuff traitBuff;
    public bool isActive = false;
    private int counter;
    private int traitLevel;

    //Get the ID from unit trait to find the corresponding definition
    public void AddBuffComponent(UnitTrait trait, int counter)
    {
        this.trait = trait;
        this.counter = counter;
        int ID = trait.traitBuff_ID;
        switch (ID)
        {
            case 1: traitBuff = new TriKingdomBuff(); break;
            case 2: traitBuff = new UnderworldBuff(); break;
            case 3: traitBuff = new VegatariValleyBuff(); break;
            case 4: traitBuff = new SyndicateBuff(); break;
            case 5: traitBuff = new MalebrancheBuff(); break;

            case 6: traitBuff = new ArcherBuff(); break;
            case 7: traitBuff = new AssassinBuff(); break;
            case 8: traitBuff = new CasterBuff(); break;
            case 9: traitBuff = new GuardianBuff(); break;
            case 10: traitBuff = new HealerBuff(); break;
            case 11: traitBuff = new InfantryBuff(); break;
            case 12: traitBuff = new MenaceBuff(); break;

            case 100: traitBuff = new DemonBuff(); break;
            case 101: traitBuff = new EdibleBuff(); break;
            case 102: traitBuff = new EliteBuff(); break;
            case 103: traitBuff = new EtherealBuff(); break;
            case 104: traitBuff = new NuggetBuff(); break;
            case 105: traitBuff = new GoonBuff(); break;
        }
    }

    public void UpdateDisplay()
    {
        icon.sprite = trait.icon;
        traitName.SetText(trait.traitName);

        traitLevel = 0;
        foreach(int i in trait.requirementTiers)
        {
            requirement.SetText(counter.ToString() + "/" + i);
            if (counter >= i) traitLevel++;
            if (counter < i) break;
        }

        if (traitBuff != null)
        {
            traitBuff.SetTraitLevel(traitLevel);
            traitEffect.SetText(traitBuff.GetEffectText());
        }

        if (traitLevel > 0) isActive = true;

    }

    public void Apply(UnitManager manager, UnitController controller)
    {
        if (traitBuff != null) traitBuff.ApplyEffect(manager, controller);
    }
    public void ApplyEffectOnCombatEnd(UnitManager manager, UnitController controller)
    {
        if (traitBuff != null) traitBuff.ApplyEffectOnCombatEnd(manager, controller);
    }

    public void ApplyDeathEffect(UnitManager manager, UnitController controller)
    {
        if (traitBuff != null) traitBuff.ApplyEffectOnDeath(manager, controller);
    }

    public void ClearEffect(UnitManager manager, UnitController controller)
    {
        if (traitBuff != null) traitBuff.ClearEffect(manager, controller);
    }



}
