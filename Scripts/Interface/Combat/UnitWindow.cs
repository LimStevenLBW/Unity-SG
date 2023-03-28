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
        UnitDataStore data = unitController.data;

        portraitCamera.SetTargetObject(unitController);

        nameText.SetText("" + data.GetName());
        rankText.SetText("" + data.GetRank() + " Rank Squad Leader");
        troopsText.SetText(data.GetCurrentTroopCount() + " / " + data.GetMaxTroopCount());
        staminaText.SetText(data.GetCurrentStamina() + " / " + data.GetMaxStamina());
        powerText.SetText("" + data.GetCurrentPower());
        armorText.SetText("" + data.GetCurrentArmor());
        speedText.SetText("" + data.GetCurrentSpeed());
        critText.SetText("" + data.GetCurrentCrit() + "%");
            
          
    }
    public void SetPosition(UnitController unitController)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitController.transform.position);
        screenPos.z = 0;

        transform.position = screenPos;
    }
}
