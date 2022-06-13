using UnityEngine;
using UnityEngine.UI;

/*
 * sets things up in Awake and triangulates in Start.
 * It need a reference to its canvas and mesh, and an array for its cells. 
 * However, it will not create these cells. We'll still let the grid do that.
 */
public class HexGridChunk : MonoBehaviour
{
    HexCell[] cells;

    HexMesh hexMesh;
    Canvas gridCanvas;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
    }

    void Start()
    {
        hexMesh.Triangulate(cells);
    }

    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }

    public void Refresh()
    {
        enabled = true;
    }

    //Each frame, the Update methods of enabled components are invoked at some point, in arbitrary order. After that's finished, the same happens with LateUpdate methods.
    void LateUpdate()
    {
        hexMesh.Triangulate(cells);
        enabled = false;
    }
}

