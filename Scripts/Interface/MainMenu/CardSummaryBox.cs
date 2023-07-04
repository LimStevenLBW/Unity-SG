using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardSummaryBox : MonoBehaviour
{
    [SerializeField] Image cardTypeBG;
    [SerializeField] TextMeshProUGUI cardType;
    [SerializeField] Image cardNameBG;
    [SerializeField] TextMeshProUGUI cardName;

    [SerializeField] GameObject unitLayout;
    private UnitDataStore unitData;
    [SerializeField] TextMeshProUGUI cantripEffect;
    [SerializeField] TextMeshProUGUI cost;

    [SerializeField] TextMeshProUGUI POW;
    [SerializeField] TextMeshProUGUI MGK;
    [SerializeField] TextMeshProUGUI DEF;
    [SerializeField] TextMeshProUGUI HP;
    [SerializeField] TextMeshProUGUI SP;


    public void Display(Card card, Color32 elementColor)
    {
        gameObject.SetActive(true);
        if (card.IsUnitType())
        {
            cardTypeBG.color = elementColor;
            cardNameBG.color = elementColor;

            unitLayout.SetActive(true);
            cantripEffect.gameObject.SetActive(false);
            cardType.SetText("Guild Member");
            cardName.SetText(card.cardName);
            cost.SetText("Cost: " + card.cost.ToString());

            unitData = card.GetUnitDataStore();
            POW.SetText("POW: " + unitData.GetPower().ToString());
            MGK.SetText("MGK: " + unitData.GetMagic().ToString());
            DEF.SetText("DEF: " + unitData.GetDefense().ToString());
            HP.SetText("HP: " + unitData.GetMaxTroopCount().ToString());
            SP.SetText("SP: " + unitData.GetMaxStamina().ToString());

        }
        else if (card.IsCantripType())
        {
            cardTypeBG.color = elementColor;
            cardNameBG.color = elementColor;
            unitLayout.SetActive(false);
            cantripEffect.gameObject.SetActive(true);
            cardType.SetText("Cantrip");
            cardName.SetText(card.cardName);
            cantripEffect.SetText(card.cantripEffect);

            cost.SetText("Cost: " + card.cost.ToString());
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
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
