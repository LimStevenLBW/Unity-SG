using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models.Unit;

namespace Assets.Scripts.Interface
{
    /*
     * Handles user interaction in combat scene
     */
    public class ManagerCombatUI : MonoBehaviour
    {
        bool isRoundRunning = false;
        public HexGrid grid;
        public UnitWindow unitWindow;
        public PlayerHandPanel playerHandPanel;

        public CameraControl mainCamera;
        public UnitManager unitManager;

        private UnitController priorController;
        private UnitController selectedController;
        private HexCell selectedCell;

        [SerializeField] private AudioSource AudioPlayer;
        [SerializeField] private AudioClip AudioHover;
        [SerializeField] private AudioClip AudioClickOpen;
        [SerializeField] private AudioClip AudioClickClose;

        void Start()
        {
            //player hand panel is set false to avoid it working before it has data
            if (playerHandPanel.gameObject.activeInHierarchy) playerHandPanel.gameObject.SetActive(false);
        }

        void Update()
        {
            //Check for mouseclick
            
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    
                    if (hit.collider.gameObject.GetComponent<UnitController>() != null) //Check if the hit GameObject is controller
                    {
                        //print(hit.collider.name);

                        UnitController controller = hit.collider.gameObject.GetComponent<UnitController>();
                        if (selectedController != controller)
                        {
                            ClearSelection();
                            mainCamera.UnFocus();
                            selectedController = controller;
                            
                            selectedCell = selectedController.Location;
                        }
                    }
                    else
                    {
                        //Grid handles cell hits, since raycast is unreliable at hitting them w/o help
                        HexCell cell = grid.GetCell(ray);
                        //print(hit.collider.name);
                    
                        ClearSelection();
                        mainCamera.UnFocus();
 

                        //If the hexcell is different from what we have currently selected
                        if (cell) 
                        {
                            if(cell != selectedCell)
                            {
                                selectedCell = cell;
                                selectedController = selectedCell.unitController;
                            }
                            
                        }          
                    }
                    
                    //Note that a cell may not necessarily have a unit
                    if (selectedController)
                    {
                        if(priorController != selectedController)
                        {
                            EnableHighlight(selectedController);
                            EnableUnitWindow(selectedController);
                            priorController = selectedController;
                        }   
                    }
   
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                ClearSelection();
            }

            //If above UI object
            if (EventSystem.current.IsPointerOverGameObject())
            {

            }

            /*
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //As long as the pointer is not above a UI element from the event system, then..
                if (Input.GetMouseButtonDown(0)) //LEFT CLICK
                {
                    print("do selection");
                    DoSelection();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    ClearSelection();
                }
            }
           */
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

        public void StartStage(Deck playerDeck, Deck enemyDeck)
        {
            //If combat stage
           
            
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

            
            UpdateSelection();

            if (selectedCell)
            {
                selectedController = selectedCell.unitController;
                
            }
            //Note that a cell may not necessarily have a unit
            if (selectedController)
            {
                Debug.Log("found a controller?");
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
            /*

         UnitController unit;
             //= grid.GetUnit(ray);


         if (unit)
         {
             cell = unit.Location;
         }
         else
         {
             //Or alternatively, just get the grid cell at mouse position
             
         }

            if (cell != selectedCell) //We have a new selected cell
            {
                selectedCell = cell;
                
                return true;
            }
            */
            return false; //No need to update it if the same thing was selected
        }

        void ClearSelection()
        {
            DisableHighlight(selectedController);
            DisableUnitWindow();
            selectedController = null;
            priorController = null;
            selectedCell = null;    
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

        public virtual void PlayAudioClip(AudioClip clip)
        {
            //AudioPlayer.clip = clip;
            AudioPlayer.PlayOneShot(clip);
        }

    }
}
