using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    private Bounds levelBounds;
    public Bounds LevelBounds
    {
        get { return levelBounds; }
    }
    [SerializeField] Waypoint gridBlock;
    [SerializeField] GameObject[] neutralBlocks;
    [SerializeField] GameObject homeBase;
    [SerializeField] GameObject enemyBase;
    int gridHeight;
    int gridWidth;
    float neutralCubeMinHeight;
    float neutralCubeMaxHeight;
    public Random random;
    public RangeAttribute range;
    public RangeInt rangeInt;
    private float neutralProbability;
    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.right,
        // Vector2Int.down,
        Vector2Int.left
    };
    private List<Vector3> waypointsList = new List<Vector3>();
    private Dictionary<Vector3, GameObject> gameGrid = new Dictionary<Vector3, GameObject>();
    public Dictionary<Vector3, GameObject> GameGrid
    {
        get { return gameGrid; }
    }
    Dictionary<Vector3, GameObject> neutralGrid = new Dictionary<Vector3, GameObject>();
    Stack<GameObject> blockStack = new Stack<GameObject>();
    private int counter = 0;
    private bool hasEnded = false;
    private Vector3 endCoordinates;
    private Vector3 startCoordinates;
    public Vector3 StartCoordinates
    {
        get { return startCoordinates; }
    }
    private GameManager gameManager;
    public void MakeMap()
    {
        DefineVariables();
        DefineBounds();
        PlaceFirstCube();
        BuildPath(waypointsList[0]);
        IterateOverGrid();
        AddBases();
    }
    private void DefineVariables()
    {
        gameManager = FindObjectOfType<GameManager>();
        gridHeight = gameManager.gridHeight;
        gridWidth = gameManager.gridWidth;
        neutralCubeMinHeight = gameManager.neutralBlocksYRange.neutralCubesMinYrange;
        neutralCubeMaxHeight = gameManager.neutralBlocksYRange.neutralCubesMaxYrange;
        neutralProbability = gameManager.neutralBlocksYRange.neutralCubeBuildProbability;
    }
    private void PlaceFirstCube()
    {
        startCoordinates = new Vector3(-gridWidth * 5f, 0, -gridHeight * 5f);
        PlaceCube(gridBlock.gameObject, startCoordinates, true, transform);
    }
    private void PlaceCube(GameObject cube, Vector3 cubePos, bool isWaypoint, Transform desiredParent)
    {
        CubeEditor cubeEditor = cube.GetComponent<CubeEditor>();
        if (cubeEditor != null)
        {
            cubeEditor.enabled = false;
        }
        GameObject newCube = Instantiate(cube, cubePos, Quaternion.identity);
        if (!desiredParent)
        {
            newCube.transform.SetParent(transform);
        }
        else
        {
            newCube.transform.SetParent(desiredParent);
        }
        if (isWaypoint)
        {
            ListPlacedCubes(cubePos, newCube);
        }
        else
        {
            ListNeutralCubes(cubePos, newCube);
        }
    }
    private void ListPlacedCubes(Vector3 cubePos, GameObject cube)
    {
        waypointsList.Add(cubePos);
        gameGrid.Add(cubePos, cube);
        blockStack.Push(cube);
    }
    private void ListNeutralCubes(Vector3 cubePos, GameObject cube)
    {
        neutralGrid.Add(new Vector3(cubePos.x, 0, cubePos.z), cube);
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
    private void BuildPath(Vector3 currentPosition)
    {
        int occupiedDirection = 0;
        Vector2Int[] shuffledDirections = Shuffled(directions);
        foreach (Vector2Int direction in shuffledDirections)
        {
            Vector2Int realDirection = direction * 10;
            Vector3 newPosition = new Vector3(currentPosition.x + realDirection.x, currentPosition.y, currentPosition.z + realDirection.y);
            bool isInBounds = CheckBounds(newPosition / 10);
            bool isOccupied = CheckClear(newPosition);
            if (!hasEnded && counter <= (gridHeight * gridWidth * 100))
            {
                if (isInBounds && !isOccupied)
                {
                    if (neutralGrid.ContainsKey(newPosition))
                    {
                        Destroy(neutralGrid[newPosition]);
                    }
                    if (gameGrid.ContainsKey(newPosition))
                    {
                        Destroy(gameGrid[newPosition]);
                    }
                    PlaceCube(gridBlock.gameObject, newPosition, true, transform);
                    counter++;
                    BuildPath(newPosition);
                    break;
                }
                else
                {
                    occupiedDirection++;
                    if (occupiedDirection >= 4)
                    {
                        BuildPath(blockStack.Pop().transform.position);
                    }
                }
                bool topBoundReached = CheckTopBound(newPosition);
                if (topBoundReached)
                {
                    if (!gameGrid.ContainsKey(newPosition))
                    {
                        PlaceCube(gridBlock.gameObject, newPosition, true, transform);
                    }
                    hasEnded = true;
                    SetStartAndEndPoints(newPosition);
                    break;
                }
            }
        }
    }
    private void SetStartAndEndPoints(Vector3 position)
    {
        gameGrid[waypointsList[0]].GetComponent<Waypoint>().isStartPoint = true;
        for (int i = 0; i < gameGrid.Count; i++)
        {
            gameGrid[waypointsList[i]].GetComponent<Waypoint>().isEndPoint = false;
            if (gameGrid[waypointsList[i]] == gameGrid[position])
            {
                gameGrid[waypointsList[i]].GetComponent<Waypoint>().isEndPoint = true;
                endCoordinates = gameGrid[waypointsList[i]].transform.position;
            }
        }
    }
    private void DefineBounds()
    {
        levelBounds = new Bounds(new Vector3(0, 0, 0), new Vector3(gridWidth, 30, gridHeight));
    }
    private void IterateOverGrid()
    {
        for (float i = -levelBounds.size.x; i <= levelBounds.size.x; i++)
        {
            for (float j = -levelBounds.size.z; j <= levelBounds.size.z; j++)
            {
                Vector3 thisPos = new Vector3(i * 10.0f, 0, j * 10.0f);
                bool isWithinBounds = CheckBounds(thisPos / 10);
                bool isOccupied = CheckClear(thisPos);
                if (!isOccupied && isWithinBounds)
                {
                    int randBlock = Random.Range(0, neutralBlocks.Length);
                    GameObject blockToBuild = neutralBlocks[randBlock];
                    bool build = (Random.value > neutralProbability);
                    if (build)
                    {

                        PlaceCube(blockToBuild, new Vector3(thisPos.x, Random.Range(neutralCubeMinHeight, neutralCubeMaxHeight), thisPos.z), false, GameObject.FindGameObjectWithTag("NeutralBlocks").transform);
                    }
                }
            }
        }
    }
    private void AddBases()
    {
        Instantiate(homeBase, new Vector3(
            endCoordinates.x,
            endCoordinates.y,
            endCoordinates.z + 10.0f
        ), Quaternion.identity);
        Instantiate(enemyBase, new Vector3(
            startCoordinates.x,
            startCoordinates.y,
            startCoordinates.z - 15.0f
        ), Quaternion.identity);
    }
    private bool CheckBounds(Vector3 point)
    {
        return levelBounds.Contains(point);
    }
    private bool CheckClear(Vector3 point)
    {
        return gameGrid.ContainsKey(point);
    }
    private bool CheckTopBound(Vector3 point)
    {
        return point.z >= gridHeight * 5;
    }
}
