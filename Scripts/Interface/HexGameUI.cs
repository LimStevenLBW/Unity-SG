﻿using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models.Unit;

namespace Assets.Scripts.Interface
{
    /*
     * Handles interaction in the overworld
     */
    class HexGameUI : MonoBehaviour
    {
        public HexGrid grid;
        
        HexUnit selectedUnit;
        HexCell selectedCell;

        void Update()
        {
            //As long as the pointer is not above a UI element from the event system...
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


            UpdateSelectedCell();

            if (selectedCell)
            {
                selectedUnit = selectedCell.Unit;
            }
        }

        /*
         * A cell can be selected either by clicking the unit or the hexgrid cell itself
         */
        bool UpdateSelectedCell()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Most people will try to select the unit, not the cell.
            HexUnit unit = grid.GetUnit(ray);
            HexCell cell;

            if (unit)
            {
                cell = unit.Location;
                Debug.Log("Casted");
            }
            else
            {
                //Get the grid cell at mouse position
                cell = grid.GetCell(ray);
            }

            if (cell != selectedCell)
            {
                selectedCell = cell;
                return true;
            }

            return false;
        }

        void DoPathfinding()
        {
            //If a new cell has been selected
            if (UpdateSelectedCell())
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

    }
}
