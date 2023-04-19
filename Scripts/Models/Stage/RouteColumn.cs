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
    private bool active;
    Image image;

    private Color activeColor;
    private Color completedColor;

    private RouteOptionsDisplay routeOptions;

    void Awake()
    {
       active = false;
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
        active = true;

        routeOptions.NextStage();
    }

    public void SetCompleted()
    {
        image.color = completedColor;
        active = false;
    }

    public void Initialize(int id)
    {
        this.id = id;

        //Hard coded stages for now
        if(id == 1)
        {
            InstantiateRoute("Route Display/1 Stage");
        }
        else if(id == 2) { 

        }

    }

    void InstantiateRoute(string prefabLocation)
    {

        //The Instantiate function returns an abstract Object reference, that's why it effectively instantiates the object but ca also give you a type error. so we do this instead
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
