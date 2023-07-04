using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildRosterContentGroup : MonoBehaviour
{
    public UnitElement unitElement;
    [SerializeField] private GuildRoster guildRoster;
    [SerializeField] private CardSummaryBox cardSummaryBox;

    private List<UnitElement> elements = new List<UnitElement>();

    public void Setup(List<Card> cardList)
    {
        //cardList.Sort();
        foreach (Card card in cardList)
        {
            UnitElement element = Instantiate(unitElement);
            element.GetData(card, guildRoster);
            element.gameObject.transform.SetParent(gameObject.transform);
            element.gameObject.transform.SetAsFirstSibling();
            element.gameObject.transform.localScale = new Vector3(1, 1, 1);
            if(cardSummaryBox!= null) element.SetCardSummaryBox(cardSummaryBox);
            elements.Add(element);

        }
    }

    public void Setup(List<UnitDataStore> unitList)
    {
        unitList.Sort();
        foreach(UnitDataStore unit in unitList)
        {
            UnitElement element = Instantiate(unitElement);
          //element.GetData(unit, guildRoster);
            element.gameObject.transform.SetParent(gameObject.transform);
            element.gameObject.transform.SetAsFirstSibling();
            element.gameObject.transform.localScale = new Vector3(1, 1, 1);
            elements.Add(element);

        }
    }

    public void Clear()
    {
        foreach(UnitElement element in elements)
        {
            Destroy(element.gameObject);
        }
        elements.Clear();
    }

    public void DisplayImmediate()
    {
        foreach (UnitElement element in elements)
        {
            element.Idle();
        }
    }

    public void DisplayDelayed()
    {
        StartCoroutine(ConsecutiveReveal());
    }

    IEnumerator ConsecutiveReveal()
    {
        foreach(UnitElement element in elements)
        {
            element.Enter();
            yield return new WaitForSeconds(0.01f);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
