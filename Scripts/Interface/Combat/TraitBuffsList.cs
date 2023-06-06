using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitBuffsList : MonoBehaviour
{
    //DO NOT EDIT ANY DATA IN UNIT TRAITS

    [SerializeField] private TraitBuffDataStore enemyTraitBuffActive;
    [SerializeField] private TraitBuffDataStore enemyTraitBuffInactive;
    [SerializeField] private TraitBuffDataStore playerTraitBuffActive;
    [SerializeField] private TraitBuffDataStore playerTraitBuffInactive;

    public int team;

    private List<UnitTrait> traits = new List<UnitTrait>(); //Working list of total traits
    public List<TraitBuffDataStore> dataStoreList = new List<TraitBuffDataStore>();

    public void GetTraitsFrom(UnitDataStore data)
    {
        ClearTraitBuffs(false);
        traits.Add(data.unitClass);
        //traits.Add(data.special);
        traits.Add(data.faction);

        traits.Sort();

        UnitTrait workingTrait = null;
        int counter = 0;

        for (int i = 0; i < traits.Count; i++)
        {
            if (workingTrait == null)
            {
                workingTrait = traits[i];
                counter++;
            }
            else if (workingTrait.traitName == traits[i].traitName)
            {
                counter++;
            }

            //Found a new trait, start the counter over
            if (workingTrait.traitName != traits[i].traitName)
            {
                AddTraitBuff(workingTrait, counter);
                workingTrait = traits[i];
                counter = 1;
            }

            //If this is the last one, add the last trait
            if (i == traits.Count - 1)
            {
                AddTraitBuff(workingTrait, counter);
            }

        }
    }

    //If it doesnt meet the minimum requirement of the trait, we create an inactive trait display
    //If it does, we create an active display
    private void AddTraitBuff(UnitTrait trait, int counter)
    {
        TraitBuffDataStore dataStore = null;


        if (counter < trait.requirementTiers[0])
        {
            if (team == 1) dataStore = Instantiate(playerTraitBuffInactive);
            if (team == -1) dataStore = Instantiate(enemyTraitBuffInactive);

            dataStore.gameObject.transform.SetParent(gameObject.transform);
            dataStore.gameObject.transform.SetAsLastSibling();
        }
        else if(counter >= trait.requirementTiers[0]){
            if (team == 1) dataStore = Instantiate(playerTraitBuffActive);
            if (team == -1) dataStore = Instantiate(enemyTraitBuffActive);
            
            dataStore.gameObject.transform.SetParent(gameObject.transform);
            dataStore.gameObject.transform.SetAsFirstSibling();
        }


        dataStore.AddBuffComponent(trait, counter);
        dataStore.UpdateDisplay();
        dataStore.gameObject.transform.localScale = new Vector3(1, 1, 1);
        dataStoreList.Add(dataStore);
    }

    
    public List<TraitBuffDataStore> GetActiveBuffs()
    {
        List<TraitBuffDataStore> activeBuffs = new List<TraitBuffDataStore>();
        foreach (TraitBuffDataStore traitBuff in dataStoreList)
        {
            if (traitBuff.isActive) activeBuffs.Add(traitBuff);
        }

        return activeBuffs;
    }

    /*
     * when false, only the shown prefabs will be cleared
     * and the actual stored traits will remain intact
     */
    public void ClearTraitBuffs(bool fullReset)
    {
        foreach(TraitBuffDataStore traitBuff in dataStoreList)
        {
            Destroy(traitBuff.gameObject);
        }
        dataStoreList.Clear();

        if (fullReset) traits.Clear();
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
