using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteMap : MonoBehaviour
{
    private int num = -1;
    private Director _director;
    public List<RouteColumn> routeColumns = new List<RouteColumn>();

    public StageSelect selectFromTwo;
    public StageSelect selectFromThree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Director director)
    {
        _director = director;

        for(int i=0; i< routeColumns.Count; i++){
            routeColumns[i].Initialize(i+1);
        }

    }

    public void AdvanceRoute()
    {
        if(num > -1) routeColumns[num].SetCompleted(); //Skip marking the first one

        num++;

        //Set a new active column
        if (routeColumns[num]) routeColumns[num].SetActive();

        
    }

    public void DisplayRoute()
    {
        gameObject.SetActive(true);
    }

    public void HideRoute()
    {
        gameObject.SetActive(false);
    }
}
