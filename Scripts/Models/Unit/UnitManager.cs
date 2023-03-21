using Assets.Scripts.Models.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Works with Hexgrid to handle instantiating units
 */
public class UnitManager : MonoBehaviour
{
    //Temporary for creating Units
    public List<PlayerFormation> units = new List<PlayerFormation>();
    public PlayerFormation unitPrefab;

    //TEST FUNCTIONS FOR UNIT CONTROLLER
    public List<UnitController> TestUnits = new List<UnitController>();
    public UnitController testUnit1;
    public UnitController testUnit2;

    public HexGrid grid;

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
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
               // DestroyTestUnit();
            }
            else
            {
                CreateCombatUnit(testUnit1);
            }
            return;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // DestroyCombatUnit();
            }
            else
            {
                CreateCombatUnit(testUnit2);
            }
            return;
        }
    }

    public void ClearUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Die();
        }
        units.Clear();
    }

    public void AddFormation(PlayerFormation unit, HexCell location, float orientation)
    {
        units.Add(unit);
        unit.Grid = grid;
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
    }

    public void RemoveUnit(UnitController unit)
    {
        TestUnits.Remove(unit);
        unit.Die();
    }

    public PlayerFormation GetFormation(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<PlayerFormation>() != null)
            {
                return hit.collider.gameObject.GetComponent<PlayerFormation>();
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
                Instantiate(PlayerFormation.unitPrefab), cell, Random.Range(0f, 360f)
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
    void CreateCombatUnit(UnitController unitController)
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && !cell.unitController) //If the cell is valid and doesn't already have a controller
        {
            AddUnit(
                Instantiate(unitController.prefab), cell, Random.Range(0f, 360f)
            );

            
            unitController.Initialize(this); //Pass itself down, likewise, make sure the unit knows about the manager
         }
    }
    public void AddUnit(UnitController controller, HexCell location, float orientation)
    {
        TestUnits.Add(controller); //Make sure UnitManager knows about the controller, for testing purposes

        controller.Grid = grid;
        controller.transform.SetParent(transform, false);
        controller.Location = location;
        controller.Orientation = orientation;
    }

    /*
     * Locate the nearest enemy that can be pathed to
     */
    public void FindNearestEnemy(UnitController controller)
    {
        Debug.Log("Reached");
        /*
        //Let's go through our list of controllers
        for(int i=0; i<TestUnits.Count; i++)
        {
            //Find the route to that selected controller
            grid.FindPath(controller.Location, TestUnits[i].Location, TestUnits[i]);

            //Store the shortest route
        }
        */

        //Have that controller do path finding?
    }

    void DoPathfinding(UnitController controller, HexCell targetCell)
    {
        if (targetCell && controller.IsValidDestination(targetCell))
        {
            grid.FindPath(controller.Location, targetCell, controller);
        }
        else
        {
            grid.ClearPath();
        }
        
    }

    void DoMove(UnitController controller)
    {
        if (grid.HasPath)
        {
            //controller.Location = selectedCell;
            controller.Travel(grid.GetPath());
            grid.ClearPath();
        }
    }
}
