using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models.Unit;

namespace Assets.Scripts.Interface
{
    /*
     * Handles interaction in combat
     */
    class ManagerCombatUI : MonoBehaviour
    {
        public HexGrid grid;
        public UnitWindow unitWindow;
        public CameraControl mainCamera;
        private UnitController selectedUnit;
        private HexCell selectedCell;

        [SerializeField] private AudioSource AudioPlayer;
        [SerializeField] private AudioClip AudioHover;
        [SerializeField] private AudioClip AudioClickOpen;
        [SerializeField] private AudioClip AudioClickClose;

        void Update()
        {
            //As long as the pointer is not above a UI element from the event system, then..
            //if (!EventSystem.current.IsPointerOverGameObject())
           // {
                if (Input.GetMouseButtonDown(0)) //LEFT CLICK
                {
                    DoSelection();
                }
                /*
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
                */
           // }
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

            DisableHighlight(selectedUnit);
            DisableUnitWindow();

            UpdateSelection();

            if (selectedCell)
            {
                selectedUnit = selectedCell.unitController;
                
            }
            //Note that a cell may not necessarily have a unit
            if (selectedUnit)
            {
                EnableHighlight(selectedUnit);
                EnableUnitWindow(selectedUnit);
            }

        }

        /*
         * A cell can be selected either by clicking the unit or the hexgrid cell itself
         */
        bool UpdateSelection()
        {
            //grid.ClearPath(); //Clear any current paths
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            HexCell cell;

            //Check if a unit/cell was clicked. Most people will try to select the unit, not the cell.
            //If we have the unit, we can get the cell easily
            UnitController unit = grid.GetUnit(ray);

            if (unit)
            {
                cell = unit.Location;
            }
            else
            {
                //Or alternatively, just get the grid cell at mouse position
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

        void EnableUnitWindow(UnitController unit)
        {
            unitWindow.gameObject.SetActive(true);

            if (unit) {
                unitWindow.SetValues(unit);
                //unitWindow.SetPosition(unit);
                mainCamera.Focus(unit.transform, 50, 50);
            }
            PlayAudioClip(AudioClickOpen);
        }
        void DisableUnitWindow()
        {
            mainCamera.UnFocus();
            PlayAudioClip(AudioClickClose);
            unitWindow.gameObject.SetActive(false);
        }
        public virtual void PlayAudioClip(AudioClip clip)
        {
            AudioPlayer.clip = clip;
            AudioPlayer.Play();
        }
    }
}
