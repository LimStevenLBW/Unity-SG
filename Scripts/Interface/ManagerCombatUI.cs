using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models.Unit;

namespace Assets.Scripts.Interface
{
    /*
     * Handles user interaction in combat scene
     */
    class ManagerCombatUI : MonoBehaviour
    {
        public HexGrid grid;
        public UnitWindow unitWindow;
        public CameraControl mainCamera;
        private UnitController priorController;
        private UnitController selectedController;
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
            else if (Input.GetMouseButtonDown(1))
            {
                ClearSelection();
            }
                /*
                else if (selectedController)
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

            ClearSelection();

            UnFocus();
            UpdateSelection();

            if (selectedCell)
            {
                selectedController = selectedCell.unitController;
                
            }
            //Note that a cell may not necessarily have a unit
            if (selectedController)
            {
                EnableHighlight(selectedController);
                EnableUnitWindow(selectedController);

                
            }

            priorController = selectedController;

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

        void ClearSelection()
        {
            DisableHighlight(selectedController);
            DisableUnitWindow();
        }

        void EnableHighlight(UnitController controller)
        {
            if (controller) controller.EnableHighlight();

        }

        void DisableHighlight(UnitController controller)
        {
            if (controller) controller.DisableHighlight();
        }

        void EnableUnitWindow(UnitController controller)
        {
            unitWindow.gameObject.SetActive(true);

            if (controller) {
                unitWindow.Initialize(controller);
                //unitWindow.SetPosition(unit);
                if (priorController != selectedController) Focus(controller.transform);
            }
            PlayAudioClip(AudioClickOpen);
        }
        void DisableUnitWindow()
        {
            PlayAudioClip(AudioClickClose);
            unitWindow.gameObject.SetActive(false);
        }

        void Focus(Transform transform)
        {
            mainCamera.Focus(transform, 50, 50);
        }

        void UnFocus()
        {
            mainCamera.UnFocus();
        }
        public virtual void PlayAudioClip(AudioClip clip)
        {
            AudioPlayer.clip = clip;
            AudioPlayer.Play();
        }
    }
}
