using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Uses unit data to interact with the combat scene. 
 * Defines the AI for units
 */
public class UnitController : MonoBehaviour
{
    enum State
    {
        IDLE,

    }

    public Unit unit;
    public UnitController prefab;

    private Animator animator;
    private State state;
    private UnitManager manager;

    private HexCell location, currentTravelLocation;
    private float orientation;

    const float travelSpeed = 4f;
    const float rotationSpeed = 180f;

    List<HexCell> pathToTravel;

    bool ACTIVE = false;

    public HexGrid Grid { get; set; }

    //Initialize is only called the first time a unit is obtained

    public void Initialize(UnitManager manager)
    {
        this.manager = manager;

        unit.Initialize(this, manager); //Initialize combat values, pass itself down

        state = State.IDLE;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ACTIVE = true;
            unit.StartListening();
        }

        if (ACTIVE)
        {

            if (state == State.IDLE)
            {
                CalculateNextAction();
            }
        }

        
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetBool("isWalking", false);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("isWalking", true);
        }
        */

    }
   

    void CalculateNextAction()
    {
        if (unit.skill1 && unit.skill1.IsAvailable())
        {
            unit.skill1.DoSkill();
        }
        else if (unit.skill2 && unit.skill2.IsAvailable())
        {
            unit.skill2.DoSkill();
        }
        else if (unit.skill3 && unit.skill3.IsAvailable())
        {
            unit.skill3.DoSkill();
        }
        else if (unit.skill4 && unit.skill4.IsAvailable())
        {
            unit.skill4.DoSkill();
        }
    }

    public float Orientation
    {
        get
        {
            return orientation;
        }
        set
        {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }
    public int Speed
    {
        get
        {
            return 24;
        }
    }
    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            if (location)
            {
                //location.DecreaseVisibility();

                location.unitController = null;
            }
            location = value;
            value.unitController = this;
            //value.IncreaseVisibility();

            transform.localPosition = value.Position;
        }
    }

    public void EnableHighlight()
    {
        GetComponent<Outline>().enabled = true;
        location.EnableHighlight(Color.white, false);
    }
    public void DisableHighlight()
    {
        GetComponent<Outline>().enabled = false;
        location.DisableHighlight(false);
    }

    public void ValidateLocation()
    {
        transform.localPosition = location.Position;
    }

    /*
     * Return true if the cell is a valid location for this unit to travel to
     */
    public bool IsValidDestination(HexCell cell)
    {
        return !cell.unitController;
    }

    public void Travel(List<HexCell> path)
    {
        location.formationController = null;
        location = path[path.Count - 1];
        //location.Unit = this;
        Debug.Log("REENABLE Line 108 PLAYER FORMATION");

        pathToTravel = path;
        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

    public void Die()
    {
        if (location)
        {
           // Grid.DecreaseVisibility(location, VisionRange);
        }
        location.unitController = null;
        Destroy(gameObject);
    }


    IEnumerator TravelPath()
    {
        Vector3 a, b, c = pathToTravel[0].Position;
        //transform.localPosition = c;
        yield return LookAt(pathToTravel[1].Position);

        float t = Time.deltaTime * travelSpeed;

        for (int i = 1; i < pathToTravel.Count; i++)
        {
            currentTravelLocation = pathToTravel[i];
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + currentTravelLocation.Position) * 0.5f;

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                yield return null;
            }

            t -= 1f;
        }
        currentTravelLocation = null;

        a = c;
        //b = pathToTravel[pathToTravel.Count - 1].Position;
        b = location.Position;
        c = b;

        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            yield return null;
        }
        transform.localPosition = location.Position;

        //Release Cell List
        ListPool<HexCell>.Add(pathToTravel);
        pathToTravel = null;
    }

    IEnumerator LookAt(Vector3 point)
    {
        point.y = transform.localPosition.y;
        Quaternion fromRotation = transform.localRotation;
        Quaternion toRotation =
            Quaternion.LookRotation(point - transform.localPosition);
        float angle = Quaternion.Angle(fromRotation, toRotation);
        if (angle > 0f)
        {
            float speed = rotationSpeed / angle;

            for (
                float t = Time.deltaTime * speed;
                t < 1f;
                t += Time.deltaTime * speed
            )
            {
                transform.localRotation =
                    Quaternion.Slerp(fromRotation, toRotation, t);
                yield return null;
            }
        }

        transform.LookAt(point);
        orientation = transform.localRotation.eulerAngles.y;
    }

    public int GetMoveCost(HexCell fromCell, HexCell toCell, HexDirection direction)
    {
        HexEdgeType edgeType = fromCell.GetEdgeType(toCell);
        if (edgeType == HexEdgeType.Cliff) //AVOID THESE EDGE TYPES
        {
            return -1;
        }

        int moveCost;
        if (fromCell.HasRoadThroughEdge(direction))
        {
            moveCost = 1;
        }
        else if (fromCell.Walled != toCell.Walled)
        {
            return -1;
        }
        else
        {
            moveCost = edgeType == HexEdgeType.Flat ? 5 : 10; //MOVE COSTS
            moveCost +=
                toCell.UrbanLevel + toCell.FarmLevel + toCell.PlantLevel;
        }

        return moveCost;
    }

}
