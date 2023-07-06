using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Holds stage select objects, displays where we are on the route
 */
public class RouteColumn : MonoBehaviour
{
    private int id;
    //private bool active;
    Image image;

    private Color activeColor;
    private Color completedColor;

    private RouteOptionsDisplay routeOptions;

    void Awake()
    {
       image = GetComponent<Image>();
       activeColor = new Color(140f/255, 225f/255, 75f/255, 0.3f);
       completedColor = new Color(45f/255, 45f/255, 45f/255, 0.7f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetActive()
    {
        image.color = activeColor;
        //active = true;

        if(routeOptions) routeOptions.NextStage();
        else
        {
            Debug.Log("no stage data");
        }
    }

    public void SetCompleted()
    {
        image.color = completedColor;
        //active = false;
    }

    //Grant a prefab to the column that contains the stage display information and data
    public void Initialize(int id)
    {
        this.id = id;

        //Hard coded stages for now
        if(id == 1) {
            InstantiateRoute("Route Display/1 Route Stage 1");
        }
        else if(id == 2) {
            InstantiateRoute("Route Display/1 Route Stage 2");
        }
        else if(id == 3) {
            InstantiateRoute("Route Display/1 Route Stage 3");
        }
        else if (id == 4) {
            InstantiateRoute("Route Display/1 Route Stage 4");
        }
        else if (id == 5)
        {
            InstantiateRoute("Route Display/1 Route Stage 5");
        }

    }

    void InstantiateRoute(string prefabLocation)
    {

        //The Instantiate function returns an abstract Object reference, that's why it effectively instantiates the object but can also give you a type error. so we do this instead
        GameObject obj = Instantiate(Resources.Load(prefabLocation),
               transform.position,
               Quaternion.identity,
               transform
           ) as GameObject;


        routeOptions = obj.GetComponent<RouteOptionsDisplay>();
        Vector3 pos = routeOptions.transform.position;
        routeOptions.transform.position = new Vector3(pos.x + 25, pos.y, pos.z); //Adjusting with offset
    }

}
