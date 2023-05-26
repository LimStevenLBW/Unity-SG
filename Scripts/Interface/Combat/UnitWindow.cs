using Microlight.MicroBar;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] UnitSkillDisplay _skill1;
    [SerializeField] UnitSkillDisplay _skill2;
    [SerializeField] UnitSkillDisplay _skill3;
    [SerializeField] UnitSkillDisplay _skill4;

    [SerializeField] Image unitClassImage;
    [SerializeField] Image unitFactionImage;
    [SerializeField] Image unitSpecialImage;
    [SerializeField] TextMeshProUGUI traitsText;

    public PortraitCamera portraitCamera;

    // Start is called before the first frame update
    public void Awake()
    {     
    }

    public void LateUpdate()
    {
        /*
        //Only when bars are changed
        if (data != null && data.barWasUpdated)
        {
            UpdateBarValues(data);
            data.barWasUpdated = false;
        }
        
    */
        //Pretty much always do
        if(data != null) UpdateTargetValues(data);
    }

    public void Initialize(UnitController unitController)
    {
        data = unitController.data;
        //data.barWasUpdated = false;

        portraitCamera.SetTargetObject(unitController);

        nameText.SetText("" + data.GetName());
        rankText.SetText("" + data.GetRank() + " Rank Squad Leader");

        //Initialize HP Bars
        if (_tcBar != null) _tcBar.Initialize(data.GetMaxTroopCount());
        if (_spBar != null) _spBar.Initialize(data.GetMaxStamina());
        if (_skill1 != null) _skill1.Initialize(data.skill1);
        if (_skill2 != null) _skill2.Initialize(data.skill2);
        if (_skill3 != null) _skill3.Initialize(data.skill3);
        if (_skill4 != null) _skill4.Initialize(data.skill4);

        UpdateTargetValues(data);
        UpdateBarValues(data);
        if( data!= null) data.OnBarUpdated += UpdateBarValues;
    }

    public void UpdateTargetValues(UnitDataStore data)
    {
        powerText.SetText("" + data.GetCurrentPower());
        magicText.SetText("" + data.GetCurrentMagic());
        defenseText.SetText("" + data.GetCurrentDefense());
        speedText.SetText("" + data.GetCurrentSpeed());
        critText.SetText("" + data.GetCurrentCrit() * 100 + "%");

        string traits = "";

        if (data.faction) {
            unitFactionImage.sprite = data.faction.icon;
            traits += (" " + data.faction.name);
        }
        else
        {
            unitFactionImage.gameObject.SetActive(false);
        }
        if (data.special) {
            unitSpecialImage.sprite = data.special.icon;
            traits += (" " + data.special.name);
        }
        else
        {
            unitSpecialImage.gameObject.SetActive(false);
        }
        if (data.unitClass)
        {
            unitClassImage.sprite = data.unitClass.icon;
            traits += (" " + data.unitClass.name);
        }
        else
        {
            unitClassImage.gameObject.SetActive(false);
        }
        traitsText.SetText(traits);
        // Data gets skill information late, so we initialize when it is available.

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
            _spBar.UpdateHealthBar(data.GetCurrentStamina());
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

    void OnDisable()
    {
        if(data != null) data.OnBarUpdated -= UpdateBarValues;
    }
}
