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
    private int counter;
    private int traitLevel;

    public void AddBuffComponent(UnitTrait trait, int counter)
    {
        this.trait = trait;
        this.counter = counter;
        int ID = trait.traitBuff_ID;
        switch (ID)
        {
            case 1: traitBuff = gameObject.AddComponent<TriKingdomBuff>(); break;
            case 2: traitBuff = gameObject.AddComponent<UnderworldBuff>(); break;
            case 3: traitBuff = gameObject.AddComponent<VegatariValleyBuff>(); break;
            case 4: traitBuff = gameObject.AddComponent<SyndicateBuff>(); break;
            case 5: traitBuff = gameObject.AddComponent<MalebrancheBuff>(); break;

            case 6: traitBuff = gameObject.AddComponent<ArcherBuff>(); break;
            case 7: traitBuff = gameObject.AddComponent<AssassinBuff>(); break;
            case 8: traitBuff = gameObject.AddComponent<CasterBuff>(); break;
            case 9: traitBuff = gameObject.AddComponent<GuardianBuff>(); break;
            case 10: traitBuff = gameObject.AddComponent<HealerBuff>(); break;
            case 11: traitBuff = gameObject.AddComponent<InfantryBuff>(); break;
            case 12: traitBuff = gameObject.AddComponent<MenaceBuff>(); break;

            case 100: traitBuff = gameObject.AddComponent<DemonBuff>(); break;
            case 101: traitBuff = gameObject.AddComponent<EdibleBuff>(); break;
            case 102: traitBuff = gameObject.AddComponent<EliteBuff>(); break;
            case 103: traitBuff = gameObject.AddComponent<EtherealBuff>(); break;
            case 104: traitBuff = gameObject.AddComponent<NuggetBuff>(); break;
            case 105: traitBuff = gameObject.AddComponent<GoonBuff>(); break;
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

        if (traitBuff)
        {
            traitBuff.SetTraitLevel(traitLevel);
            traitEffect.SetText(traitBuff.GetEffectText());
        }

        

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
