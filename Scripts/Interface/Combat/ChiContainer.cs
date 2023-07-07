using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiContainer : MonoBehaviour
{
    public int chi { get; private set; }
    public GameObject chiStarPrefab;
    private List<GameObject> children = new List<GameObject>();

    public void Setup(int amount)
    {
        chi = amount;
        for(int i = 0; i < amount; i++)
        {
            GameObject element = Instantiate(chiStarPrefab);
            element.gameObject.transform.SetParent(gameObject.transform);
            element.gameObject.transform.SetAsFirstSibling();
            element.gameObject.transform.localScale = new Vector3(1, 1, 1);

            children.Add(element);
        }
    }

    public void Clear()
    {
        foreach (GameObject element in children)
        {
            Destroy(element.gameObject);
        }
        children.Clear();
    }

    public void Refresh()
    {
        Clear();
        Setup(chi);
    }

    public void Gain(int count)
    {
        chi += count;
        if (chi > 9) chi = 9;

        Refresh();
    }
    public void Spend(int count)
    {
        chi -= count;
        if (chi < 0) chi = 0;

        Refresh();
    }


}
