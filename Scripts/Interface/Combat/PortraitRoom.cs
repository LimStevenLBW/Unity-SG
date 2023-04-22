using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitRoom : MonoBehaviour
{
    UnitDataStore unit;
    UnitController model;
    public void UpdatePortrait(UnitDataStore unit)
    {
        if (this.unit != null) Destroy(model);
        this.unit = unit;
        model = Instantiate(unit.controller.prefab, transform);
        model.transform.SetParent(transform, false);
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
