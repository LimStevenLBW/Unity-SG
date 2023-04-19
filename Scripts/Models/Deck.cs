using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public List<UnitController> deckList;
    private int troopCount;


    public void Init()
    {
        troopCount = 0;
        foreach (UnitController unit in deckList)
        {
            unit.UpdateFields(); //Setup initial fields for unit
            troopCount += unit.data.GetBaseTroopCount(); //Currently using base for testing
            
        }

    }



    public int GetTroopSize()
    {
        return troopCount;
    }

    public int GetCommanderCount()
    {
        return deckList.Count;
    }
}
