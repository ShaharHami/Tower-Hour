using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    float spawnDelay = 5f;
    [SerializeField] EnemyMovement enemyToSpawn;
    float startHealth = 100;
    private Vector3 spawnPosition;
    private Coroutine enemySpawningCoroutine;
    private GameManager gameManager;
    private EnemyBase enemyBase;
    public float SpawnDelay
    {
        get { return spawnDelay; }
        set { spawnDelay = value; }
    }
    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        spawnPosition = GameObject.FindObjectOfType<LevelCreator>().StartCoordinates;
        spawnDelay = gameManager.enemySpawnRate;
    }
    public void StartSpawning()
    {
        enemySpawningCoroutine = StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {
        while (!gameManager.GameOver)
        {
            float timer = 0f;
            while (timer < 1f)
            {
                while (gameManager.IsPaused)
                {
                    yield return null;
                }

                timer += Time.deltaTime;
                yield return null;
            }
            enemyBase = FindObjectOfType<EnemyBase>();
            if (enemyBase != null)
            {
                Transform enemy = Instantiate(enemyToSpawn, enemyBase.transform.position, Quaternion.identity).transform;
                enemy.SetParent(transform);
                enemy.GetComponent<Enemy>().StartHealth = startHealth;
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}