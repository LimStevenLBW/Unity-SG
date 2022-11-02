using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    public RectTransform uiRect; //Track Label Location
    public HexGridChunk chunk;

    [SerializeField]
    private bool[] roads;

    private bool hasIncomingRiver, hasOutgoingRiver;
    private HexDirection incomingRiver, outgoingRiver;
    private Color color;
    private Color defaultColor = Color.white;

    private int elevation = int.MinValue; //Lowest value an integer can have, just to avoid skipping first computation

    public int Elevation
    {
        get { return elevation; }
        set {
            if (elevation == value) return; // Skip computation if no change

            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            position.y += (HexMetrics.SampleNoise(position).y * 2f - 1f) * HexMetrics.elevationPerturbStrength;

            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            //uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;

            if (
                hasOutgoingRiver &&
                elevation < GetNeighbor(outgoingRiver).elevation
            )
            {
                RemoveOutgoingRiver();
            }
            if (
                hasIncomingRiver &&
                elevation > GetNeighbor(incomingRiver).elevation
            )
            {
                RemoveIncomingRiver();
            }

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

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            if (color == value) return;

            color = value;
            Refresh();
        }
    }
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
            waterLevel = value;
            Refresh();
        }
    }

    int waterLevel;
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
        if (!neighbor || elevation < neighbor.elevation) //Abort if neighbor has higher elevation(cannot flow rivers uphill)
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

        neighbor.RemoveIncomingRiver(); //Remove and set an incoming river for the neighbor
        neighbor.hasIncomingRiver = true;
        neighbor.incomingRiver = direction.Opposite();
        SetRoad((int)direction, false);
    }

    //A Hex cell has 6 neighbors
    [SerializeField] HexCell[] neighbors;

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
    public Vector3 Position
    {
        get
        {
            return transform.localPosition;
        }
    }
    void Refresh()
    {
        if(chunk) chunk.Refresh();
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
        if (!roads[(int)direction] && !HasRiverThroughEdge(direction) && GetElevationDifference(direction) <= 1)
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

    void Start()
    {
        color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
         
    }


}
