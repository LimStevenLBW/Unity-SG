using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models.Unit;

namespace Assets.Scripts.Interface
{
    /*
     * Handles interaction in combat
     */
    class CombatUI : MonoBehaviour
    {
        public HexGrid grid;
        
        UnitController selectedUnit;
        HexCell selectedCell;

        void Update()
        {
            //As long as the pointer is not above a UI element from the event system, then..
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0)) //LEFT CLICK
                {
                    DoSelection();
                }
                else if (selectedUnit)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        DoMove();
                    }
                    else
                    {
                        DoPathfinding();
                    }
                }
            }
        }

        public void ToggleEditMode()
        {
            //enabled = !enabled;
            //grid.ToggleShowGrid();
            //grid.ShowUI(enabled); 
        }

        /*
         * Handle what the user selects
         */
        void DoSelection()
        {
            grid.ClearPath(); //Clear any current paths

            Debug.Log("clicked");

            DisableHighlight(selectedUnit);

            UpdateSelection();

            if (selectedCell)
            {
                selectedUnit = selectedCell.unitController;
                EnableHighlight(selectedUnit);
            }

        }

        /*
         * A cell can be selected either by clicking the unit or the hexgrid cell itself
         */
        bool UpdateSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Most people will try to select the unit, not the cell.
            UnitController unit = grid.GetUnit(ray);
            HexCell cell;

            //If we get the unit, we can get the cell easily
            if (unit)
            {
                cell = unit.Location;

               // selectedUnit = unit;
            }
            else
            {
                //Or just get the grid cell at mouse position
                cell = grid.GetCell(ray);
            }

            if (cell != selectedCell) //We have a new selected cell
            {
                selectedCell = cell;
                
                return true;
            }

            return false; //No need to update it if the same thing was selected
        }

        void DoPathfinding()
        {
            //If a new cell has been selected
            if (UpdateSelection())
            {
                if (selectedCell && selectedUnit.IsValidDestination(selectedCell))
                {
                    grid.FindPath(selectedUnit.Location, selectedCell, selectedUnit);
                }
                else
                {
                    grid.ClearPath();
                }
            }
        }

        void DoMove()
        {
            if (grid.HasPath)
            {
                //selectedUnit.Location = selectedCell;
                selectedUnit.Travel(grid.GetPath());
                grid.ClearPath();
            }
        }

        void EnableHighlight(UnitController unit)
        {
            if (unit) unit.GetComponent<Outline>().enabled = true;
        }

        void DisableHighlight(UnitController unit)
        {
            if (unit) unit.GetComponent<Outline>().enabled = false;
        }

    }
}
