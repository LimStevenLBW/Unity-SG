using Microlight.MicroBar;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MicroBarFollow : MonoBehaviour
{
    public MicroBarFollow prefab;

    [SerializeField] MicroBar _tcBar;
    [SerializeField] MicroBar _spBar;
    //public TextMeshProUGUI troopsText;
    //public TextMeshProUGUI staminaText;

    private UnitDataStore data;
    private UnitController unitController;

    public void Update()
    {
        if (unitController) transform.position = new Vector3(unitController.Location.transform.position.x, transform.position.y, unitController.transform.position.z);
    }
    public void LateUpdate()
    {
      

        //Only when bars are changed
        if (data != null && data.barWasUpdated)
        {
            UpdateBarValues(data);
            data.barWasUpdated = false;
        }

    }

    public void Initialize(UnitController unitController)
    {
        data = unitController.data;
        this.unitController = unitController;
        if (_tcBar != null) _tcBar.Initialize(data.GetMaxTroopCount());
        if (_spBar != null) _spBar.Initialize(data.GetMaxStamina());
        UpdateBarValues(data);

        BoxCollider box = unitController.GetComponent<BoxCollider>();
        Vector3 boxSizeConverted = unitController.transform.TransformVector(box.size);

        //Place the bar above the unit controller's head
        transform.position = new Vector3(unitController.Location.transform.position.x, boxSizeConverted.y+5, unitController.transform.position.z);
        
    }

    public void UpdateBarValues(UnitDataStore data)
    {
        if (_tcBar != null)
        {
            _tcBar.UpdateHealthBar(data.GetCurrentTroopCount());
            //_tcBar.SetMaxHealth(data.GetMaxTroopCount());
            //troopsText.SetText(data.GetCurrentTroopCount() + " / " + data.GetMaxTroopCount());
        }

        if (_spBar != null)
        {
            _spBar.UpdateHealthBar((float)data.GetCurrentStamina());
            // _spBar.SetMaxHealth((float)data.GetMaxStamina());

            //staminaText.SetText(data.GetCurrentStamina() + " / " + data.GetMaxStamina());
        }
    }
}
