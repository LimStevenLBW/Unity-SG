using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Assets.Scripts.Models.Unit;
using Assets.Scripts.Models.HexMap.HexHelpers;

public class HexCell : MonoBehaviour
{
    public int cell_ID;

    public HexCoordinates coordinates;
    public RectTransform uiRect; //Track Label Location
    public HexGridChunk chunk;

    [SerializeField]
    private bool[] roads;

    bool walled; // Because the walls are placed in between cells, we have to refresh both the edited cell and its neighbors.

    private bool hasIncomingRiver, hasOutgoingRiver;
    private HexDirection incomingRiver, outgoingRiver;
    int terrainTypeIndex;

    private int elevation = int.MinValue; //Lowest value an integer can have, just to avoid skipping first computation
    int urbanLevel, farmLevel, plantLevel, waterLevel;
    int specialIndex; // determine the special feature it has, if any.
    int distance; //From this cell to a selected origin cell

    //outdated?
    //0 means the cell has not yet been reached, 
    //1 indicated that the cell is currently in the frontier, 
    //2 means it has been taken out of the frontier.
    public int SearchPhase { get; set; }

    public bool Explorable { get; set; }

    public int Index { get; set; }
    public FormationController formationController { get; set; }
    public UnitController unitController { get; set; }

    private int visibility;

    bool explored;

    public bool IsExplored
    {
        get
        {
            return explored && Explorable;
        }
        private set
        {
            explored = value;
        }
    }

    public void ResetSearchPriority()
    {
        SearchPhase = 0;
    }

    public HexCellShaderData ShaderData {
        get;
        set;
    }

    public int Elevation
    {
        get { return elevation; }
        set {
            if (elevation == value) return; // Skip computation if no change

            int originalViewElevation = ViewElevation;
            elevation = value;

            if (ViewElevation != originalViewElevation)
            {
                ShaderData.ViewElevationChanged();
            }

            RefreshPosition();
            ValidateRivers();

            for (int i = 0; i < roads.Length; i++) //If an elevation difference has become too great, an existing road has to be removed.
            {
                if (roads[i] && GetElevationDifference((HexDirection)i) > 1)
                {
                    SetRoad(i, false);
                }
            }
            Refresh();
        }
    }
    public int UrbanLevel
    {
        get
        {
            return urbanLevel;
        }
        set
        {
            if (urbanLevel != value)
            {
                urbanLevel = value;
                RefreshSelfOnly();
            }
        }
    }
    public int FarmLevel
    {
        get
        {
            return farmLevel;
        }
        set
        {
            if (farmLevel != value)
            {
                farmLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    public int PlantLevel
    {
        get
        {
            return plantLevel;
        }
        set
        {
            if (plantLevel != value)
            {
                plantLevel = value;
                RefreshSelfOnly();
            }
        }
    }

    public bool IsVisible
    {
        get
        {
            return visibility > 0 && Explorable;
        }
    }

    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
            //UpdateDistanceLabel();
        }
    }

    public float StreamBedY
    {
        get
        { //Get vertical position of its stream bed
            return
                (elevation + HexMetrics.streamBedElevationOffset) *
                HexMetrics.elevationStep;
        }
    }
    //Retrieve vertical pos of its river surface
    public float RiverSurfaceY
    {
        get
        {
            return
                (elevation + HexMetrics.waterElevationOffset) *
                HexMetrics.elevationStep;
        }
    }
    public float WaterSurfaceY
    {
        get
        {
            return
                (waterLevel + HexMetrics.waterElevationOffset) *
                HexMetrics.elevationStep;
        }
    }
    /*
    public Color Color
    {
        get
        {
            return HexMetrics.colors[terrainTypeIndex];
        }

        set
        {
            if (color == value) return;

            color = value;
            Refresh();
        }
       
    } */
    public int TerrainTypeIndex
    {
        get
        {
            return terrainTypeIndex;
        }
        set
        {
            if (terrainTypeIndex != value)
            {
                terrainTypeIndex = value;

                ShaderData.RefreshTerrain(this);
            }
        }
    }

    /**
     * Every hexcell keeps track of its own waterlevel
     */
    public int WaterLevel
    {
        get
        {
            return waterLevel;
        }
        set
        {
            if (waterLevel == value)
            {
                return;
            }

            int originalViewElevation = ViewElevation;
            waterLevel = value;
            if (ViewElevation != originalViewElevation)
            {
                ShaderData.ViewElevationChanged();
            }


            ValidateRivers();
            Refresh();
        }
    }

    public int SpecialIndex
    {
        get
        {
            return specialIndex;
        }
        set
        {
            if (specialIndex != value && !HasRiver)
            {
                specialIndex = value;
                RemoveRoads();
                RefreshSelfOnly();
            }
        }
    }

    public bool IsSpecial
    {
        get
        {
            return specialIndex > 0;
        }
    }

    public bool IsUnderwater
    {
        get
        {
            return waterLevel > elevation;
        }
    }
    public bool HasIncomingRiver
    {
        get
        {
            return hasIncomingRiver;
        }
    }
    public bool HasOutgoingRiver
    {
        get
        {
            return hasOutgoingRiver;
        }
    }
    public bool HasRiver
    {
        get
        {
            return hasIncomingRiver || hasOutgoingRiver;
        }
    }
    public bool HasRiverBeginOrEnd
    {
        get
        {
            return hasIncomingRiver != hasOutgoingRiver;
        }
    }
    public bool HasRiverThroughEdge(HexDirection direction)
    {
        return
            hasIncomingRiver && incomingRiver == direction ||
            hasOutgoingRiver && outgoingRiver == direction;
    }

    public HexDirection IncomingRiver
    {
        get
        {
            return incomingRiver;
        }
    }
    public HexDirection OutgoingRiver
    {
        get
        {
            return outgoingRiver;
        }
    }
    public HexDirection RiverBeginOrEndDirection
    {
        get
        {
            return hasIncomingRiver ? incomingRiver : outgoingRiver;
        }
    }
    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }
    public bool Walled
    {
        get
        {
            return walled;
        }
        set
        {
            if (walled != value)
            {
                walled = value;
                Refresh();
            }
        }
    }
    public int SearchPriority
    {
        get
        {
            return distance + SearchHeuristic;
        }
    }

