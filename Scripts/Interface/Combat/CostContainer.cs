using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostContainer : MonoBehaviour
{
    public GameObject chiStarPrefab;
    private List<GameObject> children;

    public void Setup(int cost)
    {
        for(int i = 0; i < cost; i++)
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
}
