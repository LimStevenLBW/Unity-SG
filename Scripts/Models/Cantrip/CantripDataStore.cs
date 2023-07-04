using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantripDataStore : IComparable<CantripDataStore>
{
    public string cantripName;
    public int cost;
    public string effectText;

    public GameObject effect;

    public CantripDataStore(Cantrip cantrip)
    {
        cantripName = cantrip.cantripName;
        cost = cantrip.cantripCost;
        effectText = cantrip.effectText;
    }

    public string GetName() { return cantripName; }
    public int GetCost() { return cost; }
    public string GetEffectText()
    {
        return effectText;
    }


    public int CompareTo(CantripDataStore other)
    {
        /*
         * The method returns 0 if the data is equal to the other data. A value less than 0 is returned if the data is 
         * less than the other data (less characters) 
         * and a value greater than 0 if the data is greater than the other data (more characters).
         */

        int nameCompare = other.cantripName.CompareTo(cantripName);

        return nameCompare;
    }
}
