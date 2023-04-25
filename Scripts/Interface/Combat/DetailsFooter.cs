using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DetailsFooter : MonoBehaviour
{
    private UnitDataStore currentDisplay;

    public TextMeshProUGUI name;
    public TextMeshProUGUI troops;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI defense;

    // Start is called before the first frame update
    void Start()
    {
        
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
        troops.SetText(unit.GetMaxTroopCount().ToString() + " TROOPS");
        attack.SetText(unit.GetCurrentPower().ToString() + " POW");
        defense.SetText(unit.GetCurrentDefense().ToString() + " DEF");


    }
}
