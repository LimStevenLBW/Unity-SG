using Microlight.MicroBar;
using TMPro;
using UnityEngine;

public class UnitWindow : MonoBehaviour
{
    UnitDataStore data;
    public GameObject troops;
    public GameObject stamina;
    public GameObject power;
    public GameObject magic;
    public GameObject defense;
    public GameObject speed;
    public GameObject crit;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI troopsText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI magicText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI critText;

    [SerializeField] MicroBar _tcBar;
    [SerializeField] MicroBar _spBar;

    public PortraitCamera portraitCamera;

    // Start is called before the first frame update
    public void Awake()
    {     
    }

    public void LateUpdate()
    {

        //Only when bars are changed
        if (data != null && data.barWasUpdated)
        {
            UpdateBarValues(data);
            data.barWasUpdated = false;
        }
        

        //Pretty much always do
        if(data != null) UpdateTargetValues(data);
    }

    public void SetValues(UnitController unitController)
    {
        data = unitController.data;
        data.barWasUpdated = false;

        portraitCamera.SetTargetObject(unitController);

        nameText.SetText("" + data.GetName());
        rankText.SetText("" + data.GetRank() + " Rank Squad Leader");

        //Initialize HP Bars
        if (_tcBar != null) _tcBar.Initialize(data.GetMaxTroopCount());
        if (_spBar != null) _spBar.Initialize(data.GetMaxStamina());

        UpdateTargetValues(data);
        UpdateBarValues(data);
    }

    public void UpdateTargetValues(UnitDataStore data)
    {
        powerText.SetText("" + data.GetCurrentPower());
        magicText.SetText("" + data.GetCurrentMagic());
        defenseText.SetText("" + data.GetCurrentDefense());
        speedText.SetText("" + data.GetCurrentSpeed());
        critText.SetText("" + data.GetCurrentCrit() + "%");
    }

    public void UpdateBarValues(UnitDataStore data)
    {
        if (_tcBar != null)
        {
            _tcBar.UpdateHealthBar(data.GetCurrentTroopCount());
            //_tcBar.SetMaxHealth(data.GetMaxTroopCount());
            troopsText.SetText(data.GetCurrentTroopCount() + " / " + data.GetMaxTroopCount());
        }

        if (_spBar != null)
        {
            _spBar.UpdateHealthBar((float)data.GetCurrentStamina());
           // _spBar.SetMaxHealth((float)data.GetMaxStamina());

            staminaText.SetText(data.GetCurrentStamina() + " / " + data.GetMaxStamina());
        }
    }

    public void SetPosition(UnitController unitController)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitController.transform.position);
        screenPos.z = 0;

        transform.position = screenPos;
    }
}
