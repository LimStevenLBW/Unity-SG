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

    private List<UnitTrait> traits = new List<UnitTrait>();
    private List<TraitBuffDataStore> traitBuffsList = new List<TraitBuffDataStore>();
    public void AddTraitsFrom(UnitDataStore data, int teamNum)
    {
        ResetTraitBuffs();
        traits.Add(data.unitClass);
        traits.Add(data.special);
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

    private void AddTraitBuff(UnitTrait trait, int counter)
    {
        TraitBuffDataStore prefab = null;

        //If it doesnt meet the minimum requirement, we create an inactive trait display
        if (counter < trait.requirementTiers[0])
        {
            if (team == 1) prefab = Instantiate(playerTraitBuffInactive);
            if (team == -1) prefab = Instantiate(enemyTraitBuffInactive);

            prefab.gameObject.transform.SetParent(gameObject.transform);
            prefab.gameObject.transform.SetAsLastSibling();
        }
        else if(counter >= trait.requirementTiers[0]){
            if (team == 1) prefab = Instantiate(playerTraitBuffActive);
            if (team == -1) prefab = Instantiate(enemyTraitBuffActive);
            
            prefab.gameObject.transform.SetParent(gameObject.transform);
            prefab.gameObject.transform.SetAsFirstSibling();
        }


        prefab.AddBuffComponent(trait, counter);
        prefab.UpdateDisplay();
        prefab.gameObject.transform.localScale = new Vector3(1, 1, 1);
        traitBuffsList.Add(prefab);
    }

    
    public void ApplyTraitBuffs()
    {

    }

    public void ResetTraitBuffs()
    {
        foreach(TraitBuffDataStore traitBuff in traitBuffsList)
        {
            Destroy(traitBuff.gameObject);
        }
        traitBuffsList.Clear();
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
