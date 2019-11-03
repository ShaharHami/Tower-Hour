using UnityEngine;
using Cinemachine;

public class CameraMovement : MonoBehaviour
{
    private CycleCameras cycleCameras;
    private GameManager gameManager;
    // good code
    [SerializeField] private float PanSpeed = 300f;
    [SerializeField] private float ZoomSpeedMouse = 6f;
    [SerializeField] private float[] BoundsX = new float[] { -10f, 10f };
    [SerializeField] private float[] BoundsZ = new float[] { -10f, 10f };
    [SerializeField] private float[] ZoomBounds = new float[] { 50f, 200f };
    private float[] FollowCamZoomBounds;
    private Camera cam;
    private Vector3 lastPanPosition;
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        cycleCameras = FindObjectOfType<CycleCameras>();
    }
    void FixedUpdate()
    {
        if (!gameManager.IsPaused && !gameManager.GameOver)
        {
            MoveGameCamera(cycleCameras.ActiveCam);
        }
    }
    private void MoveGameCamera(CycleCameras.Vcam vcam)
    {
        cam = Camera.main;
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0) && !vcam.following)
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse, vcam);
    }
    void PanCamera(Vector3 newPanPosition)
    {
        float gridWidth = GameWideData.Instance.GetComponent<LevelManager>().ReturnCurrentLevel().gridWidth * 5;
        float gridHeight = GameWideData.Instance.GetComponent<LevelManager>().ReturnCurrentLevel().gridHeight * 5;
        // Determine how much to move the camera
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, 0, offset.y * PanSpeed);

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0] - gridWidth, BoundsX[1] + gridWidth);
        pos.z = Mathf.Clamp(transform.position.z, BoundsZ[0] - gridHeight, BoundsZ[1] + gridHeight);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed, CycleCameras.Vcam vcam)
    {
        var camera = vcam.cam.GetComponent<CinemachineVirtualCamera>();
        FollowCamZoomBounds = new float[] { ZoomBounds[0] * 2, ZoomBounds[1] * 2 };
        float[] bounds;
        camera.m_Lens.Orthographic = false;
        if (offset == 0)
        {
            return;
        }
        if (vcam.following)
        {
            bounds = FollowCamZoomBounds;
        }
        else
        {
            bounds = ZoomBounds;
        }
        if (camera.m_Lens.Orthographic)
        {
            camera.m_Lens.OrthographicSize = Mathf.Clamp(camera.m_Lens.OrthographicSize - (offset * speed), bounds[0], bounds[1]);
        }
        else
        {
            camera.m_Lens.FieldOfView = Mathf.Clamp(camera.m_Lens.FieldOfView - (offset * speed), bounds[0] / 4, bounds[1] / 4);
        }

    }
}
