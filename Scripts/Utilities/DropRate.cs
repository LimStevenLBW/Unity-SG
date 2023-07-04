using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRate : MonoBehaviour
{
    //The Second value of the UDictionary indicates how many copies of the unit exist in the drop table
    public UDictionary<Unit, int> unitDictionary = new UDictionary<Unit, int>();
    public UDictionary<Cantrip, int> cantripDictionary = new UDictionary<Cantrip, int>();
    // public UDictionary<Relic, int> relicDictionary = new UDictionary<Relic, int>();

    private List<Unit> unitDropList = new List<Unit>();
    private List<Cantrip> cantripDropList = new List<Cantrip>();

    [SerializeField] private int A_RANK_LIMIT;
    [SerializeField] private int B_RANK_LIMIT;
    [SerializeField] private int DECK_GENERATION_SIZE;

    //[SerializeField] private int UNIT_DROP_MAX_AMT;
    [SerializeField] private int UNIT_PREFERRED_AMT;

    //[SerializeField] private int CANTRIP_DROP_MAX_AMT;
    //[SerializeField] private int CANTRIP_PREFERRED_AMT;

    private const int deviation = 3; //How much random values can deviate from the preferred amount


    // Start is called before the first frame update
    void Start()
    {
        FormDropLists();
    }

    void FormDropLists()
    {
        unitDropList.Clear();
        cantripDropList.Clear();

        //Form the droplists
        foreach (Unit unit in unitDictionary.Keys)
        {
            int num = unitDictionary[unit];
            for (int i = 0; i < num; i++)
            {
                unitDropList.Add(unit);
            }
        }

        foreach (Cantrip cantrip in cantripDictionary.Keys)
        {
            int num = cantripDictionary[cantrip];
            for (int i = 0; i < num; i++)
            {
                cantripDropList.Add(cantrip);
            }
        }
    }

    //Get a new unit list from the drop table with a preset size
    public List<Unit> GetUnitDropSelection(int listSize)
    {
        List<Unit> dropSelection = new List<Unit>();
        if (listSize == 0) return dropSelection;
        int selector;

        //Create a random list of units from the droplist
        for (int i = 0; i < listSize; i++)
        {
            Unit unit = GetRandomUnit();

            //If our deck already contains the dropped unit, there's a chance that a reroll of it will happen
            if (dropSelection.Contains(unit))
            {
                selector = Random.Range(0, 2);
                if (selector == 1) unit = GetRandomUnit();

            }
            dropSelection.Add(unit);
        }

        return dropSelection;
    }

    //Get a new cantrip list from the drop table with a preset size
    public List<Cantrip> GetCantripDropSelection(int listSize)
    {
        List<Cantrip> dropSelection = new List<Cantrip>();
        if (listSize == 0) return dropSelection;
        int selector;

        for (int i = 0; i < listSize; i++)
        {
            Cantrip cantrip = GetRandomCantrip();

            //If our deck already contains the dropped cantrip, there's a chance that a reroll of it will happen
            if (dropSelection.Contains(cantrip))
            {
                selector = Random.Range(0, 2);
                if (selector == 1) cantrip = GetRandomCantrip();
            }
            dropSelection.Add(cantrip);
        }

        return dropSelection;
    }

    //Generate a random deck according to the preferred amount restrictions
    public Deck GetRandomDeck()
    {
        int unitDeviation = Random.Range(-deviation, deviation) + 1;
        int unit_amount = UNIT_PREFERRED_AMT + unitDeviation;
        if (unit_amount > DECK_GENERATION_SIZE) unit_amount = DECK_GENERATION_SIZE; // Cannot go over max

        List<Unit> myUnitList = GetUnitDropSelection(unit_amount);
        List<Cantrip> myCantripList = GetCantripDropSelection(DECK_GENERATION_SIZE - unit_amount); //Fill the rest of space with random cantrips
        return Deck.CreateInstance(myUnitList, myCantripList);
    }

    private Cantrip GetRandomCantrip()
    {
        int selector = Random.Range(0, cantripDropList.Count);
        Cantrip cantrip = cantripDropList[selector];
        return cantrip;
    }

    private Unit GetRandomUnit()
    {
        //The int version is maxexclusive
        int selector = Random.Range(0, unitDropList.Count);
        Unit unit = unitDropList[selector];
        return unit;
    }
}
