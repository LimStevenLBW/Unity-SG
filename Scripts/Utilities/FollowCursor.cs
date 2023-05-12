using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO MAKE FOLLOW CURSOR MORE GENERIC, MOVE LOGIC ELSEWHERE
public class FollowCursor : MonoBehaviour
{
    private UnitController controller;
    private HexCell cell;
    private HexGrid grid;
    public float distance = 30.5f;
    float originY;
    // Start is called before the first frame update
    void Start()
    {
        originY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        ObjectFollowCursor();
    }
    void ObjectFollowCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Vector3 point = ray.origin + (ray.direction * distance);
        //Debug.Log( "World point " + point );

        cell = grid.GetCell(ray);
        if (cell && cell.unitController == null)
        {
            if (cell.cell_ID <= 40)
            {
                transform.position = new Vector3(cell.transform.position.x, cell.transform.position.y + 5, cell.transform.position.z);
            }
        }
        /*
         *  if (controller.teamNum == 1)
            {
                
            else if (controller.teamNum == -1)
            {
                if(cell.cell_ID > 40) transform.position = cell.transform.position;
            }
            else
            {
                Debug.Log("Invalid team num");
            }
         */
 
    }
    public void GetGrid(HexGrid grid)
    {
        this.grid = grid;
    }
    public void GetController(UnitController controller)
    {
        this.controller = controller;
    }

    public bool Reposition()
    {
        if(cell.cell_ID < 41)
        {
            if (cell.unitController != null) return false; //Occupied
            controller.Location = cell;
            controller.UpdateStartingLocation();
            Destroy(this);
            return true;
        }

        return false;
    }
}
