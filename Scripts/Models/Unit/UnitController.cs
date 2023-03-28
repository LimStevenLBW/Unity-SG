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
        ACTING,
    }

    public Unit unitBase;
    public UnitDataStore data;
    public UnitController prefab;
    public Pathfinder path;

    private UnitManager manager;
    private Animator animator;
    private State state;

    private const int MOVECOST = 1;
    private const int MOVECOST_ELEVATION = 5;

    private HexCell location, currentTravelLocation;
    private float orientation;

    const float travelSpeed = 4f;
    const float rotationSpeed = 200f;

    List<HexCell> pathToTravel;

    bool ACTIVE = false;

    public HexGrid Grid { get; set; }


    //Initialize is only called the first time a unit is obtained
    public void Initialize(UnitManager manager)
    {
        this.manager = manager;
       
        path = new Pathfinder(manager.grid, manager, this, null);
        data = new UnitDataStore(this, unitBase);
        

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
            data.StartListening();
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
        //MOVEMENT SKILL
        //To prevent problems, only one unit is allowed to calculate movement at a time
       
        if (data.IsMovementAvailable())
        {
            Debug.Log(data.GetName() + " movement is available, and thinks Pathfinding is : " + manager.PATHFINDING_IN_USE);
            if (!manager.PATHFINDING_IN_USE)
            {
                manager.PATHFINDING_IN_USE = true;
                state = State.ACTING;

                Debug.Log(data.GetName() + " is doing its skill");

               data.movementSkill.DoSkill();
                Debug.Log(data.GetName() + " is finishing its skill");

               manager.PATHFINDING_IN_USE = false;

            }
        }
        

        /*
        else if (unit.skill2 && unit.skill2.IsAvailable())
        {
            state = State.ACTING;
            unit.skill2.DoSkill();
        }
        else if (unit.skill3 && unit.skill3.IsAvailable())
        {
            state = State.ACTING;
            unit.skill3.DoSkill();
        }
        else if (unit.skill4 && unit.skill4.IsAvailable())
        {
            state = State.ACTING;
            unit.skill4.DoSkill();
        }
        */



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
     * I can include conditions for why traveling to a certain cell would be invalid here
     */
    public bool IsValidDestination(HexCell cell)
    {
        return !cell.unitController;
    }

    /* Have this unit start traversing along a path of given hexcells */
    public void Travel(List<HexCell> path, int steps)
    {
        location.unitController = null; //This unit no longer occupies the current hexcell
        location = path[path.Count - 1]; //Its new location is set to the ending hexcell
        location.unitController = this; //The hexcell's unit is set to be this

        pathToTravel = path; //Store the path

        StopAllCoroutines();
        StartCoroutine(TravelPath(steps)); //Start walking, if steps is 0, walk the whole way
    }


    /*
     * Start traversing through the path to the hexcell, clear the path at the end of the walk
     */
    IEnumerator TravelPath(int steps)
    {
        Vector3 a, b, c = pathToTravel[0].Position;

        yield return LookAt(pathToTravel[1].Position);
        //Grid.DecreaseVisibility(pathToTravel[0], visionRange);
        float t = Time.deltaTime * travelSpeed;

        for (int i = 1; i < pathToTravel.Count; i++)
        {
            currentTravelLocation = pathToTravel[i];
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + currentTravelLocation.Position) * 0.5f;
            //Grid.IncreaseVisibility(pathToTravel[i], visionRange);

            for (; t < 1f; t += Time.deltaTime * travelSpeed)
            {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }

            //Grid.DecreaseVisibility(pathToTravel[i], visionRange);
            t -= 1f;
        }
        currentTravelLocation = null;

        a = c;
        b = location.Position;
        c = b;
        //Grid.IncreaseVisibility(location, visionRange);

        for (; t < 1f; t += Time.deltaTime * travelSpeed)
        {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }
        transform.localPosition = location.Position;

        //Release Cell List
        ListPool<HexCell>.Add(pathToTravel);
        pathToTravel = null;
    }

    /*
     * Turn unit to look at a vector point
     */
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
            moveCost = edgeType == HexEdgeType.Flat ? MOVECOST : MOVECOST_ELEVATION; //MOVE COSTS
            moveCost +=
                toCell.UrbanLevel + toCell.FarmLevel + toCell.PlantLevel;
        }

        return moveCost;
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

    public void SetState(string text)
    {
        if (text.Equals("IDLE"))
        {
            this.state = State.IDLE;
        }
    }
}
