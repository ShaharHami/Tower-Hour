using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWideData : MonoBehaviour
{
    public static GameWideData Instance;

    [Header("Player persistent data")]
    public int towers;
    public int shotDamage;
    public int score;
    public int maxShotDamage;
    public int maxScore;
    public int maxTowersPossible = 6;
    public float fireRate;
    public float maxFireRate = 0.01f;
    public float minFireRate = 1f;
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        
    }
    public void ResetData()
    {
        Destroy(this.gameObject);
    }
}
