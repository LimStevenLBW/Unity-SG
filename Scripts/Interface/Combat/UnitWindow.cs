using Microlight.MicroBar;
using TMPro;
using UnityEngine;

public class UnitWindow : MonoBehaviour
{
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
        //Start listening for object changes

        //Initialize HP Bars
        if (_tcBar != null) _tcBar.Initialize(100);
        if (_spBar != null) _spBar.Initialize(100);
    }

    public void SetValues(UnitController unitController)
    {
        UnitDataStore data = unitController.data;

        portraitCamera.SetTargetObject(unitController);

        nameText.SetText("" + data.GetName());
        rankText.SetText("" + data.GetRank() + " Rank Squad Leader");
        
        UpdateTargetValues(data);
    }

    public void UpdateTargetValues(UnitDataStore data)
    {
        powerText.SetText("" + data.GetCurrentPower());
        magicText.SetText("" + data.GetCurrentMagic());
        defenseText.SetText("" + data.GetCurrentDefense());
        speedText.SetText("" + data.GetCurrentSpeed());
        critText.SetText("" + data.GetCurrentCrit() + "%");

        if (_tcBar != null) _tcBar.UpdateHealthBar(data.GetCurrentTroopCount());
        troopsText.SetText(data.GetCurrentTroopCount() + " / " + data.GetMaxTroopCount());
        _tcBar.SetMaxHealth(data.GetMaxTroopCount());

        if (_spBar != null) _spBar.UpdateHealthBar((float)data.GetCurrentStamina());
        staminaText.SetText(data.GetCurrentStamina() + " / " + data.GetMaxStamina());
        _spBar.SetMaxHealth((float)data.GetMaxStamina());
    }

    public void SetPosition(UnitController unitController)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(unitController.transform.position);
        screenPos.z = 0;

        transform.position = screenPos;
    }
}
