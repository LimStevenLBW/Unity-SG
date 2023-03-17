using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitWindow : MonoBehaviour
{
    public GameObject troops;
    public GameObject stamina;
    public GameObject power;
    public GameObject armor;
    public GameObject speed;
    public GameObject crit;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI troopsText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI critText;

    public PortraitCamera portraitCamera;

    // Start is called before the first frame update
    public void Awake()
    {
        
    }

    public void SetValues(UnitController unitController)
    {
        Unit unit = unitController.unit;

        if (unit)
        {

            portraitCamera.SetTargetObject(unitController);

            nameText.SetText("" + unit.GetName());
            rankText.SetText("" + unit.GetRank() + " Rank Squad Leader");
            troopsText.SetText(unit.GetCurrentTroopCount() + " / " + unit.GetMaxTroopCount());
            staminaText.SetText(unit.GetCurrentStamina() + " / " + unit.GetMaxStamina());
            powerText.SetText("" + unit.GetCurrentPower());
            armorText.SetText("" + unit.GetCurrentArmor());
            speedText.SetText("" + unit.GetCurrentSpeed());
            critText.SetText("" + unit.GetCurrentCrit() + "%");
            
        }   
    }
    public void SetPosition(UnitController unitController)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitController.transform.position);
        screenPos.z = 0;

        transform.position = screenPos;
    }
}
