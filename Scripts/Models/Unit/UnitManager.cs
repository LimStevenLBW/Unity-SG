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

    //TEST FUNCTIONS FOR UNIT CONTROLLER
    public List<UnitController> controllers = new List<UnitController>();
    public UnitController testUnit1;
    public UnitController testUnit2;
    public UnitController testUnit3;
    public HexGrid grid;

    public bool PATHFINDING_IN_USE = false;

    
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
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // DestroyCombatUnit();
            }
            else
            {
                CreateCombatUnit(testUnit3);
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

    public void AddFormation(FormationController unit, HexCell location, float orientation)
    {
        units.Add(unit);
        unit.Grid = grid;
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
    }

    public void RemoveUnit(UnitController unit)
    {
        controllers.Remove(unit);
        unit.Die();
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
    void CreateCombatUnit(UnitController unitController)
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && !cell.unitController) //If the cell is valid and doesn't already have a controller
        {

            AddUnit(
                Instantiate(unitController.prefab), cell, Random.Range(0f, 360f)
            );
  
         }
    }
    public void AddUnit(UnitController controller, HexCell location, float orientation)
    {
        controllers.Add(controller); //Make sure UnitManager knows about the controller, for testing purposes
        
        controller.Grid = grid;
        controller.transform.SetParent(transform, false);
        controller.Location = location;
        controller.Orientation = orientation;
        controller.Initialize(this); //Pass itself down, likewise, make sure the unit knows about the manager
    }

   
}
