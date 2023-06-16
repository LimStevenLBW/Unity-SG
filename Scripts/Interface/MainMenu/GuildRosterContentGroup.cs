using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildRosterContentGroup : MonoBehaviour
{
    public UnitElement unitElement;
    private List<UnitElement> elements = new List<UnitElement>();

    public void Setup(List<UnitDataStore> unitList)
    {
        foreach(UnitDataStore unit in unitList)
        {
            UnitElement element = Instantiate(unitElement);
            element.GetData(unit);
            element.gameObject.transform.SetParent(gameObject.transform);
            element.gameObject.transform.SetAsFirstSibling();
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

    public void Display()
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
