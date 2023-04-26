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
    //Temporary for creating Units
    public List<FormationController> units = new List<FormationController>();
    public FormationController unitPrefab;

    //Fielded Controllers
    public List<UnitController> firstTeamControllers = new List<UnitController>();
    public List<UnitController> secondTeamControllers = new List<UnitController>();

    public Queue<UnitDataStore> deployableUnits;
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
        
    }

    // Update is called once per frame
    void Update()
    {
       
        //Deploy my units
        if(Director.Instance.GetPhase() == "DEPLOYMENT")
        {
            bool wasDeployed = false;
            if (currentController == null && deployableUnits.Count > 0)
            {

                currentController = deployableUnits.Dequeue().controller;

                for (int i = 0; i < 40; i++)
                {
                    if (grid.cells[i].unitController == null)
                    {
                        AddUnit(Instantiate(currentController), grid.cells[i], Random.Range(0f, 360f), currentController.data, 1);
                        wasDeployed = true;
                    }

                    if (wasDeployed) break;
                }
                currentController = null;
            }
            
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
            if (PATHFINDING_IN_USE) continue;
            PATHFINDING_IN_USE = true;
            break;
        }

        //We cannot edit anything that affects unit pathfinding without permission first

        if (teamNum == 1)
        {
            firstTeamControllers.Remove(controller);
        }
        else if (teamNum == -1)
        {
            secondTeamControllers.Remove(controller);
        }
        else
        {
            Debug.Log("Invalid Team Number provided on RemoveUnit");
        }

        controller.Die();
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
