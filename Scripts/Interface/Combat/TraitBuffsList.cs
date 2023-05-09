using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraitBuffsList : MonoBehaviour
{
    //DO NOT EDIT ANY DATA IN UNIT TRAITS

    [SerializeField] private TraitBuffDisplay enemyTraitBuffActive;
    [SerializeField] private TraitBuffDisplay enemyTraitBuffInactive;
    [SerializeField] private TraitBuffDisplay playerTraitBuffActive;
    [SerializeField] private TraitBuffDisplay playerTraitBuffInactive;

    public int team;

    private List<UnitTrait> traits = new List<UnitTrait>();
    private List<UnitTrait> displayedTraits = new List<UnitTrait>();
    private List<TraitBuffDisplay> traitBuffs = new List<TraitBuffDisplay>();
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
        TraitBuffDisplay prefab = null;

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

        prefab.UpdateTraitDisplay(trait, counter);
        prefab.gameObject.transform.localScale = new Vector3(1, 1, 1);

        traitBuffs.Add(prefab);
    }

    public void ResetTraitBuffs()
    {
        foreach(TraitBuffDisplay traitBuff in traitBuffs)
        {
            Destroy(traitBuff.gameObject);
        }
        traitBuffs.Clear();
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
