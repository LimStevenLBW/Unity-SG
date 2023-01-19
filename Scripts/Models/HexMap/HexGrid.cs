using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Assets.Scripts.Models.Unit;

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
    HexCellPriorityQueue searchFrontier;
    int searchFrontierPhase;
    HexCell currentPathFrom, currentPathTo;
    bool currentPathExists;
    List<HexUnit> units = new List<HexUnit>();
    public HexUnit unitPrefab;

    void Awake()
    {
        HexMetrics.noiseSource = noiseSource;
        HexMetrics.InitializeHashGrid(seed);
        //HexMetrics.colors = colors;
        HexUnit.unitPrefab = unitPrefab;
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

        ClearPath();
        ClearUnits();
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
            HexUnit.unitPrefab = unitPrefab;
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

        writer.Write(units.Count);
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Save(writer);
        }
    }

    public void Load(BinaryReader reader, int header)
    {
        ClearPath();
        ClearUnits();
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

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].Load(reader);
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
                HexUnit.Load(reader, this);
            }
        }
    }

    public void FindPath(HexCell fromCell, HexCell toCell, int speed)
    {
        StopAllCoroutines();
        //StartCoroutine(Search(fromCell, toCell, speed));
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        currentPathExists = Search(fromCell, toCell, speed);
        ShowPath(speed);
    }

    /*
     * Breadth-First Search using the selected cell as the tree root
     */
    bool Search(HexCell fromCell, HexCell toCell, int speed)
    {
        searchFrontierPhase += 2;
        if (searchFrontier == null)
        {
            searchFrontier = new HexCellPriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

        /*
        for (int i = 0; i < cells.Length; i++)
        {
           // cells[i].Distance = int.MaxValue; //if distance at max value, hexcell will determine that it should not display coordinates
            cells[i].SetLabel(null); //Hide labels
            cells[i].DisableHighlight();
        }
        fromCell.EnableHighlight(Color.green);
        */

        //WaitForSeconds delay = new WaitForSeconds(1 / 60f);
        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);

        while (searchFrontier.Count > 0)
        {
            //yield return delay;
            HexCell current = searchFrontier.Dequeue();
            current.SearchPhase += 1;

            if (current == toCell)
            {
                return true;
                /*
                while (current != fromCell)
                {
                    int turn = current.Distance / speed;
                    current.SetLabel(turn.ToString());
                    current.EnableHighlight(Color.blue);
                    current = current.PathFrom;
                }
                toCell.EnableHighlight(Color.red);
                break; //We can stop once we find destination cell
                */
            }

            int currentTurn = current.Distance / speed;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase)
                {
                    continue;
                }
                if (neighbor.IsUnderwater || neighbor.Unit) //Go around water and units
                {
                    continue;
                }

                HexEdgeType edgeType = current.GetEdgeType(neighbor);
                if (edgeType == HexEdgeType.Cliff)
                {
                    continue;
                }

                int moveCost;
                if (current.HasRoadThroughEdge(d))
                {
                    moveCost = 1;
                }
                else if (current.Walled != neighbor.Walled)
                {
                    continue; //Walls block movement
                }
                else
                {
                    moveCost = edgeType == HexEdgeType.Flat ? 5 : 10;              //MOVEMENT COSTS
                    moveCost += neighbor.UrbanLevel + neighbor.FarmLevel +
                        neighbor.PlantLevel;
                }

                int distance = current.Distance + moveCost;
                int turn = distance / speed;
                if (turn > currentTurn)
                {
                    distance = turn * speed + moveCost;
                }

                //add a neighbor to the frontier. When a cell is added, it is not guaranteed to be the shortest distance, we need to check
                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    //neighbor.SetLabel(turn.ToString());
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    //neighbor.SetLabel(turn.ToString());
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }

                /*
                frontier.Sort(
                    (x, y) => x.SearchPriority.CompareTo(y.SearchPriority)
                );
                */
            }
           
        }
        return false;
    }
    void ShowPath(int speed)
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                int turn = current.Distance / speed;
                current.SetLabel(turn.ToString());
                current.EnableHighlight(Color.white);
                current = current.PathFrom;
            }
        }
        currentPathFrom.EnableHighlight(Color.blue);
        currentPathTo.EnableHighlight(Color.red);
    }

    public void ClearPath()
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                current.SetLabel(null);
                current.DisableHighlight();
                current = current.PathFrom;
            }
            current.DisableHighlight();
            currentPathExists = false;
        }
        else if (currentPathFrom)
        {
            currentPathFrom.DisableHighlight();
            currentPathTo.DisableHighlight();
        }
        currentPathFrom = currentPathTo = null;
    }

    void ClearUnits()
    {
        for (int i = 0; i < units.Count; i++)
        {
            units[i].Die();
        }
        units.Clear();
    }
    public void AddUnit(HexUnit unit, HexCell location, float orientation)
    {
        units.Add(unit);
        unit.transform.SetParent(transform, false);
        unit.Location = location;
        unit.Orientation = orientation;
    }

    public void RemoveUnit(HexUnit unit)
    {
        units.Remove(unit);
        unit.Die();
    }
    public HexCell GetCell(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return GetCell(hit.point);
        }
        return null;
    }

    public void ShowUI(bool visible)
    {
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i].ShowUI(visible);
        }
    }

    public bool HasPath
    {
        get
        {
            return currentPathExists;
        }
    }
}
