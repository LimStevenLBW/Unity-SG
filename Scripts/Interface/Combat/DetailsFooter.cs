using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailsFooter : MonoBehaviour
{
    private UnitDataStore currentDisplay;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI rank;
    public TextMeshProUGUI troops;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;

    // Start is called before the first frame update
    void Start()
    {
        ResetText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateData(UnitDataStore unit)
    {
        if (unit == null) { Debug.Log("Null"); return; }
        if (currentDisplay == unit) return;
        currentDisplay = unit;

        name.SetText(unit.GetName());
        rank.SetText(unit.GetRank() + " RANK");
        troops.SetText(unit.GetMaxTroopCount().ToString() + " TROOPS");
        attack.SetText(unit.GetCurrentPower().ToString() + " POW");
        defense.SetText(unit.GetCurrentDefense().ToString() + " DEF");


    }

    public void ResetText()
    {
        name.SetText("");
        rank.SetText("");
        troops.SetText("");
        attack.SetText("");
        defense.SetText("");
        currentDisplay = null;
    }
}
