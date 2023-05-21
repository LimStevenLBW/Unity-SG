﻿using UnityEngine;
using UnityEngine.EventSystems;
using Assets.Scripts.Models.Unit;

namespace Assets.Scripts.Interface
{
    /*
     * Handles user interaction in combat scene
     */
    public class ManagerCombatUI : MonoBehaviour
    {
        public HexGrid grid;

        public GameObject header;
        public UnitWindow unitWindow;
        public PlayerHandPanel playerHandPanel;

        public CameraControl mainCamera;
        public UnitManager unitManager;

        private UnitController priorController;
        private UnitController selectedController;
        private HexCell selectedCell;
        public GameObject startCombatButton;

        [SerializeField] private AudioSource AudioPlayer;
        [SerializeField] private AudioClip AudioClickSelect;
        [SerializeField] private AudioClip AudioLift;
        [SerializeField] private AudioClip AudioDrop;
        [SerializeField] private AudioClip AudioClipDeselect;

        void Start()
        {
            //player hand panel is set false to avoid it working before it has data
            if (playerHandPanel.gameObject.activeInHierarchy) playerHandPanel.gameObject.SetActive(false);
        }

        void Update()
        {
            //Disable input when  in intro and conclusion phases
            if(Director.Instance.GetPhase() != "INTRO" && Director.Instance.GetPhase() != "CONCLUSION")
            {
                if (Director.Instance.GetPhase() == "REPOSITIONING") HandleRepositioning();
                else { HandleNormalInput(); }
            }

        }

        public void DisplayHeader()
        {
            header.SetActive(true);
        }

        public void HideHeader()
        {
            header.SetActive(false);
        }

        public void ClearSelectionKeepWindow()
        {
            DisableHighlight(selectedController);
            selectedController = null;
        }

        private void HandleRepositioning()
        {
            //Check for mouseclick
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    //Check if the hit GameObject is controller
                    if (!selectedController && hit.collider.gameObject.GetComponent<UnitController>() != null) 
                    {
                        UnitController controller = hit.collider.gameObject.GetComponent<UnitController>();
                        selectedController = controller;
                        LiftAndSelect(controller);
                        AudioPlayer.PlayOneShot(AudioLift);
                    }
                    else if (selectedController)
                    {
                        //Temporary fix, for if the player selected enemy controllers
                        if(selectedController.teamNum == -1)
                        {
                            DisableHighlight(selectedController);
                            DisableUnitWindow();
                            selectedController = null;

                            if(hit.collider.gameObject.GetComponent<UnitController>() != null)
                            {
                                UnitController controller = hit.collider.gameObject.GetComponent<UnitController>();
                                selectedController = controller;
                                LiftAndSelect(controller);
                                AudioPlayer.PlayOneShot(AudioLift);
                            }
                            return;
                        }

                        FollowCursor following = selectedController.gameObject.GetComponent <FollowCursor>();     
                        bool wasSet = following.Reposition();
                        if (wasSet)
                        {
                            DisableHighlight(selectedController);
                            DisableUnitWindow();
                            selectedController = null;
                            AudioPlayer.PlayOneShot(AudioDrop);
                            startCombatButton.SetActive(true);
                        }

                    }
                    else
                    {
                        HexCell cell = grid.GetCell(ray);

                        if(cell) selectedController = cell.unitController;
                        if (selectedController)
                        {
                            LiftAndSelect(selectedController);
                            AudioPlayer.PlayOneShot(AudioLift);
 
                        }
                        
                    }
                }
            }
        }

        private void LiftAndSelect(UnitController controller)
        {
            EnableHighlight(selectedController);
            unitWindow.gameObject.SetActive(true);
            unitWindow.Initialize(controller);

            if(controller.teamNum != 1)
            {
                return;
            }

            FollowCursor following = controller.gameObject.AddComponent<FollowCursor>();
            following.GetGrid(grid);
            following.GetController(controller);
            controller.Location = null;
            startCombatButton.SetActive(false);
        }

        private void HandleNormalInput()
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
                            if (cell != selectedCell)
                            {
                                selectedCell = cell;
                                selectedController = selectedCell.unitController;
                            }

                        }
                    }

                    //Note that a cell may not necessarily have a unit
                    if (selectedController)
                    {
                        if (priorController != selectedController)
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
         * A cell can be selected either by clicking the unit or the hexgrid cell itself
         */
        bool UpdateSelection()
        {
            //grid.ClearPath(); //Clear any current paths
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //HexCell cell;

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
            PlayAudioClip(AudioClickSelect);

        }
        public void DisableUnitWindow()
        {
            if(selectedController) PlayAudioClip(AudioClipDeselect);
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
