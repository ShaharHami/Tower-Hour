using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CycleCameras : MonoBehaviour
{
    [SerializeField] GameObject[] vcams;
    private bool following = false;
    private Transform closestEnemy;
    public Transform ClosestEnemy
    {
        set { closestEnemy = value; }
        get { return closestEnemy; }
    }
    private int currentCamera = 0;
    private Vcam activeCam;
    public Vcam ActiveCam
    {
        get { return activeCam; }
    }
    private GameManager gameManager;
    public float spawnDelay;
    private CameraMovement cameraMovement;
    private DialogueTrigger dialogueTrigger;
    void Awake()
    {
        cameraMovement = FindObjectOfType<CameraMovement>();
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
    }
    private void Start()
    {
        vcams[currentCamera].SetActive(true);
        activeCam = new Vcam();
        activeCam.cam = vcams[currentCamera];
        gameManager = FindObjectOfType<GameManager>();
        // spawnDelay = GameObject.FindObjectOfType<EnemySpawner>().SpawnDelay;
    }
    void Update()
    {
        Cycle();
        OnEnemyDestroyed();
    }
    private void OnEnemyDestroyed()
    {
        if (closestEnemy == null && following && !gameManager.GameOver && !gameManager.LevelWon)
        {
            Invoke("DelayedDestroy", spawnDelay);
        }
    }

    private void DelayedDestroy()
    {
        FindClosestEnemy();
        GameObject followCam = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
        SetFollowTarget(followCam);
    }

    private void Cycle()
    {
        if (Input.GetKeyDown(KeyCode.C) && !gameManager.GameOver)
        {
            GetComponent<AudioSource>().Play();
            CamNum();
            SelectCamera(currentCamera);
        }
    }
    private void CamNum()
    {
        if (currentCamera >= vcams.Length - 1)
        {
            currentCamera = 0;
        }
        else
        {
            currentCamera++;
        }
    }
    private void PositionBaseCam(GameObject vcam)
    {
        float rigY = vcam.transform.parent.position.y;
        ResetPos(vcam.transform.parent, rigY);
        Transform lookAtTransform = GameObject.FindGameObjectWithTag("ZeroPoint").transform;
        vcam.transform.position = new Vector3(
            vcam.transform.position.x,
            vcam.transform.position.y,
            gameManager.gridHeight * -3
        );
        vcam.transform.LookAt(lookAtTransform);
    }
    private void SelectCamera(int camNum)
    {
        if (!gameManager.GameOver && !gameManager.IsPaused)
        {
            FindClosestEnemy();
        }
        Vcam vcam = new Vcam();
        for (int i = 0; i < vcams.Length; i++)
        {
            if (camNum == i)
            {
                switch (vcams[i].tag)
                {
                    case "EnemyCam":
                        following = true;
                        vcam.inverted = false;
                        vcam.rotating = false;
                        vcam.following = true;
                        SetFollowTarget(vcams[i]);
                        dialogueTrigger.RemoveFromQueue("Top Cam");
                        dialogueTrigger.RemoveFromQueue("Base Cam");
                        dialogueTrigger.SetMessageDirectly("Enemy Cam");
                        dialogueTrigger.TriggerDialogue("Enemy Cam");
                        // AudioListener.pause = false;
                        break;

                    case "BaseCam":
                        following = false;
                        vcam.inverted = true;
                        vcam.rotating = true;
                        vcam.following = false;
                        PositionBaseCam(vcams[i]);
                        dialogueTrigger.RemoveFromQueue("Top Cam");
                        dialogueTrigger.RemoveFromQueue("Enemy Cam");
                        dialogueTrigger.SetMessageDirectly("Base Cam");
                        dialogueTrigger.TriggerDialogue("Base Cam");
                        // AudioListener.pause = true;
                        // SetFollowTarget(vcams[i]);
                        break;

                    default:
                        following = false;
                        vcam.inverted = false;
                        vcam.rotating = false;
                        vcam.following = false;
                        dialogueTrigger.RemoveFromQueue("Enemy Cam");
                        dialogueTrigger.RemoveFromQueue("Base Cam");
                        dialogueTrigger.SetMessageDirectly("Top Cam");
                        dialogueTrigger.TriggerDialogue("Top Cam");
                        // AudioListener.pause = true;
                        break;
                }
                vcams[i].SetActive(true);
                vcam.cam = vcams[i];
                activeCam = vcam;
            }
            else
            {
                vcams[i].SetActive(false);
            }
        }
    }

    private void SetFollowTarget(GameObject gameObject)
    {
        if (closestEnemy == null)
        {
            Invoke("OnEnemyDestroyed", spawnDelay);
        }
        else
        {
            CinemachineVirtualCamera followCam = gameObject.GetComponent<CinemachineVirtualCamera>();
            Transform vcamContainer = followCam.transform.parent;
            ResetPos(vcamContainer, vcamContainer.transform.position.y);
            Transform followTarget;
            Transform lookAtTarget;
            if (following)
            {
                followTarget = closestEnemy.GetComponent<Enemy>().FollowPoint;
                lookAtTarget = GameObject.FindGameObjectWithTag("FriendlyBase").transform;
            }
            else
            {
                followTarget = null;
                lookAtTarget = null;
            }
            followCam.m_Follow = followTarget;
            followCam.m_LookAt = lookAtTarget;
        }
    }
    private void ResetPos(Transform t, float y)
    {
        t.position = new Vector3(0.0f, y, 0.0f);
    }
    private void FindClosestEnemy()
    {
        Transform enemyBase = GameObject.FindGameObjectWithTag("EnemyBase").transform;
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length > 0)
        {
            Transform closest = enemies[0].transform;
            float oldDistance = 0;
            foreach (Enemy enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, enemyBase.position);
                if (distance < oldDistance)
                {
                    closest = enemy.transform;
                    oldDistance = distance;
                }
            }
            closestEnemy = closest;
        }
        else
        {
            closestEnemy = null;
        }
    }
    public class Vcam
    {
        public GameObject cam;
        public bool rotating;
        public bool canMove;
        public bool inverted;
        public bool following;
    }
}
