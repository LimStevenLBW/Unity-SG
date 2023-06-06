using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsFooter : MonoBehaviour
{
    private UnitDataStore currentDisplay;

    public new TextMeshProUGUI name;
    public TextMeshProUGUI rank;
    public TextMeshProUGUI troops;
    public TextMeshProUGUI attack;
    public TextMeshProUGUI magic;
    public TextMeshProUGUI defense;

    public Image powIcon;
    public Image mgkIcon;
    public Image defIcon;
    public Image troopsIcon;

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
        magic.SetText(unit.GetCurrentMagic().ToString() + " MGK");
        defense.SetText(unit.GetCurrentDefense().ToString() + " DEF");

        powIcon.gameObject.SetActive(true);
        mgkIcon.gameObject.SetActive(true);
        defIcon.gameObject.SetActive(true);
        troopsIcon.gameObject.SetActive(true);
    }

    public void ResetText()
    {       
        name.SetText("");
        rank.SetText("");
        troops.SetText("");
        attack.SetText("");
        magic.SetText("");
        defense.SetText("");
        currentDisplay = null;
        if(powIcon != null) powIcon.gameObject.SetActive(false);
        if(mgkIcon != null) mgkIcon.gameObject.SetActive(false);
        if(defIcon != null) defIcon.gameObject.SetActive(false);
        if(troopsIcon != null) troopsIcon.gameObject.SetActive(false);
    }
}
