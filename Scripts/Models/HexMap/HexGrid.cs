using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.Models.HexMap.HexHelpers;

public class HexGrid : MonoBehaviour
{
    private int chunkCountX, chunkCountZ;
    public int cellCountX = 20, cellCountZ = 15;
    
    //public Color defaultColor = Color.white;
    public Color[] colors;

    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    public Texture2D noiseSource;
    public HexGridChunk chunkPrefab;
    public CameraControl cameraControl;

    //private Canvas gridCanvas;
    public HexCell[] cells;
    //private HexMesh hexMesh;
    private HexGridChunk[] chunks;

    public int seed = 1234;

    //Unit Manager contains interaction involving units and formations
    public UnitManager unitManager;

    HexCellShaderData cellShaderData;

    void Awake()
    {
        HexMetrics.noiseSource = noiseSource;
        HexMetrics.InitializeHashGrid(seed);

        cellShaderData = gameObject.AddComponent<HexCellShaderData>();

        cellShaderData.Grid = this;
        CreateMap(cellCountX, cellCountZ);

        unitManager.InitGrid(this);
        
        //gridCanvas = GetComponentInChildren<Canvas>();
        //hexMesh = GetComponentInChildren<HexMesh>();
    }

    void OnEnable()
    {
        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexMetrics.InitializeHashGrid(seed);

            //ResetVisibility(); Reset visibility for
        }
    }

    public bool CreateMap(int cellCountX, int cellCountZ)
    {
        if (
            cellCountX <= 0 || cellCountX % HexMetrics.chunkSizeX != 0 ||
            cellCountZ <= 0 || cellCountZ % HexMetrics.chunkSizeZ != 0
        )
        {
            Debug.LogError("Unsupported map size.");
            return false;
        }

        //ClearPath(); clear any overworld paths
        unitManager.ClearUnits();

        if (chunks != null)
        {
            for (int i = 0; i < chunks.Length; i++)
            {
                Destroy(chunks[i].gameObject);
            }
        }

        this.cellCountX = cellCountX;
        this.cellCountZ = cellCountZ;
        chunkCountX = this.cellCountX / HexMetrics.chunkSizeX;
        chunkCountZ = this.cellCountZ / HexMetrics.chunkSizeZ;
        cellShaderData.Initialize(cellCountX, cellCountZ);

        CreateChunks();
        CreateCells();
        cameraControl.UpdateClampPositions(this.cellCountX, this.cellCountZ);

        return true;
    }

    void CreateChunks()
    {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++)
        {
            for (int x = 0; x < chunkCountX; x++)
            {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    void CreateCells()
    {
        cells = new HexCell[cellCountZ * cellCountX];

        for (int z = 0, i = 0; z < cellCountZ; z++)
        {
            for (int x = 0; x < cellCountX; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    /*
    * Instantiates a Hex Cell Prefab
    */
    void CreateCell(int x, int z, int i)
    {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.Index = i;
        cell.ShaderData = cellShaderData;
        //cell.Color = defaultColor;

        InitCellConnections(x, z, i, cell);
        CreateLabel(cell, position);
        cell.Elevation = 0;

        //Fade out the edges of the map
        cell.Explorable =
            x > 0 && z > 0 && x < cellCountX - 1 && z < cellCountZ - 1;

        AddCellToChunk(x, z, cell);
    }

    /**
     * We can find the correct chunk via integer divisions of x and z by the chunk sizes.
     * we can also determine the cell's index local to its chunk. Once we have that, we can add the cell to the chunk.
     */
    void AddCellToChunk(int x, int z, HexCell cell)
    {
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
    }
    /**
     * Establish HexCell Neightbor links
     */
    private void InitCellConnections(int x, int z, int i, HexCell cell)
    {
        if (x > 0) // Cells on the far left of the grid have no western neightbor
        {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        if (z > 0)
        {
            if ((z & 1) == 0) // IF Z is EVEN
            {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if (x > 0)
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else // OR ODD
            {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }
    }

    void CreateLabel(HexCell cell, Vector3 position)
    {
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines(); //Display Cell Coordinates
        cell.uiRect = label.rectTransform;
    }

    // Start is called before the first frame update
    void Start()
    {
        //hexMesh.Triangulate(cells);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            HandleInput();
        }
    }

    void HandleInput()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(inputRay, out hit))
        {
            //ColorCell(hit.point, touchedColor);
        }
    }

    /**
     * Return the hex cell at the given position
     */
    public HexCell GetCell(Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        //Debug.Log("touched at " + position);

        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        // HexCell cell = cells[index];
        //cell.color = color;
        //hexMesh.Triangulate(cells);
        return cells[index];
    }

    /**
     * Return the hex cell at the given coordinates
     */
    public HexCell GetCell(HexCoordinates coordinates)
    {
        int z = coordinates.Z;
        if (z < 0 || z >= cellCountZ)
        {
            return null;
        }
        int x = coordinates.X + z / 2;
        if (x < 0 || x >= cellCountX)
        {
            return null;
        }
        return cells[x + z * cellCountX];
    }

    /**
     * Return the hex cell pointed to by raycast
     */
    public HexCell GetCell(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return GetCell(hit.point);
        }
        return null;
    }

    public FormationController GetFormation(Ray ray)
    {
        //return unitManager.GetUnit(ray);
        return null;
    }

    public UnitController GetUnit(Ray ray)
    {
        return unitManager.GetUnit(ray);
    }

    public void Save(BinaryWriter writer)
    {
        writer.Write(cellCountX);
        writer.Write(cellCountZ);
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Save(writer);
        }

        writer.Write(unitManager.units.Count);
        for (int i = 0; i < unitManager.units.Count; i++)
        {
            unitManager.units[i].Save(writer);
        }
    }

    public void Load(BinaryReader reader, int header)
    {
        //ClearPath(); clear any overworld paths
        unitManager.ClearUnits();
        //StopAllCoroutines();
        int x = 20, z = 15;
        if (header >= 2)
        {
            x = reader.ReadInt32();
            z = reader.ReadInt32();
        }

        if (x != cellCountX || z != cellCountZ)
        {
            if (!CreateMap(x, z))
            {
                return;
            }
        }

        bool originalImmediateCellReveal = cellShaderData.isImmediateCellReveal;
        cellShaderData.isImmediateCellReveal = true;
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader, header);
        }
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Refresh();
        }

        if (header >= 2)
        {
            int unitCount = reader.ReadInt32();
            for (int i = 0; i < unitCount; i++)
            {
                FormationController.Load(reader, this);
            }
        }

        cellShaderData.isImmediateCellReveal = originalImmediateCellReveal;
    }

    public void ShowUI(bool visible)
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].ShowUI(visible);
        }
    }

    public void ClearAllPaths()
    {
        //Request all known units to clear paths
    }

    public void ResetVisibility()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].ResetVisibility();
        }
        for (int i = 0; i < unitManager.units.Count; i++)
        {
            FormationController unit = unitManager.units[i];
            unit.path.IncreaseVisibility(unit.Location, unit.VisionRange);
        }
    }
}
