using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck", menuName = "Deck")]
public class Deck : ScriptableObject
{
    public List<Unit> unitList;
    public Unit captain;

    private void Init(List<Unit> unitList)
    {
        this.unitList = unitList;
    }
    public static Deck CreateInstance(List<Unit> unitList)
    {
        var data = ScriptableObject.CreateInstance<Deck>();
        data.Init(unitList);
        return data;
    }
}
