using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public List<Unit> unitList = new List<Unit>();
    public List<Cantrip> cantripList = new List<Cantrip>();
    //public List<Relic> relicList;

    //public List<Unit> benchedUnitList; //Units that are not currently deployed in the deck

    public Unit captain;

    //Create an instance of this scriptable object from given lists
    public static Deck CreateInstance(List<Unit> unitList, List<Cantrip> cantripList)
    {
        var data = CreateInstance<Deck>();
        data.Init(unitList, cantripList);
        return data;
    }

    //Converting back
    public static Deck CreateInstance(DeckDataStore deck)
    {
        var data = CreateInstance<Deck>();
        data.Init(deck);
        return data;
    }

    private void Init(List<Unit> unitList, List<Cantrip> cantripList)
    {
        this.unitList = unitList;
        this.cantripList = cantripList;

        //benchedUnitList = new List<Unit>();
    }

    //Converting back from DeckDataStore
    private void Init(DeckDataStore deck)
    {
        unitList = new List<Unit>();
        //benchedUnitList = new List<Unit>();

        foreach(UnitDataStore unit in deck.unitList)
        {
            unitList.Add(unit.unitBase);
        }

        /*
        foreach (UnitDataStore unit in deck.benchedUnitList)
        {
            benchedUnitList.Add(unit.unitBase);
        }
        */
    }
}