    public HexCell PathFrom { get; set; }
    public int SearchHeuristic { get; set; }
    public HexCell NextWithSamePriority { get; set; }

    public void RemoveOutgoingRiver()
    {
        if (!hasOutgoingRiver)
        {
            return;
        }
        hasOutgoingRiver = false;
        RefreshSelfOnly();

        HexCell neighbor = GetNeighbor(outgoingRiver);
        neighbor.hasIncomingRiver = false;
        neighbor.RefreshSelfOnly();
    }

    public void RemoveIncomingRiver()
    {
        if (!hasIncomingRiver)
        {
            return;
        }
        hasIncomingRiver = false;
        RefreshSelfOnly();

        HexCell neighbor = GetNeighbor(incomingRiver);
        neighbor.hasOutgoingRiver = false;
        neighbor.RefreshSelfOnly();
    }
    public void RemoveRiver()
    {
        RemoveOutgoingRiver();
        RemoveIncomingRiver();
    }
    public void SetOutgoingRiver(HexDirection direction)
    {
        if (hasOutgoingRiver && outgoingRiver == direction)
        {
            return;
        }

        HexCell neighbor = GetNeighbor(direction);
        if (!IsValidRiverDestination(neighbor))
        {
            return;
        }

        RemoveOutgoingRiver(); //Clear any previous outgoing river
        if (hasIncomingRiver && incomingRiver == direction)
        {
            RemoveIncomingRiver();
        }

        hasOutgoingRiver = true;
        outgoingRiver = direction;
        specialIndex = 0;

        neighbor.RemoveIncomingRiver(); //Remove and set an incoming river for the neighbor
        neighbor.hasIncomingRiver = true;
        neighbor.incomingRiver = direction.Opposite();
        neighbor.specialIndex = 0;
        SetRoad((int)direction, false);
    }

