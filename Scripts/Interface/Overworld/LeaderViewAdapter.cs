using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameSystems;
using States;
public class LeaderViewAdapter : MonoBehaviour
{
    /* The Leaders owned by the player at this current session*/
    //public List<Leader> PlayerLeaders { get; private set; }

    /* The Scriptable Object containing a list of all Leader SOs */
    public InventoryMap LeaderMap;
    /* The prefab template of the list items to display */
    public GameObject ItemTemplate;
    /* The actual list of gameobjects to display in the scroll view */
    private List<GameObject> ItemList;

    /*
    public static LeaderViewAdapter i = null;
    void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else if (i != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PlayerLeaders = new List<Leader>();
        ItemList = new List<GameObject>();
        ItemList.Add(ItemTemplate);
    }

    void OnEnable()
    {
        EventBroker.OnDataLoaded += UnpackLoadedData;
    }

    void OnDisable()
    {
        EventBroker.OnDataLoaded -= UnpackLoadedData;
    }

    public void UnpackLoadedData(GameState savedata)
    {
        //Cycle through each id in the savedata
        foreach(int id in savedata.Leaders)
        {
            //Find the matching id in LeaderMap(A glossary of all leaders, and add it to the current session
            Leader newLeader = LeaderMap.Leaders.Find(x => x.ID == id);
            Debug.Log("Found Leader: " + newLeader.FirstName + "\n");
            PlayerLeaders.Add(newLeader);
        }
        RefreshList();
    }

    /*Trigger when a change to the list occurs, refreshes and displays the leaders list*/
    /*
    public void RefreshList()
    {
        if(ItemList.Count > 0)
        {
            foreach(GameObject item in ItemList)
            {
                Destroy(item);
            }
            ItemList.Clear();
        }

        foreach(Leader leader in PlayerLeaders)
        {
            GameObject newItem = Instantiate(ItemTemplate) as GameObject;
            newItem.SetActive(true);
            newItem.GetComponent<LeaderViewItem>().SetName(leader.FirstName);
            newItem.GetComponent<LeaderViewItem>().SetTroopCount(leader.TroopCount);
            newItem.GetComponent<LeaderViewItem>().SetStatus("On Standby");
            newItem.GetComponent<LeaderViewItem>().SetImage(leader.Portrait);

            newItem.transform.SetParent(ItemTemplate.transform.parent, false);
        }
    }
    */

}
