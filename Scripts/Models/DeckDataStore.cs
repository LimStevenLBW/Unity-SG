using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDataStore
{
    private Deck deckBase;
    private List<Card> cardList;
   // private List<Card> benchedCardList;

    public List<UnitDataStore> unitList;
    public List<CantripDataStore> cantripList;
    //public List<UnitDataStore> benchedUnitList; //Units that are not currently deployed in the deck
    //public List<UnitDataStore> drawnList;
    public UnitDataStore captain;

    private int troopCount;

    public DeckDataStore(Deck deckBase)
    {
        this.deckBase = deckBase;

        unitList = new List<UnitDataStore>();

        cantripList = new List<CantripDataStore>();
       // benchedUnitList = new List<UnitDataStore>();
       // drawnList = new List<UnitDataStore>();

        //Cycle through and create our working deck
        foreach (Unit unit in deckBase.unitList) unitList.Add(new UnitDataStore(unit));
        foreach (Cantrip cantrip in deckBase.cantripList) cantripList.Add(new CantripDataStore(cantrip));

        //Sort our lists
        unitList.Sort();
        cantripList.Sort();
        /*
        foreach (Unit unit in deckBase.benchedUnitList)
        {
            if (unit == null) Debug.Log("null");

            benchedUnitList.Add(new UnitDataStore(unit));
        }
        */

        FormCardList();

        //captain = new UnitDataStore(deckBase.captain);
    }

    private void FormCardList()
    {
        cardList = new List<Card>();
        foreach (UnitDataStore data in unitList) cardList.Add(new Card(data));
        foreach (CantripDataStore data in cantripList) cardList.Add(new Card(data));
    }

    public void AddDrops(List<Unit> benchList)
    {
        foreach (Unit unit in benchList)
        {
            //benchedUnitList.Add(new UnitDataStore(unit));
        }

    }

    public bool IsEmpty()
    {
        if (unitList.Count == 0) return true;
        return false;
    }

    public void SortByClassAndRank()
    {
        unitList.Sort();
    }
    public void SortByRank()
    {
    //    foreach (UnitDataStore unit in unitList) unit.sortByRank = true;
        unitList.Sort();
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

     //   drawnList.Add(unit);
        return unit;
    }

    //Return cards that were drawn into the deck
    public void ReturnCardsToDeck()
    {
      //  foreach (UnitDataStore unit in drawnList) unitList.Add(unit);
    }

    public void UpdateTroopCount()
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

    public int GetDeckCount()
    {
        return unitList.Count;
    }

    public List<Card> GetCardList()
    {
        return cardList;
    }

}
