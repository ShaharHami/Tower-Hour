using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    float movementSpeed = 1f;
    [SerializeField] float offsetY = 10.0f;
    private BaseHealth baseObject;
    private GameManager gameManager;
    private bool isEnabled = true;
    public bool IsEnabled
    {
        set { isEnabled = value; }
    }
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        movementSpeed = gameManager.enemySpeed;
        baseObject = FindObjectOfType<BaseHealth>();
        PathFinder pathFinder = FindObjectOfType<PathFinder>();
        var path = pathFinder.GetPath();
        StartCoroutine(FollowPath(path));
    }
    IEnumerator FollowPath(List<Waypoint> path)
    {
        Waypoint oldWaypoint = path[0];
        Enemy enemy = gameObject.GetComponent<Enemy>();
        foreach (Waypoint waypoint in path)
        {
            while (FindObjectOfType<GameManager>().IsPaused)
            {
                yield return null;
            }
            Vector3 rotationDirection = (waypoint.transform.position - oldWaypoint.transform.position) / 10;
            if (isEnabled)
            {
                // GameWideData.Instance.GetComponent<AudioManager>().Play("Enemy Move");
                transform.GetComponent<AudioSource>().Play();
                transform.position = new Vector3(
                    waypoint.transform.position.x,
                    waypoint.transform.position.y + offsetY,
                    waypoint.transform.position.z
                );
                if (rotationDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(rotationDirection, Vector3.up);
                }
                yield return new WaitForSeconds(movementSpeed);
            }
            oldWaypoint = waypoint;
        }
        if (isEnabled)
        {
            enemy.DetonateEnemy();
            baseObject.Damage(enemy.damage);
        }
    }
}
