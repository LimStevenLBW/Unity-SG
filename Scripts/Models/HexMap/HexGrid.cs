using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
    private HexCell[] cells;
    //private HexMesh hexMesh;
    private HexGridChunk[] chunks;

    public int seed = 1234;

    void Awake()
    {
        HexMetrics.noiseSource = noiseSource;
        HexMetrics.InitializeHashGrid(seed);
       // HexMetrics.colors = colors;
        CreateMap(cellCountX, cellCountZ);
        //gridCanvas = GetComponentInChildren<Canvas>();
        //hexMesh = GetComponentInChildren<HexMesh>();
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

        CreateChunks();
        CreateCells();
        cameraControl.UpdateClampPositions(this.cellCountX, this.cellCountZ);

        return true;
    }
    void OnEnable()
    {
        if (!HexMetrics.noiseSource)
        {
            HexMetrics.noiseSource = noiseSource;
            HexMetrics.InitializeHashGrid(seed);
           // HexMetrics.colors = colors;
        }
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
        //cell.Color = defaultColor;

        InitCellConnections(x, z, i, cell);
        CreateLabel(cell, position);
        cell.Elevation = 0;

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
        label.text = cell.coordinates.ToStringOnSeparateLines();
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
     * Return the cell at a given position
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
    public void Save(BinaryWriter writer)
    {
        writer.Write(cellCountX);
        writer.Write(cellCountZ);
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Save(writer);
        }
    }

    public void Load(BinaryReader reader, int header)
    {
        int x = 20, z = 15;
        if (header >= 1)
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

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader);
        }
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].Refresh();
        }
    }

    public void FindDistancesTo(HexCell cell)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Distance =
                cell.coordinates.DistanceTo(cells[i].coordinates) / 2;
        }
    }
}
