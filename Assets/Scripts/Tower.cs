using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] Transform objectToMove;
    [SerializeField] ParticleSystem shots;
    [SerializeField] ParticleSystem detonateFX;
    [SerializeField] float destroyDelay;
    [SerializeField] float range = 20f;
    [SerializeField] float offsetY = 2f;
    private float fireRate = 2f;
    private int shotDamage = 10;
    Transform targetEnemy;
    GameManager gameManager;
    public int ShotDamage
    {
        set { shotDamage = value; }
    }
    public float FireRate
    {
        set { fireRate = value; }
    }
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void Start()
    {
        if (GameWideData.Instance != null)
        {
            shotDamage = GameWideData.Instance.shotDamage;
            fireRate = GameWideData.Instance.fireRate;
        }
        if (shots != null)
        {
            ShootingLogic();
        }
    }
    void Update()
    {
        if (FindObjectOfType<GameManager>() == null || !FindObjectOfType<GameManager>().IsPaused)
        {
            GetTarget();
            if (targetEnemy && !targetEnemy.GetComponent<Enemy>().isDead)
            {
                Vector3 relativePos = new Vector3(targetEnemy.position.x, targetEnemy.position.y + offsetY, targetEnemy.position.z) - objectToMove.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                objectToMove.rotation = Quaternion.Lerp(objectToMove.rotation, toRotation, 1 * (Time.deltaTime * 10));
            }
            else
            {
                if (shots != null)
                {
                    shots.Stop();
                }
            }
        }
        else
        {
            if (shots != null)
            {
                shots.Pause();
            }
        }

    }
    void GetTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0) { return; }
        Enemy winner = enemies[0];
        float closestDistance = Vector3.Distance(transform.position, winner.transform.position);
        foreach (Enemy enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                winner = enemy;
                closestDistance = distance;
            }
        }
        targetEnemy = winner.transform;
    }
    private bool shooting = false;
    private void ShootingLogic()
    {
        var em = shots.emission;
        em.enabled = true;
        StartCoroutine(Shoot());
    }
    IEnumerator Shoot()
    {
        while (true)
        {
            GetTarget();
            if (targetEnemy && Vector3.Distance(transform.position, targetEnemy.position) <= range)
            {
                if (gameManager && !gameManager.IsPaused)
                {
                    shots.GetComponent<AudioSource>().Play();
                    shots.Emit(1);
                }
            }
            yield return new WaitForSeconds(fireRate);
        }
    }
    public void DetonateTower()
    {
        detonateFX.Play();
        DisableTower();
    }
    private void DisableTower()
    {
        ToggleVisibility();
        Invoke("DestroyTower", destroyDelay);
    }
    private void ToggleVisibility()
    {
        shots.gameObject.SetActive(false);
        foreach (MeshRenderer meshRenderer in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }
    private void DestroyTower()
    {
        Destroy(gameObject);
    }
}
