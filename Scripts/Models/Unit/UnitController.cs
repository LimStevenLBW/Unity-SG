using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*
 * Uses unit data to interact with the combat scene. 
 * Defines the AI for units
 */
public class UnitController : MonoBehaviour
{
    //Debugging
    public TextMeshProUGUI statusText;
    public int teamNum;
    enum State
    {
        IDLE,
        ACTING,
        MOVING,
        STUNNED,
        DEAD
    }

    public Unit unitBase;
    public UnitDataStore data;
    public UnitController prefab;
    public Pathfinder path;

    public List<UnitController> myAllies;
    public List<UnitController> myEnemies;

    private UnitManager manager;
    private Animator animator;

    private State state = State.IDLE;

    private const int MOVECOST = 1;
    private const int MOVECOST_ELEVATION = 5;

    private HexCell location, currentTravelLocation;
    private float orientation;

    const float travelSpeed = 4f;
    const float rotationSpeed = 100f;

    List<HexCell> pathToTravel;

    bool ACTIVE = false;

    public HexGrid Grid { get; set; }

    // Called when a controller is instantiated by the manager
    public void Initialize(UnitManager manager)
    {
        this.manager = manager;

        path = new Pathfinder(manager.grid, manager, this, null);
        data = new UnitDataStore(this, unitBase);

        //Copy a reference the controller lists. Remember to only edit this in UnitManager for organization!
        myAllies = manager.GetControllers(teamNum, true);
        myEnemies = manager.GetControllers(teamNum, false);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ACTIVE = true;
            data.StartListening();
        }

        if (state == State.DEAD)
        {
            StopAllCoroutines();
            animator.SetTrigger("die");
            ACTIVE = false;

            manager.RemoveUnit(this); //problematic at the moment
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetBool("isAttacking", false);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("isAttacking", true);
        }

    }

    void LateUpdate()
    {
        if (ACTIVE)
        {
            if (myEnemies.Count > 0) //If there are still enemies
            {
                if(state == State.IDLE) CalculateNextAction();
            }

        }

        //For Debugging
        statusText.SetText(state.ToString() + "Team: " + teamNum);
    }

    void CalculateNextAction()
    {
        if(data.skill1 != null && data.skill1.IsAvailable())
        {
            state = State.ACTING;
            data.skill1.DoSkill(); 
        }
        else if (data.skill2 != null && data.skill2.IsAvailable())
        {
            state = State.ACTING;
            data.skill2.DoSkill();
        }
        else if (data.skill3 != null && data.skill3.IsAvailable())
        {
            state = State.ACTING;
            data.skill3.DoSkill();
        }
        else if (data.skill4 != null && data.skill4.IsAvailable())
        {
            state = State.ACTING;
            data.skill4.DoSkill();
        }
        else if (data.movementSkill != null && data.movementSkill.IsAvailable())
        {
            //MOVEMENT SKILL
            //To prevent problems, only one unit is allowed to calculate movement at a time
            if (!manager.PATHFINDING_IN_USE)
            {
                manager.PATHFINDING_IN_USE = true;
                state = State.MOVING;
                data.movementSkill.DoSkill();
                manager.PATHFINDING_IN_USE = false;
           }
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

        Destroy(gameObject, 4f);
    }


    public void SetState(string text)
    {
        if (text.Equals("DEAD"))
        {
            state = State.DEAD;
        }
        if (text.Equals("IDLE"))
        {
            state = State.IDLE;
        }
    }

    public string GetState()
    {
        if(state == State.DEAD)
        {
            return "DEAD";
        }
        if(state == State.IDLE)
        {
            return "IDLE";
        }
        if (state == State.ACTING) return "ACTING";

        return "Unknown State";
    }


    public void PlayAnim(string anim, float timing, Skill skill)
    {
        animator.SetBool(anim, true);
       
        StartCoroutine(OnPerformAnimation(anim, timing, skill, null));

    }

    //With direction
    public void PlayAnim(string anim, float timing, Skill skill, HexCell cell)
    {
        animator.SetBool(anim, true);
       
        StartCoroutine(OnPerformAnimation(anim, timing, skill, cell));


    }
    public void StopAnim(string anim)
    {
        
    }

    //Works in tandem with Talent/Skill
    IEnumerator OnPerformAnimation(string anim, float timing, Skill skill, HexCell cell)
    {

        //Turn to look at cell, if need be
        if (cell)
        {
            yield return LookAt(cell.Position);
        }

        //Normalized time updates rather slowly, we check if its above 1f before proceeding because it is currently 
        //at the old value from the previous animation
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < timing) yield return null;

        //Do something in the middle of the animation, based on the skill 
        skill.HandleAnimExtra();

        //Let it end normally
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f) yield return null;
        animator.SetBool(anim, false);

        while (skill.IsSkillRunning() == true) yield return null;

        //Reset state after the animation is done
        SetState("IDLE");

    }

    public void PlayEffect(GameObject effect, Vector3 pos)
    {
        Instantiate(effect, pos, transform.rotation);
    }

    //Play effect according to a certain amount of time, needed when you need to control animation time or if animation wont close on its own
    public void PlayEffect(GameObject effect, Vector3 pos, float ms)
    {
        GameObject obj = Instantiate(effect, pos, Quaternion.identity) as GameObject;
        // effect.GetComponent<DestroySelf>();

        obj.SendMessage("SelfDestruct", ms);
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
    public void Travel(List<HexCell> path, int steps, Skill movementSkill)
    {
        List<HexCell> trimmedPath = new List<HexCell>();

        
        for (int i = 0; i < path.Count; i++)
        {
            trimmedPath.Add(path[i]);
            if (trimmedPath.Count > steps) break;
        }
        if (!IsValidDestination(trimmedPath[1])) { Debug.Log("Invalid?"); };

        pathToTravel = trimmedPath; //Store the path
        location.unitController = null; //This unit no longer occupies the current hexcell
        location = pathToTravel[pathToTravel.Count - 1]; //Its new location is set to the ending hexcell
        location.unitController = this; //The hexcell's unit is set to be this

        StopAllCoroutines();
        StartCoroutine(TravelPath(movementSkill)); //Start walking, if steps is 0, walk the whole way
    }


    /*
     * Start traversing through the path to the hexcell, clear the path at the end of the walk
     */
    IEnumerator TravelPath(Skill movementSkill)
    {
        Vector3 a, b, c = pathToTravel[0].Position;
        transform.localPosition = c;
        yield return LookAt(pathToTravel[1].Position);

        //Grid.DecreaseVisibility(pathToTravel[0], visionRange);
        float t = Time.deltaTime * travelSpeed;

        for (int i = 1; i < pathToTravel.Count; i++) {
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

        while (movementSkill.IsSkillRunning() == true)
        {
            Debug.Log("Stuck");
            yield return null;
        }

        SetState("IDLE"); //Reset the state
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

            for (float t = Time.deltaTime * speed; t < 1f; t += Time.deltaTime * speed
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
}
