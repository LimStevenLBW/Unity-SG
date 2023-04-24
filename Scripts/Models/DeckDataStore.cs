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

        Shuffle();
        UpdateTroopCount();
    }

    public bool IsEmpty()
    {
        if (unitList.Count == 0) return true;
        return false;
    }

    //Fisher yates shuffle
    public void Shuffle()
    {
        System.Random _random = new System.Random();
        UnitDataStore temp;

        int n = unitList.Count;
        for(int i = 0; i < n; i++)
        {
            int random = i + (int)(_random.NextDouble() * (n - i));
            temp = unitList[random];
            unitList[random] = unitList[i];
            unitList[i] = temp;
        }
    }

    public UnitDataStore DrawCard()
    {
        int n = unitList.Count;
        UnitDataStore unit = unitList[n - 1];
        unitList.RemoveAt(n - 1);
        return unit;
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
