using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using Assets.Scripts.Models.Unit;

public class HexMapEditor : MonoBehaviour
{
    enum OptionalToggle { Ignore, Yes, No }
    OptionalToggle riverMode, roadMode, walledMode;

    //public Color[] colors;
    public Material terrainMaterial;
    public HexGrid hexGrid;
    private int brushSize;
    //private Color activeColor;

    private int activeTerrainTypeIndex = -1;
    private int activeElevation;
    private int activeWaterLevel = 1;
    private int activeUrbanLevel, activeFarmLevel, activePlantLevel, activeSpecialIndex;

    //bool applyColor;

    bool isDrag;
    bool applyElevation;
    bool applyWaterLevel;
    bool applyUrbanLevel;
    bool applyFarmLevel, applyPlantLevel, applySpecialIndex, showGrid;
    bool editMode = true;

    HexDirection dragDirection;
    HexCell previousCell;

    void Awake()
    {
        //SetFogOfWar();
    }

    // Start is called before the first frame update
    void Start()
    {
        showGrid = true;
        if (showGrid)
        {

            terrainMaterial.EnableKeyword("GRID_ON");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                HandleInput();
                return;
            }
        }
        previousCell = null;
    }

    void HandleInput()
    {
        HexCell selectedCell = GetCellUnderCursor();

        if (selectedCell)
        {

            if (previousCell && previousCell != selectedCell)
            {
                ValidateDrag(selectedCell);
            }
            else
            {
                isDrag = false;
            }

            EditCells(selectedCell);


            previousCell = selectedCell;
        }
        else
        {
            previousCell = null;
        }
    }

    HexCell GetCellUnderCursor()
    {
        return
            hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }


    void EditCell(HexCell cell)
    {
        if (cell)
        {
            /*
            if (applyColor)
            {
                cell.Color = activeColor;
            }
            */
            if (activeTerrainTypeIndex >= 0)
            {
                cell.TerrainTypeIndex = activeTerrainTypeIndex;
            }
            if (applyElevation)
            {
                cell.Elevation = activeElevation;
            }
            if (applyWaterLevel)
            {
                cell.WaterLevel = activeWaterLevel;
            }
            if (applySpecialIndex)
            {
                cell.SpecialIndex = activeSpecialIndex;
            }
            if (applyUrbanLevel)
            {
                cell.UrbanLevel = activeUrbanLevel;
            }
            if (applyFarmLevel)
            {
                cell.FarmLevel = activeFarmLevel;
            }
            if (applyPlantLevel)
            {
                cell.PlantLevel = activePlantLevel;
            }
            if (riverMode == OptionalToggle.No)
            {
                cell.RemoveRiver();
            }
            if (roadMode == OptionalToggle.No)
            {
                cell.RemoveRoads();
            }
            if (walledMode != OptionalToggle.Ignore)
            {
                cell.Walled = walledMode == OptionalToggle.Yes;
            }

            ///bool editing = (activeTerrainTypeIndex > -1 || applyElevation || applyWaterLevel || applySpecialIndex || applyUrbanLevel
            //    || applyFarmLevel || applyPlantLevel || riverMode != OptionalToggle.Ignore || roadMode != OptionalToggle.Ignore || walledMode != OptionalToggle.Ignore);
           

            if (isDrag)
            {
                HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
                if (otherCell)
                {
                    if (riverMode == OptionalToggle.Yes)
                    {
                        otherCell.SetOutgoingRiver(dragDirection);
                    }
                    if (roadMode == OptionalToggle.Yes)
                    {
                        otherCell.AddRoad(dragDirection);
                    }
                }
            }
        }
        //hexGrid.Refresh();
    }
    void EditCells(HexCell center)
    {
        int centerX = center.coordinates.X;
        int centerZ = center.coordinates.Z;

        for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
        {
            for (int x = centerX - r; x <= centerX + brushSize; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
        for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
        {
            for (int x = centerX - brushSize; x <= centerX + r; x++)
            {
                EditCell(hexGrid.GetCell(new HexCoordinates(x, z)));
            }
        }
    }
 
    void ValidateDrag(HexCell currentCell)
    {
        for (
            dragDirection = HexDirection.NE;
            dragDirection <= HexDirection.NW;
            dragDirection++
        )
        {
            if (previousCell.GetNeighbor(dragDirection) == currentCell)
            {
                isDrag = true;
                return;
            }
        }
        isDrag = false;
    }
    public void SetBrushSize(float size)
    {
        brushSize = (int)size;
    }

    public void SetElevation(float elevation)
    {
        activeElevation = (int)elevation;
    }

    /*
    public void SelectColor(int index)
    {
        applyColor = index >= 0;
        if (applyColor)
        {
            activeColor = colors[index];
        }
    }
    */
    public void SetTerrainTypeIndex(int index)
    {
        activeTerrainTypeIndex = index;
    }
    public void ToggleApplyElevation()
    {
        applyElevation = !applyElevation;
    }
    public void ToggleApplyWaterLevel()
    {
        applyWaterLevel = !applyWaterLevel;
    }
    public void ToggleApplyUrbanLevel()
    {
        applyUrbanLevel = !applyUrbanLevel;
    }

    public void ToggleApplyFarmLevel()
    {
        applyFarmLevel = !applyFarmLevel;
    }
    public void ToggleApplyPlantLevel()
    {
        applyPlantLevel = !applyPlantLevel;
    }
    public void ToggleApplySpecialIndex()
    {
        applySpecialIndex = !applySpecialIndex;
    }

    public void SetRiverMode(int mode)
    {
        riverMode = (OptionalToggle)mode;
    }
    public void SetRoadMode(int mode)
    {
        roadMode = (OptionalToggle)mode;
    }
    public void SetWalledMode(int mode)
    {
        walledMode = (OptionalToggle)mode;
    }

    public void SetWaterLevel(float level)
    {
        activeWaterLevel = (int)level;
    }
    public void SetSpecialIndex(float index)
    {
        activeSpecialIndex = (int)index;
    }

    public void SetUrbanLevel(float level)
    {
        activeUrbanLevel = (int)level;
    }
    public void SetFarmLevel(float level)
    {
        activeFarmLevel = (int)level;
    }
    public void SetPlantLevel(float level)
    {
        activePlantLevel = (int)level;
    }

    public void ToggleShowGrid()
    {
        showGrid = !showGrid;
        if (showGrid)
        {
         
            terrainMaterial.EnableKeyword("GRID_ON");
        }
        else
        {
            terrainMaterial.DisableKeyword("GRID_ON");
        }
    }
    public void ToggleEditMode() //Invoked same time as HexGameUI's version, will need to be cleaned up
    {
        editMode = !editMode;
        SetFogOfWar();
        //grid.ShowUI(!toggle);
        hexGrid.ClearAllPaths();
    }
    public void ShowUI(bool visible)
    {
        //hexGrid.ShowUI(visible);
    }

    public void SetFogOfWar()
    {
        if (editMode)
        {
            Shader.EnableKeyword("HEX_MAP_EDIT_MODE");
        }
        else
        {
            Shader.DisableKeyword("HEX_MAP_EDIT_MODE");
        }
    }
}
