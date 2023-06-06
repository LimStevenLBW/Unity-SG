using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortraitRoom : MonoBehaviour
{
    UnitDataStore dataStore;
    UnitController model;
    public void UpdatePortrait(UnitDataStore dataStore)
    {
        //Debug.Log(dataStore.GetName() + " " + dataStore.faction.traitName + " " + dataStore.unitClass.traitName);
        if (this.dataStore != null) Destroy(model.gameObject);
        this.dataStore = dataStore;
        model = Instantiate(dataStore.prefab, transform);
        model.transform.SetParent(transform, false);
    }

    public void ClearRoom()
    {
        if (dataStore != null) Destroy(model.gameObject);
        dataStore = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Director.Instance.GetPhase() == "DEPLOYMENT" || Director.Instance.GetPhase() == "ENEMYDEPLOYMENT")
        {
            ClearRoom();
        }
    }
}
