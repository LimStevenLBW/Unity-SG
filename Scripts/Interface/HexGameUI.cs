using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models.Unit;

namespace Assets.Scripts.Interface
{
    class HexGameUI : MonoBehaviour
    {
        public HexGrid grid;
        HexCell currentCell;
        HexUnit selectedUnit;

        public void SetEditMode(bool toggle)
        {
            enabled = !toggle;
            //grid.ToggleShowGrid();
            grid.ShowUI(!toggle); 
        }

        bool UpdateCurrentCell()
        {
            HexCell cell =
                grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell != currentCell)
            {
                currentCell = cell;
                return true;
            }
            return false;
        }

        void DoSelection()
        {
            grid.ClearPath();
            UpdateCurrentCell();
            if (currentCell)
            {
                selectedUnit = currentCell.Unit;
            }
        }

        void Update()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
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

        void DoPathfinding()
        {
            if (UpdateCurrentCell())
            {
                if (currentCell && selectedUnit.IsValidDestination(currentCell))
                {
                    grid.FindPath(selectedUnit.Location, currentCell, 24);
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
                //selectedUnit.Location = currentCell;
                selectedUnit.Travel(grid.GetPath());
                grid.ClearPath();
            }
        }

    }
}
