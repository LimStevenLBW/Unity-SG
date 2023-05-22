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
    public enum State
    {
        IDLE,
        ACTING,
        MOVING,
        STUNNED,
        DEAD
    }

    public UnitDataStore data;
    public UnitController prefab;
    public Pathfinder path;
    public MicroBarFollow bars;

    private List<UnitController> myAllies;
    private List<UnitController> myEnemies;

    private UnitManager manager;
    private Animator animator;

    public State state = State.IDLE;

    private const int MOVECOST = 1;
    private const int MOVECOST_ELEVATION = 5;
    
    private HexCell location, currentTravelLocation, startingLocation;
    private float orientation;
    public int maxRange; //The range that unit wants to stay at due to their skills

    private float travelSpeed = 4f;
    private float rotationSpeed = 360f;

    List<HexCell> pathToTravel;

    bool ACTIVE = false;

    private HexCell highlightedCell;
    public HexGrid Grid { get; set; }

    // Called when a controller is instantiated by the manager
    public void Initialize(UnitManager manager, MicroBarFollow bars)
    {
        this.manager = manager;
        path = new Pathfinder(manager.grid, manager, this, null);

        //Might have to update later, but for now, update the data with this instantiated controller.
        data.controller = this;
        data.InitSkills();

        this.bars = bars;
        bars.Initialize(this);
        startingLocation = location;

        //Copy a reference the controller lists. Remember to only edit this in UnitManager for organization!
        myAllies = manager.GetControllers(teamNum, true);
        myEnemies = manager.GetControllers(teamNum, false);
        animator = GetComponent<Animator>();

        Director.Instance.OnCombatStarted += SetActive;
        Director.Instance.OnCombatEnded += SetInactive;
        Director.Instance.AddControllerTraits(this);
    }

    void Awake()
    {
        
    }

    // Update is called once per frame, nothing runs unless active
    void Update()
    {
        if (ACTIVE && state == State.DEAD)
        {
            StopAllCoroutines();
            animator.SetTrigger("die");
            ACTIVE = false; //Prevents consecutive runs of this block
            data.StopListening();
            manager.RemoveUnit(this); //problematic at the moment
        }

        //Check for any previously highlighted cells and disable them
        if(highlightedCell)
        {
            if(highlightedCell != location)
            {
                highlightedCell.DisableHighlight(false);
                highlightedCell = null;
            }
        }

        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetBool("isAttacking", false);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("isAttacking", true);
        }
        */
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
        //statusText.SetText(state.ToString() + "Team: " + teamNum);
    }

    /*
     * Frees up this unit start fighting
     */
    public void SetActive()
    {
        ACTIVE = true;
        data.StartListening();
    }

    public void SetInactive()
    {
        ACTIVE = false;
        data.StopListening();
        data.ResetCooldowns();
    }

    public void UpdateStartingLocation()
    {
        startingLocation = location;
    }

    public void ResetLocation()
    {
        Location = startingLocation;
    }

    void CalculateNextAction()
    {
        if (data.skill1 != null && data.skill1.IsAvailable())
        {
            SetState("ACTING");
            data.skill1.DoSkill();
        }
        else if (data.skill2 != null && data.skill2.IsAvailable())
        {
            SetState("ACTING");
            data.skill2.DoSkill();
            
        }
        else if (data.skill3 != null && data.skill3.IsAvailable())
        {
            SetState("ACTING");
            data.skill3.DoSkill();
        }
        else if (data.skill4 != null && data.skill4.IsAvailable())
        {
            SetState("ACTING");
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

            if (value)
            {
                value.unitController = this;
                transform.localPosition = value.Position;
            }
                //value.IncreaseVisibility();

            }
    }

    public void EnableHighlight()
    {
        highlightedCell = location;
        GetComponent<Outline>().enabled = true;

        //Do not hightlight the cell if the unit is dead
        if(state != State.DEAD) location.EnableHighlight(Color.white, false);
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

    public void Die(float time)
    {
        if (location)
        {
            location.DisableHighlight(false); //Disable cell highlight
            location.unitController = null;
            // Grid.DecreaseVisibility(location, VisionRange);
        }

        //works with unitmanager, removeunit()

        manager.PATHFINDING_IN_USE = false;

        Destroy(gameObject, time);
        Destroy(bars.gameObject, time);
    }


    public void SetState(string text)
    {
        if (text.Equals("DEAD"))
        {
            state = State.DEAD;
        }
        else if (text.Equals("IDLE"))
        {
            state = State.IDLE;
        }
        else if(text.Equals("ACTING"))
        {
            state = State.ACTING;
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

       
        StartCoroutine(OnPerformAnimation(anim, timing, skill, cell));


    }
    public void StopAnim(string anim)
    {
        
    }

    IEnumerator OnPerformAnimation(string anim, float timing, Skill skill, HexCell cell)
    {
        float animationLength = 1f;
        //Turn to look at target cell, if need be
        if (cell)
        {
            yield return LookAt(cell.Position);
        }

        animator.SetBool(anim, true);

        yield return new WaitForSeconds(timing);
        skill.HandleAnimExtra();

        if (timing < animationLength) animationLength -= timing;
        yield return new WaitForSeconds(animationLength);

        animator.SetBool(anim, false);

        while (skill.IsSkillRunning() == true) yield return null;

        //Return to IDLE state after completion
        SetState("IDLE");
    }

    /* OLDER VERSION KEPT HERE
     * Works in tandem with Talent/Skill
     * todo, normalizedtime may be very unreliable depending on the animation, look into animation events
     * We run into problems with animation timings, for example, some models have faster attack animations than others, this affects the skill usefulness
     */

    /*
    IEnumerator OnPerformAnimation(string anim, float timing, Skill skill, HexCell cell)
    {
        //Turn to look at cell, if need be
        if (cell)
        {
            yield return LookAt(cell.Position);
        }

        //Normalized time updates rather slowly, we check if it's above 1f before proceeding because it is currently 
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
    */

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

    public void AddAura(GameObject aura, Vector3 pos, Skill skill, float ms)
    {
        GameObject obj = Instantiate(aura, pos, Quaternion.identity) as GameObject;
        obj.transform.SetParent(gameObject.transform); //Have the aura follow this unit

        obj.SendMessage("SelfDestruct", ms);
        obj.SendMessage("SetSkill", skill);
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
        animator.SetBool("isWalking", true);
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

        while (movementSkill.IsSkillRunning() == true) yield return null;

        animator.SetBool("isWalking", false);
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

    public List<UnitController> GetAllies() {
        return myAllies;
    }

    public List<UnitController> GetEnemies()
    {
        return myEnemies;
    }
    public UnitManager GetManager()
    {
        return manager;
    }

    public float GetTravelSpeed()
    {
        return travelSpeed;
    }

    public void SetTravelSpeed(float travelSpeed)
    {
        this.travelSpeed = travelSpeed;
    }
    public float GetRotationSpeed()
    {
        return rotationSpeed;
    }

    public void SetRotationSpeed(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }
}
