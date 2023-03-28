using Assets.Scripts.Models.Unit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{
    private HexGrid grid;
    private UnitManager manager;
    private UnitController controller;
    private FormationController formation;

    private HexCell currentPathFrom, currentPathTo;
    public bool currentPathExists;

    private HexCellPriorityQueue searchFrontier;
    private int searchFrontierPhase;

    public Pathfinder(HexGrid grid, UnitManager manager, UnitController controller, FormationController formation)
    {
        this.grid = grid;
        this.manager = manager;
        this.controller = controller;
        this.formation = formation;
    }

    /*
     * Only for overworld
     */
    public void FindPath(HexCell fromCell, HexCell toCell, FormationController unit)
    {
        //StopAllCoroutines();
        //StartCoroutine(Search(fromCell, toCell, speed));
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        currentPathExists = Search(fromCell, toCell, unit);
        ShowPath(unit.Speed);
    }

    /*
     * When a path is found, we have to remember it. 
     * That way, we can clean it up next time. So keep track of the end points and whether a path exists between them.
     */
    public void FindPath(HexCell fromCell, HexCell toCell, UnitController unit)
    {
        ClearPath(); //Clear any known paths
        currentPathFrom = fromCell;
        currentPathTo = toCell;

        //Returns true if there is an available path
        currentPathExists = Search(fromCell, toCell, unit);
    }

    public void ClearPath()
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                current.SetLabel(null);
                current.DisableHighlight(true);
                current = current.PathFrom;
            }
            current.DisableHighlight(true);
            currentPathExists = false;
        }
        else if (currentPathFrom)
        {
            currentPathFrom.DisableHighlight(true);
            currentPathTo.DisableHighlight(true);
        }
        currentPathFrom = currentPathTo = null;
    }

    /*
     * Return a list of hexcells for the unit/formation to traverse
     * The list is created through hex-cell links
     */
    public List<HexCell> GetPath()
    {
        if (!currentPathExists)
        {
            return null;
        }

        List<HexCell> path = ListPool<HexCell>.Get();
        //The list is filled by following the path reference from the destination back to the start
        for (HexCell c = currentPathTo; c != currentPathFrom; c = c.PathFrom)
        {
            path.Add(c);
        }

        path.Add(currentPathFrom);
        path.Reverse();
        return path;
    }

    public void ShowPath(int speed)
    {
        if (currentPathExists)
        {
            HexCell current = currentPathTo;
            while (current != currentPathFrom)
            {
                int turn = (current.Distance - 1) / speed;
                current.SetLabel(turn.ToString());
                current.EnableHighlight(Color.white, true);
                current = current.PathFrom;
            }
        }
        currentPathFrom.EnableHighlight(Color.blue, true);
        currentPathTo.EnableHighlight(Color.red, true);
    }


    public void DoPathfinding(UnitController controller, HexCell targetCell)
    {
        if (targetCell && controller.IsValidDestination(targetCell))
        {
            FindPath(controller.Location, targetCell, controller);
        }
        else
        {
            ClearPath();
        }

    }

    public void DoMove(UnitController controller)
    {
        if (currentPathExists)
        {
            controller.Travel(GetPath(), 1);
            ClearPath();
        }
    }

    /*
     * Breadth-First Search using the selected cell as the tree root
     */
    bool Search(HexCell fromCell, HexCell toCell, FormationController unit)
    {
        int speed = unit.Speed;
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

            int currentTurn = (current.Distance - 1) / speed;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase)
                {
                    continue;
                }
                if (!unit.IsValidDestination(neighbor))
                {
                    continue;
                }
                int moveCost = unit.GetMoveCost(current, neighbor, d);

                if (moveCost < 0)
                {
                    continue;
                }

                int distance = current.Distance + moveCost;
                int turn = (distance - 1) / speed;
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

    /*
     * Searches for the best route to a target cell.
     * Establishes a link between the cells along the route and returns true if the path exists
     */
    bool Search(HexCell fromCell, HexCell toCell, UnitController unit)
    {
        int speed = unit.Speed;
        searchFrontierPhase += 2;
        if (searchFrontier == null)
        {
            searchFrontier = new HexCellPriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

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
                for (int i = 0; i < grid.cells.Length; i++)
                {
                    grid.cells[i].ResetSearchPriority();
                }
                return true;
            }

            int currentTurn = (current.Distance - 1) / speed;

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase)
                {
                    continue;
                }
                if (!unit.IsValidDestination(neighbor))
                {
                    continue;
                }
                int moveCost = unit.GetMoveCost(current, neighbor, d);

                if (moveCost < 0)
                {
                    continue;
                }

                int distance = current.Distance + moveCost;
                int turn = (distance - 1) / speed;
                if (turn > currentTurn)
                {
                    distance = turn * speed + moveCost;
                }

                
                //add a neighbor to the frontier. When a cell is added, it is not guaranteed to be the shortest distance, we need to check
                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    Debug.Log("search phase < search frontier phase" + controller.data.GetName());
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    //neighbor.SetLabel(turn.ToString());
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic = neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    Debug.Log("distance < neighbor distance" + controller.data.GetName());
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
        for (int i = 0; i < grid.cells.Length; i++)
        {
            grid.cells[i].ResetSearchPriority();
        }
        return false;
    }


    List<HexCell> GetVisibleCells(HexCell fromCell, int range)
    {
        List<HexCell> visibleCells = ListPool<HexCell>.Get();

        searchFrontierPhase += 2;
        if (searchFrontier == null)
        {
            searchFrontier = new HexCellPriorityQueue();
        }
        else
        {
            searchFrontier.Clear();
        }

        range += fromCell.ViewElevation;
        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);

        HexCoordinates fromCoordinates = fromCell.coordinates;
        while (searchFrontier.Count > 0)
        {
            HexCell current = searchFrontier.Dequeue();
            current.SearchPhase += 1;
            visibleCells.Add(current);

            for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
            {
                HexCell neighbor = current.GetNeighbor(d);
                if (neighbor == null || neighbor.SearchPhase > searchFrontierPhase || !neighbor.Explorable)
                {
                    continue;
                }

                int distance = current.Distance + 1;
                if (distance + neighbor.ViewElevation > range || distance > fromCoordinates.DistanceTo(neighbor.coordinates))
                {
                    continue;
                }

                if (neighbor.SearchPhase < searchFrontierPhase)
                {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.SearchHeuristic = 0;
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance)
                {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }
        return visibleCells;
    }

    public void IncreaseVisibility(HexCell fromCell, int range)
    {
        List<HexCell> cells = GetVisibleCells(fromCell, range);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].IncreaseVisibility();
        }
        ListPool<HexCell>.Add(cells);
    }

    public void DecreaseVisibility(HexCell fromCell, int range)
    {
        List<HexCell> cells = GetVisibleCells(fromCell, range);
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].DecreaseVisibility();
        }
        ListPool<HexCell>.Add(cells);
    }


    /*
     * Locate the nearest enemy that can be pathed to, create the path
     * Return the distance to that path
     */
    public int FindPathToNearestEnemy()
    {
        int shortestDistance = 99999;
        HexCell targetCell = null;
        List<HexCell> alreadyChecked = new List<HexCell>(); //Cells that were already checked

        Debug.Log("Find Nearest Enemy Reached BY.." + controller.data.GetName());

        //Let's go through our list of controllers
        for (int i = 0; i < manager.controllers.Count; i++)
        {
            //Need to check enemy faction here todo
            UnitController enemy = manager.controllers[i];

            if (controller == enemy) continue;  //If they're the same unit
            if (controller.data.faction.Equals(enemy.data.faction)) continue; //if they have the same faction

            //Go through the hexcell's neighbors
            for (int j = 0; j < enemy.Location.neighbors.Length; j++)
            {
                HexCell enemyCell = enemy.Location.neighbors[j];
                if (enemyCell)
                {
                    if (enemyCell.unitController) continue;
                    if (alreadyChecked.Contains(enemyCell)) continue;

                    alreadyChecked.Add(enemyCell);

                    //Find this unit's route to that selected enemy controller
                    FindPath(controller.Location, enemyCell, controller);

                    if (currentPathExists && enemyCell.Distance < shortestDistance)
                    {
                        shortestDistance = enemyCell.Distance;
                        targetCell = enemyCell;

                        //There is an enemy right next to our unit! We don't have to move
                        if (shortestDistance <= 0) break;
                    }
                }
              
            }     
        }

        //If we got a valid target
        if (targetCell && shortestDistance > 0)
        {
            FindPath(controller.Location, targetCell, controller);
            return shortestDistance;
        }
        else
        {
            return -1;
        }
    }

    /*
     * Check if the adjacent squares have an enemy, if so, return true
     */
    public bool IsThereAdjacentEnemy()
    {
        for (int i=0; i < controller.Location.neighbors.Length; i++){
            HexCell enemyCell = controller.Location.neighbors[i];

            if (enemyCell && enemyCell.unitController)
            {
                UnitController enemyController = enemyCell.unitController;
                if (!controller.data.faction.Equals(enemyController.data.faction))
                {
                    
                    return true;
                }
            }
            
        }

        return false;
    }


}