using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    private List<Card> cardList;

    public List<Unit> unitList;
    public List<Cantrip> cantripList;
    //public List<Relic> relicList;

    public List<Unit> benchedUnitList; //Units that are not currently deployed in the deck

    public Unit captain;

    private void Init(List<Unit> unitList)
    {
        this.unitList = unitList;
        benchedUnitList = new List<Unit>();
    }

    //Converting back from DeckDataStore
    private void Init(DeckDataStore deck)
    {
        unitList = new List<Unit>();
        benchedUnitList = new List<Unit>();

        foreach(UnitDataStore unit in deck.unitList)
        {
            unitList.Add(unit.unitBase);
        }

        foreach (UnitDataStore unit in deck.benchedUnitList)
        {
            benchedUnitList.Add(unit.unitBase);
        }
    }

    public static Deck CreateInstance(List<Unit> unitList)
    {
        var data = ScriptableObject.CreateInstance<Deck>();
        data.Init(unitList);
        return data;
    }

    //Converting back
    public static Deck CreateInstance(DeckDataStore deck)
    {
        var data = ScriptableObject.CreateInstance<Deck>();
        data.Init(deck);
        return data;
    }
}
