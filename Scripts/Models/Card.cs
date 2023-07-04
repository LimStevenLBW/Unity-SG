using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//A card's type depends on the datastore it owns. It only owns one datastore.
public class Card
{
    public UnitDataStore unit { get; private set; }
    public CantripDataStore cantrip { get; private set; }

    public string cardName { get; private set;}
    public string rank { get; private set; }
    public string cantripEffect { get; set; }
    public int cost { get; private set; }
    public Sprite classIcon { get; private set; } 

    //private RelicDataStore relic;

    public Card(UnitDataStore unit)
    {
        this.unit = unit;
        cardName = unit.GetName();
        rank = unit.GetRank();
        classIcon = unit.unitClass.icon;
        cost = 1;
    }
    public Card(CantripDataStore cantrip)
    {
        this.cantrip = cantrip;
        cardName = cantrip.GetName();
        cantripEffect = cantrip.GetEffectText();
        cost = cantrip.GetCost();
    }

    public bool IsUnitType()
    {
        if (unit != null) return true;
        else return false;
    }
    
    public bool IsCantripType()
    {
        if (cantrip != null) return true;
        else return false;
    }

    public UnitDataStore GetUnitDataStore()
    {
        return unit;
    }



}
