using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>();
    bool isRunning = true;
    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };
    [SerializeField] Waypoint startWaypoint, endWaypoint;
    Waypoint searchCenter;
    private List<Waypoint> path = new List<Waypoint>();
    public List<Waypoint> GetPath()
    {
        if (path.Count <= 0)
        {
            LoadBlocks();
            BreadthFirstSearch();
            CreatePath();
        }
        return path;
    }
    Vector2Int[] Shuffled(Vector2Int[] list)
    {
        Vector2Int[] newList = (Vector2Int[])list.Clone();
        for (int i = 0; i < newList.Length; i++)
        {
            Vector2Int temp = newList[i];
            int randomIndex = Random.Range(i, newList.Length);
            newList[i] = newList[randomIndex];
            newList[randomIndex] = temp;
        }
        return newList;
    }
    private void CreatePath()
    {
        SetAsPath(endWaypoint);
        Waypoint previous = endWaypoint.exploredFrom;
        while (previous != startWaypoint)
        {
            SetAsPath(previous);
            previous = previous.exploredFrom;
        }
        SetAsPath(startWaypoint);
        path.Reverse();
    }
    private void SetAsPath(Waypoint waypoint)
    {
        path.Add(waypoint);
        waypoint.isPlacable = false;
    }
    private void BreadthFirstSearch()
    {
        queue.Enqueue(startWaypoint);
        while (queue.Count > 0 && isRunning)
        {
            searchCenter = queue.Dequeue();
            OnEndFound();
            ExploreNeighbours();
            searchCenter.isExplored = true;
        }
    }
    private void OnEndFound()
    {
        if (searchCenter == endWaypoint)
        {
            isRunning = false;
        }
    }
    private void LoadBlocks()
    {
        var waypoints = gameObject.GetComponent<LevelCreator>().GameGrid.Values;
        foreach (GameObject waypointGO in waypoints)
        {
            Waypoint waypoint = waypointGO.GetComponent<Waypoint>();
            var gridPos = waypoint.GetWaypointPos();
            if (!grid.ContainsKey(gridPos))
            {
                grid.Add(gridPos, waypoint);
            }
            if (waypoint.isEndPoint == true)
            {
                endWaypoint = waypoint;
            }
            if (waypoint.isStartPoint)
            {
                startWaypoint = waypoint;
            }
        }
    }
    private void ExploreNeighbours()
    {
        if (!isRunning) { return; }
        directions = Shuffled(directions);
        foreach (Vector2Int direction in directions)
        {
            Vector2Int exploreCoordinates = searchCenter.GetWaypointPos() + direction;
            if (grid.ContainsKey(exploreCoordinates))
            {
                QueueNewNeighbour(exploreCoordinates);
            }
        }
    }
    private void QueueNewNeighbour(Vector2Int exploreCoordinates)
    {
        Waypoint neighbour = grid[exploreCoordinates];
        if (!neighbour.isExplored && !queue.Contains(neighbour))
        {
            queue.Enqueue(neighbour);
            neighbour.exploredFrom = searchCenter;
        }
    }

}