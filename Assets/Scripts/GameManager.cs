using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText, levelText;
    private int score;
    private string levelMessage, levelName;
    [HideInInspector] public int gridHeight;
    [HideInInspector] public int gridWidth;
    [System.Serializable]
    public class NeutralBlocksYRange : System.Object
    {
        public float neutralCubesMinYrange = 0;
        public float neutralCubesMaxYrange = 0;
        public float neutralCubeBuildProbability = 0;
    }
    public NeutralBlocksYRange neutralBlocksYRange;
    private int passingScore;
    [HideInInspector] public int enemyDamage;
    [HideInInspector] public int enemyHealth;
    [HideInInspector] public int enemyScore;
    [HideInInspector] public float enemySpeed;
    [HideInInspector] public float enemySpawnRate;
    public int Score
    {
        set { score = value; }
        get { return score; }
    }
    private bool isPaused = false;
    public bool IsPaused
    {
        set { isPaused = value; }
        get { return isPaused; }
    }
    private bool gameOver = false;
    public bool GameOver
    {
        set { gameOver = value; }
        get { return gameOver; }
    }
    private bool levelWon = false;
    public bool LevelWon
    {
        set { levelWon = value; }
        get { return levelWon; }
    }
    private int baseMaxHealth;
    public int BaseMaxHealth
    {
        set { baseMaxHealth = value; }
        get { return baseMaxHealth; }
    }
    [SerializeField] UpgradeMenu upgradeMenu;
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private SceneTransition sceneTransition;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private PlayerMessages playerMessages;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private AudioClip winSound, loseSound, winGameSound;
    private AudioSource source;
    private int currentLevel;
    private LevelManager levelManager;
    [SerializeField] private bool cheat;
    public bool Cheat
    {
        get { return cheat; }
    }
    private void Start()
    {
        levelManager = GameWideData.Instance.GetComponent<LevelManager>();
        SetLevelPrefs();
        source = GetComponent<AudioSource>();
        score = GameWideData.Instance.score;
        passingScore += score;
        pausePanel.SetActive(isPaused);
        UpdateScore(0);
        levelText.text = levelName;
        GetComponent<EnvironmentRandomizer>().Randomize();
        levelCreator.MakeMap();
        sceneTransition.FadeIn();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10) && cheat)
        {
            UpdateScore(passingScore - score);
        }
    }
    private void SetLevelPrefs()
    {
        LevelPrefs levelprefs = levelManager.ReturnCurrentLevel();
        if (levelprefs != null)
        {
            levelName = levelprefs.name;
            enemySpawnRate = levelprefs.enemySpawnDelay;
            gridHeight = levelprefs.gridHeight;
            gridWidth = levelprefs.gridWidth;
            enemyHealth = levelprefs.enemyHealth;
            enemyDamage = levelprefs.enemyDamage;
            enemySpeed = levelprefs.enemySpeed;
            enemyScore = levelprefs.enemyScore;
            passingScore = levelprefs.goal;
            baseMaxHealth = levelprefs.health;
            levelMessage = levelprefs.levelMessage;
        }

    }
    public void StartLevel()
    {
        dialogueTrigger.TriggerDialogue(levelMessage);
        dialogueTrigger.TriggerDialogue("Top Cam");
        GameWideData.Instance.GetComponent<MusicManager>().HandleMessage();
        spawner.StartSpawning();
    }
    public void UpdateScore(int scoreUpdate)
    {
        if (score <= GameWideData.Instance.maxScore)
        {
            score += scoreUpdate;
            if (score < 0)
            {
                score = 0;
            }
            scoreText.text = "score:" + score + " goal:" + passingScore;
            GameWideData.Instance.score = score;
            if (score >= passingScore)
            {
                levelWon = true;
                isPaused = true;
                FindObjectOfType<EnemyBase>().DestroyBase();
                AdvanceToNextLevel();
            }
        }
    }
    private void AdvanceToNextLevel()
    {
        levelManager.currentLevel++;
        if (levelManager.ReturnCurrentLevel() != null)
        {
            WinLevel();
        }
        else
        {
            EndGame(true);
        }
    }
    public void LoadNextLevel()
    {
        sceneTransition.FadeOut(1);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadSplashScreen()
    {
        SceneManager.LoadScene(0);
    }
    public void WinLevel()
    {
        DestroyEnemies();
        popupManager.OnLevelWon();
        source.PlayOneShot(winSound);
    }
    public void EndGame(bool win) // game over, bool for win or lose
    {
        isPaused = true;
        gameOver = true;
        dialogueTrigger.ClearQueue();

        if (win)
        {
            source.PlayOneShot(winGameSound);
            dialogueTrigger.SetMessageDirectly("You Won");
            DestroyEnemies();
        }
        else
        {
            dialogueTrigger.SetMessageDirectly("You Lost");
            DestroyTowers();
            source.PlayOneShot(loseSound);
        }
        popupManager.OnGameOver(win);
        ResetGame();
    }

    public void ResetGame()
    {
        if (GameWideData.Instance != null)
        {
            GameWideData.Instance.GetComponent<MusicManager>().StopSequence();
            GameWideData.Instance.ResetData();
        }
    }

    private static void DestroyTowers()
    {
        Tower[] towers = FindObjectsOfType<Tower>();
        foreach (Tower tower in towers)
        {
            tower.DetonateTower();
        }
    }
    private static void DestroyEnemies()
    {
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemy in enemies)
        {
            enemy.IsEnabled = false;
            enemy.GetComponent<Enemy>().DetonateEnemy();
        }
    }
    public void ToggleUpgradeMenu()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
