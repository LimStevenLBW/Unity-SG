using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TraitBuffDisplay : MonoBehaviour
{
    public TraitBuffDisplay prefab;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI requirement;
    [SerializeField] private TextMeshProUGUI traitName;
    [SerializeField] private TextMeshProUGUI traitEffect;

    private UnitTrait trait;

    public void UpdateTraitDisplay(UnitTrait trait, int counter)
    {
        this.trait = trait;
        icon.sprite = trait.icon;
        requirement.SetText(counter.ToString());
        traitName.SetText(trait.traitName);
        traitEffect.SetText(trait.effect);

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
