using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
[RequireComponent(typeof(Waypoint))]
public class CubeEditor : MonoBehaviour
{
    [SerializeField] string waypointLabel = "Waypoint";
    public bool drawLabels = false;
    Waypoint waypoint;
    TextMesh textMesh;
    void Awake()
    {
        waypoint = GetComponent<Waypoint>();
        textMesh = GetComponentInChildren<TextMesh>();
        textMesh.gameObject.SetActive(drawLabels);
    }
    void Update()
    {
        // Snap
        SnapToGrid();
        // Label
        if (drawLabels)
        {
            textMesh.gameObject.SetActive(drawLabels);
            LabelWaypoint();
        }
    }
    private void SnapToGrid()
    {
        int gridCellSize = waypoint.GetGridCellSize();
        transform.position = new Vector3(
        waypoint.GetWaypointPos().x * gridCellSize,
        0f,
        waypoint.GetWaypointPos().y * gridCellSize);
    }

    private void LabelWaypoint()
    {
        string labelText = waypoint.GetWaypointPos().x + "," + waypoint.GetWaypointPos().y;
        textMesh.text = labelText;
        gameObject.name = waypointLabel + " " + labelText;
    }
}
