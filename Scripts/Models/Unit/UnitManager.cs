using Assets.Scripts.Models.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * There is only one UnitManager.
 * The Unit Manager knows the controllers that will go into the combat scene
 */
public class UnitManager : MonoBehaviour
{
    [SerializeField] internal AudioSource AudioPlayer;
    [SerializeField] internal AudioClip AudioDeployedUnit;
    private GameObject effect;
    //Temporary for creating Units
    public List<FormationController> units = new List<FormationController>();
    public FormationController unitPrefab;

    //Fielded Controllers
    public List<UnitController> firstTeamControllers = new List<UnitController>();
    public List<UnitController> secondTeamControllers = new List<UnitController>();

    private UnitController currentController;

    //Preset Types
    public UnitController testUnit1;
    public UnitController testUnit2;
    public UnitController testUnit3;
    public UnitController testUnit4;
    public UnitController testUnit5;
    public UnitController testUnit6;

    public HexGrid grid;

    public bool PATHFINDING_IN_USE = false;

    public MicroBarFollow microBars;
    public MicroBarFollow microBarsEnemy;
    // public Queue<UnitController> pathfindingQueue;

    public void InitGrid(HexGrid grid)
    {
        this.grid = grid; 
    }

    void Awake()
    {

    }
    void OnEnable()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //Spawning effect
        effect = Resources.Load("Effects/CFX3_Hit_SmokePuff") as GameObject;
    }

    public void DeployQueuedUnits(Queue<UnitDataStore> deployableUnits, bool isFriendly)
    {
        StartCoroutine(AnimateDeployment(deployableUnits, isFriendly));
    }


    public IEnumerator AnimateDeployment(Queue<UnitDataStore> deployableUnits, bool isFriendly)
    {
        yield return new WaitForSeconds(0.5f);

        int i = 0;
        int size = deployableUnits.Count;
        bool wasDeployed;


        //Cycle for the same number of elements as the queue size
        while(i < size) {
            wasDeployed = false;
            if (currentController == null)
            {
                UnitDataStore data = deployableUnits.Dequeue();
                currentController = data.prefab;

                if (isFriendly)
                {
                    for (int j = 0; j < 40; j++)
                    {
                        if (grid.cells[j].unitController == null)
                        {
                            AddUnit(Instantiate(currentController), grid.cells[j], Random.Range(0f, 360f), data, 1);
                            wasDeployed = true;
                        }

                        if (wasDeployed) break;
                    }
                }
                else
                {
                    for (int j = 79; j >= 40; j--)
                    {
                        if (grid.cells[j].unitController == null)
                        {
                            AddUnit(Instantiate(currentController), grid.cells[j], Random.Range(0f, 360f), data, -1);
                            wasDeployed = true;
                        }

                        if (wasDeployed) break;
                    }
                }
              
                currentController = null;
            }

            i++;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.2f);

        //CHANGE PHASE
        if (isFriendly) Director.Instance.SetPhase("ENEMYCARDSELECT");
        else { Director.Instance.SetPhase("REPOSITIONING"); }
        //StartCoroutine(AnimateDeployEnemy(deployableUnits));
    }

    // Update is called once per frame
    void Update()
    {
       
        //Unit Manager will deploy all selected units in order
        if(Director.Instance.GetPhase() == "DEPLOYMENT")
        {
       
            
        }
        /*
        if (Input.GetKeyDown(KeyCode.A)) CreateCombatUnit(testUnit1, 1);
        else if (Input.GetKeyDown(KeyCode.S)) CreateCombatUnit(testUnit2, 1);
        else if (Input.GetKeyDown(KeyCode.D)) CreateCombatUnit(testUnit3, 1);
        else if (Input.GetKeyDown(KeyCode.F)) CreateCombatUnit(testUnit4, 1);
        else if (Input.GetKeyDown(KeyCode.G)) CreateCombatUnit(testUnit5, 1);
        else if (Input.GetKeyDown(KeyCode.H)) CreateCombatUnit(testUnit6, 1);
        //Enemies are assigned to -1
        if (Input.GetKeyDown(KeyCode.Z)) CreateCombatUnit(testUnit1, -1);
        else if (Input.GetKeyDown(KeyCode.X)) CreateCombatUnit(testUnit2, -1);
        else if (Input.GetKeyDown(KeyCode.C)) CreateCombatUnit(testUnit3, -1);
        else if (Input.GetKeyDown(KeyCode.V)) CreateCombatUnit(testUnit4, -1);
        else if (Input.GetKeyDown(KeyCode.B)) CreateCombatUnit(testUnit5, -1);
        else if (Input.GetKeyDown(KeyCode.N)) CreateCombatUnit(testUnit6, -1);
        */
    }

    public void ClearUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Die();
        }
        units.Clear();
    }

    public void AddFormation(FormationController unit, HexCell location, float orientation)
    {
        units.Add(unit);
        unit.Grid = grid;
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
    }

    public void RemoveUnit(UnitController controller)
    {
        int teamNum = controller.teamNum;
        while (true)
        {        
            //We cannot edit anything that affects unit pathfinding without permission first
            if (PATHFINDING_IN_USE) continue;
            PATHFINDING_IN_USE = true;
            break;
        }

        if (teamNum == 1)
        {
            firstTeamControllers.Remove(controller);
            if (firstTeamControllers.Count == 0)
            {
                Director.Instance.SetPhase("ENDCOMBAT");
                Director.Instance.TakeDamage(true, 1);
            }
        }
        else if (teamNum == -1)
        {
            secondTeamControllers.Remove(controller);
            if (secondTeamControllers.Count == 0)
            {
                Director.Instance.SetPhase("ENDCOMBAT");
                Director.Instance.TakeDamage(false, 1);
            }
        }
        else
        {
            Debug.Log("Invalid Team Number provided on RemoveUnit");
        }

        controller.Die();
    }

    public void ResetUnitPositions()
    {
        foreach(UnitController unit in firstTeamControllers)
        {
            if(unit.gameObject) unit.ResetLocation();
        }
        foreach (UnitController unit in secondTeamControllers)
        {
            if (unit.gameObject) unit.ResetLocation();
        }
    }

    public FormationController GetFormation(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<FormationController>() != null)
            {
                return hit.collider.gameObject.GetComponent<FormationController>();
            }
        }
        return null;
    }

    public UnitController GetUnit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<UnitController>() != null)
            {
                return hit.collider.gameObject.GetComponent<UnitController>();
            }
        }
        return null;
    }


    /*
     * COPIED FROM HEXMAPEDITOR, NEED TO MOVE THIS LATER
     */
    HexCell GetCellUnderCursor()
    {
        return
            grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    void CreateFormation()
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && !cell.unitController)
        {
            AddFormation(
                Instantiate(FormationController.unitPrefab), cell, Random.Range(0f, 360f)
            );
        }
    }
    void DestroyUnit()
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && cell.unitController)
        {
            RemoveUnit(cell.unitController);
        }
    }
    ///------------------------------------------
    void CreateCombatUnit(UnitController template, int teamNum)
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && !cell.unitController) //If the cell is valid and doesn't already have a controller
        {
            AddUnit(
                Instantiate(template), cell, Random.Range(0f, 360f), template.data, teamNum
            );
         }
    }

    /*
     * Place the captain from a deck
     */
    public void AddCaptain(UnitController controller, bool isPlayerCaptain)
    {
        if (isPlayerCaptain) AddUnit(Instantiate(controller), grid.cells[0], Random.Range(0f, 360f), controller.data, 1);
        else { AddUnit(Instantiate(controller), grid.cells[79], Random.Range(0f, 360f), controller.data, -1); }
    }

    /*
     * does some initialization for the unit controller
     * todo, merge with Initialize function to keep it all in one place
     */
    public void AddUnit(UnitController controller, HexCell location, float orientation, UnitDataStore data, int teamNum)
    {
        controller.data = data;
        controller.teamNum = teamNum;
        MicroBarFollow bars = null;

        if (teamNum == 1)
        {
            firstTeamControllers.Add(controller); //Make sure UnitManager knows about the controller}
            bars = Instantiate(microBars.prefab);
        }
        else if (teamNum == -1)
        {
            secondTeamControllers.Add(controller);
            bars = Instantiate(microBarsEnemy.prefab);
        }
        else
        {
            Debug.Log("Addunit, invalid teamnumber was provided");
        }

        controller.Grid = grid;
        controller.transform.SetParent(transform, false);
        controller.Location = location;
        controller.Orientation = orientation;

        //Setup health and stamina bars
        
        controller.Initialize(this, bars); //Pass itself down, likewise, make sure the unit knows about the manager
       
        controller.PlayEffect(effect, controller.transform.position, 3);
        AudioPlayer.PlayOneShot(AudioDeployedUnit);
    }

    public List<UnitController> GetControllers(int teamNum, bool isSameTeam)
    {
        //Reverse the team we're looking for if false
        teamNum = (isSameTeam ? teamNum : teamNum * -1 );

        if (teamNum == 1) return firstTeamControllers;
        else if (teamNum == -1) return secondTeamControllers;

        Debug.Log("Invalid teamNum provided on GetControllers");
        return null;
    }

}
