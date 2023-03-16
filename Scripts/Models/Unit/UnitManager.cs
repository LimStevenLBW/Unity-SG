using Assets.Scripts.Models.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        testUnit1.unit.InitUnit();
        testUnit2.unit.InitUnit();
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
                CreateTestUnit(testUnit1);
            }
            return;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                // DestroyTestUnit();
            }
            else
            {
                CreateTestUnit(testUnit2);
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
    //TEMPORARY FUNCTIONS FOR TESTING COMBAT
    void CreateTestUnit(UnitController unit)
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && !cell.unitController)
        {
            AddTestUnit(
                Instantiate(unit.prefab), cell, Random.Range(0f, 360f)
            );
        }
    }
    public void AddTestUnit(UnitController unit, HexCell location, float orientation)
    {
        TestUnits.Add(unit);
        unit.Grid = grid;
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
    }
}
