using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public bool isExplored = false;
    public Waypoint exploredFrom;
    Vector2Int gridPos;
    const int gridCellSize = 10;
    public bool isPlacable = true;
    public bool isEndPoint = false;
    public bool isStartPoint = false;
    private Material mat;
    private Color oldColor;
    private bool isDragging;
    [SerializeField] Color placableColor;
    [SerializeField] Color nonPlacableColor;
    [SerializeField] Transform innerPrefab;
    private GameManager gameManager;
    private void Start()
    {
        mat = innerPrefab.GetComponent<Renderer>().material;
        oldColor = mat.color;
        gameManager = FindObjectOfType<GameManager>();
    }
    public int GetGridCellSize()
    {
        return gridCellSize;
    }
    public Vector2Int GetWaypointPos()
    {
        return new Vector2Int(
        GetSnapPos(transform.position.x),
        GetSnapPos(transform.position.z)
        );
    }
    int GetSnapPos(float axis)
    {
        return Mathf.RoundToInt(axis / gridCellSize);
    }
    void OnMouseOver()
    {
        if (!isDragging && Input.GetMouseButtonDown(0) && !gameManager.GameOver && !gameManager.IsPaused)
        {
            if (isPlacable)
            {
                FindObjectOfType<TowerPlacement>().PlaceTower(this);
                isPlacable = false;
            }
        }
    }
    void OnMouseEnter()
    {
        if (!gameManager.GameOver && !gameManager.IsPaused)
        {
            if (isPlacable)
            {
                Highlight(true, placableColor);
            }
            else
            {
                Highlight(true, nonPlacableColor);
            }
        }
    }
    private void OnMouseExit()
    {
        if (!gameManager.GameOver && !gameManager.IsPaused)
        {
            if (isPlacable)
            {
                Highlight(false, placableColor);
            }
            else
            {
                Highlight(false, nonPlacableColor);
            }
        }
    }
    private void Highlight(bool on, Color color)
    {
        if (on)
        {
            mat.color = color;
        }
        else
        {
            mat.color = oldColor;
        }
    }
}