    /*
     * Validate the rivers when changing either the elevation or water level
     */
    void ValidateRivers()
    {
        if (
            hasOutgoingRiver &&
            !IsValidRiverDestination(GetNeighbor(outgoingRiver))
        )
        {
            RemoveOutgoingRiver();
        }
        if (
            hasIncomingRiver &&
            !GetNeighbor(incomingRiver).IsValidRiverDestination(this)
        )
        {
            RemoveIncomingRiver();
        }
    }

    //A Hex cell has 6 neighbors
    [SerializeField] public HexCell[] neighbors;

    public HexCell GetNeighbor(HexDirection direction)
    {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(HexDirection direction, HexCell cell)
    {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction.Opposite()] = this;
    }

    public HexEdgeType GetEdgeType(HexDirection direction)
    {
        return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
    }

    public HexEdgeType GetEdgeType(HexCell otherCell)
    {
        return HexMetrics.GetEdgeType(
            elevation, otherCell.elevation
        );
    }

    void Refresh()
    {
        if(chunk) chunk.Refresh();

        if (formationController)
        {
            formationController.ValidateLocation();
        }
        if (unitController)
        {
            unitController.ValidateLocation();
        }
        for (int i = 0; i < neighbors.Length; i++)
        {
            HexCell neighbor = neighbors[i];
            if (neighbor != null && neighbor.chunk != chunk)
            {
                neighbor.chunk.Refresh();
            }
        }
    }
    void RefreshSelfOnly()
    {
        chunk.Refresh();
        if (formationController)
        {
            formationController.ValidateLocation();
        }
    }

    //Method to check whether the cell has a road in a certain direction.
    public bool HasRoadThroughEdge(HexDirection direction)
    {
        return roads[(int)direction];
    }

    //It is also handy to know whether a cell has at least one road, so add a property for that.
    //Just loop through the array and return true as soon as you find a road. If there isn't any, return false.
    public bool HasRoads
    {
        get
        {
            for (int i = 0; i < roads.Length; i++)
            {
                if (roads[i])
                {
                    return true;
                }
            }
            return false;
        }
    }

    //Both cells are refreshed. As the roads are local to the cells, we only have to refresh the cells themselves, not also their neighbors.
    public void RemoveRoads()
    {
        for (int i = 0; i < neighbors.Length; i++)
        {
            if (roads[i])
            {
                SetRoad(i, false);
            }
        }
    }

    void SetRoad(int index, bool state)
    {
        roads[index] = state;
        neighbors[index].roads[(int)((HexDirection)index).Opposite()] = state;
        neighbors[index].RefreshSelfOnly();
        RefreshSelfOnly();
    }

    //We cannot have both a river and a road going in the same direction. So make sure that there is room for the new road, before adding it.
    public void AddRoad(HexDirection direction)
    {
        if (!roads[(int)direction] && 
            !HasRiverThroughEdge(direction) && GetElevationDifference(direction) <= 1 && !IsSpecial && !GetNeighbor(direction).IsSpecial)
        {
            SetRoad((int)direction, true);
        }
    }

    /**
     * method that tells us the elevation difference in a certain direction.
     */
    public int GetElevationDifference(HexDirection direction)
    {
        int difference = elevation - GetNeighbor(direction).elevation;
        return difference >= 0 ? difference : -difference;
    }

    bool IsValidRiverDestination(HexCell neighbor) {
        return neighbor && (
            elevation >= neighbor.elevation || waterLevel == neighbor.elevation
        );
    }

