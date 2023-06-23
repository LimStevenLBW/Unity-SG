using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRate : MonoBehaviour
{
    public UDictionary<Unit, int> unitDictionary = new UDictionary<Unit, int>();
    public int totalAmount;
    private List<Unit> dropList = new List<Unit>();
    // Start is called before the first frame update
    void Start()
    {
        FormDropList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FormDropList()
    {
        dropList.Clear();

        //Form the droplist
        foreach (Unit unit in unitDictionary.Keys)
        {
            int num = unitDictionary[unit];
            for (int i = 0; i < num; i++)
            {
                dropList.Add(unit);
            }
        }
    }


    public List<Unit> GetDropSelection(int size)
    {
        if (dropList.Count == 0) FormDropList();
        List<Unit> dropSelection = new List<Unit>();

        int selector;

        //Create a random deck from the droplist
        for (int i = 0; i < size; i++)
        {
            //The int version is maxexclusive
            selector = Random.Range(0, dropList.Count);
            Unit unit = dropList[selector];

            //If it already contains the unit
            if (dropSelection.Contains(unit))
            {

                selector = Random.Range(0, 2);

                //If it already contains that unit, try again
                if (selector == 1)
                {
                    selector = Random.Range(0, dropList.Count);
                    unit = dropList[selector];
                }
            }

            dropSelection.Add(unit);
        }

        return dropSelection;
    }

    public Deck GetRandomDeck(int size)
    {

        return Deck.CreateInstance(GetDropSelection(size));
    }
}
