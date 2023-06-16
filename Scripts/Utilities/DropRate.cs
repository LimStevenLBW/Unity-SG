using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRate : MonoBehaviour
{
    public UDictionary<Unit, int> unitDictionary = new UDictionary<Unit, int>();
    public int totalAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Deck GetRandomDeck()
    {
        List<Unit> dropList = new List<Unit>();
        List<Unit> newList = new List<Unit>();

        //Form the droplist
        foreach(Unit unit in unitDictionary.Keys)
        {
            int num = unitDictionary[unit];
            for(int i=0; i < num; i++)
            {
                dropList.Add(unit);
            }
        }

        int selector;

        //Create a random deck from the droplist
        for (int i=0; i < 25; i++)
        {
            //The int version is maxexclusive
            selector = Random.Range(0, dropList.Count);
            Unit unit = dropList[selector];

            //If it already contains the unit
            if (newList.Contains(unit)) {

                selector = Random.Range(0, 2);

                //If it already contains that unit, try again
                if(selector == 1)
                {
                    selector = Random.Range(0, dropList.Count);
                    unit = dropList[selector];
                }
            }

            newList.Add(unit);
        }

        
        return Deck.CreateInstance(newList);
    }
}
