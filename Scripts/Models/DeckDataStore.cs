using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDataStore
{
    private Deck deckBase;

    public List<UnitDataStore> unitList;
    public UnitDataStore captain;

    private int troopCount;

    public DeckDataStore(Deck deckBase)
    {
        unitList = new List<UnitDataStore>();
        this.deckBase = deckBase;


        //Cycle through and create our working deck
        foreach(Unit unit in deckBase.unitList)
        {
            if (unit == null) Debug.Log("null");   
            unitList.Add(new UnitDataStore(unit));
        }

        captain = new UnitDataStore(deckBase.captain);

        UpdateTroopCount();
    }

    void UpdateTroopCount()
    {
        troopCount = 0;
        foreach (UnitDataStore unit in unitList)
        {
            troopCount += unit.GetMaxTroopCount();

        }
    }
    public int GetTroopSize()
    {
        return troopCount;
    }

    public int GetCommanderCount()
    {
        return unitList.Count;
    }

}