    void Start()
    {
        //color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void Save(BinaryWriter writer)
    {
        //The various levels and indices of our cells are stored as integers. However, they only cover a small value range. 
        //They certainly stay inside the 0–255 range each. 
        //This means that only the first byte of each integer will be used. The other three will always be zero.
        writer.Write((byte)terrainTypeIndex);
        writer.Write((byte)elevation);
        writer.Write((byte)waterLevel);
        writer.Write((byte)urbanLevel);
        writer.Write((byte)farmLevel);
        writer.Write((byte)plantLevel);
        writer.Write((byte)specialIndex);
        writer.Write(walled);

        if (hasIncomingRiver)
        {
            writer.Write((byte)(incomingRiver + 128));
        }
        else
        {
            writer.Write((byte)0);
        }
        if (hasOutgoingRiver)
        {
            writer.Write((byte)(outgoingRiver + 128));
        }
        else
        {
            writer.Write((byte)0);
        }

        int roadFlags = 0;
        for (int i = 0; i < roads.Length; i++) {
            if (roads[i])
            {
                roadFlags |= 1 << i;
            }
        }
        writer.Write((byte)roadFlags);
        writer.Write(IsExplored);
    }

    public void Load(BinaryReader reader, int header)
    {
        terrainTypeIndex = reader.ReadByte();
        elevation = reader.ReadByte();
        ShaderData.RefreshTerrain(this);
        RefreshPosition();

        waterLevel = reader.ReadByte();
        urbanLevel = reader.ReadByte();
        farmLevel = reader.ReadByte();
        plantLevel = reader.ReadByte();
        specialIndex = reader.ReadByte();
        walled = reader.ReadBoolean();

        byte riverData = reader.ReadByte();
        if (riverData >= 128)
        {
            hasIncomingRiver = true;
            incomingRiver = (HexDirection)(riverData - 128);
        }
        else
        {
            hasIncomingRiver = false;
        }
        riverData = reader.ReadByte();
        if (riverData >= 128)
        {
            hasOutgoingRiver = true;
            outgoingRiver = (HexDirection)(riverData - 128);
        }
        else
        {
            hasOutgoingRiver = false;
        }

        int roadFlags = reader.ReadByte();
        for (int i = 0; i < roads.Length; i++)
        {
            roads[i] = (roadFlags & (1 << i)) != 0;
        }


        IsExplored = header >= 3 ? reader.ReadBoolean() : false;
        ShaderData.RefreshVisibility(this);
    }

    void RefreshPosition()
    {
        Vector3 position = transform.localPosition;
        position.y = elevation * HexMetrics.elevationStep;
        position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) * HexMetrics.elevationPerturbStrength;
        //position.y -= (HexMetrics.SampleNoise(position).y * 2f - 1f) * 2;
        transform.localPosition = position;

        Vector3 uiPosition = uiRect.localPosition;
        //uiPosition.z = elevation * -HexMetrics.elevationStep;
        uiPosition.z = -position.y;
        uiRect.localPosition = uiPosition;

    }

    /*
    void UpdateDistanceLabel()
    {
        Text label = uiRect.GetComponent<Text>();
        label.text = distance == int.MaxValue ? "" : distance.ToString();
    }
    */
    public void SetLabel(string text)
    {
        UnityEngine.UI.Text label = uiRect.GetComponent<Text>();
        label.text = text;
    }

    public void DisableHighlight(bool shaderAlwaysOnTop)
    {
        int pickShader = 0; //DefaultAlways on Top
        if (!shaderAlwaysOnTop)
        {
            pickShader = 1;
        }

        Image highlight = uiRect.GetChild(pickShader).GetComponent<Image>();
        highlight.enabled = false;
    }

    public void EnableHighlight(Color color, bool shaderAlwaysOnTop)
    {
        int pickShader = 0; //DefaultAlways on Top
        if (!shaderAlwaysOnTop)
        {
            pickShader = 1;
        }
        Image highlight = uiRect.GetChild(pickShader).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }

    //Fog of War
    public void IncreaseVisibility()
    {
        visibility += 1;
        if (visibility == 1)
        {
            IsExplored = true; // Mark this cell as explored
            ShaderData.RefreshVisibility(this);
        }
    }

    public void DecreaseVisibility()
    {
        visibility -= 1;
        if (visibility == 0)
        {
            ShaderData.RefreshVisibility(this);
        }
    }
    public int ViewElevation
    {
        get
        {
            return elevation >= waterLevel ? elevation : waterLevel;
        }
    }

    public void ResetVisibility()
    {
        if (visibility > 0)
        {
            visibility = 0;
            ShaderData.RefreshVisibility(this);
        }
    }
}

