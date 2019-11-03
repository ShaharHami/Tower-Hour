using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    private float health;
    private float startHealth = 100f;
    [SerializeField] ParticleSystem deathFX;
    [SerializeField] ParticleSystem detonateFX;
    [SerializeField] float destroyDelay = 2f;
    [SerializeField] Transform boosters;
    [SerializeField] Transform body;
    [SerializeField] TextMeshProUGUI[] healthTextMeshes;
    [SerializeField] Transform healthBar;
    [SerializeField] Image healthBarUnderlay;
    [SerializeField] GameObject scoring;
    [SerializeField] Animator scoringAnimator;
    [SerializeField] Color killColor, hitColor;
    [SerializeField] float scoringOffsetY;
    int enemyScore = 1;
    [SerializeField] private Transform followPoint;
    public Transform FollowPoint
    {
        get { return followPoint; }
    }
    [HideInInspector] public bool isDead = false;
    [HideInInspector] public int damage = 10;
    private GameManager gameManager;
    public float StartHealth
    {
        set { startHealth = value; }
    }
    ParticleSystem deathFXInstance;
    ParticleSystem detonateFXInstance;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            health = startHealth = gameManager.enemyHealth;
            damage = gameManager.enemyDamage;
            enemyScore = gameManager.enemyScore;
            UpdateHelathText(health);
        }
    }
    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            KillEnemy();
            gameManager.UpdateScore(enemyScore);
        }
        UpdateHelathText(health);
    }
    private void TriggerScoringAnimation(int scoreToDisplay, bool killOrHit)
    {
        GameObject scoringInstance = Instantiate(scoring, new Vector3(
            transform.position.x,
            transform.position.y + scoringOffsetY,
            transform.position.z
        ), Quaternion.identity, transform.parent);
        TextMeshProUGUI scoringText = scoringInstance.GetComponent<Scoring>().scoringText;
        if (killOrHit)
        {
            scoringText.text = "+" + scoreToDisplay;
            scoringText.color = killColor;
        }
        else
        {
            scoringText.text = "-" + scoreToDisplay;
            scoringText.color = hitColor;
        }
    }
    private void UpdateHelathText(float health)
    {
        if (health < 0)
        {
            health = 0;
        }
        foreach (TextMeshProUGUI healthTextMesh in healthTextMeshes)
        {
            healthTextMesh.text = health + "/" + startHealth;
        }
        Vector3 scaleTo = new Vector3((health / startHealth) * 10,
        healthBar.transform.localScale.y,
        healthBar.transform.localScale.z);
        healthBar.transform.localScale = scaleTo;
    }
    void KillEnemy()
    {
        TriggerScoringAnimation(enemyScore, true);
        deathFXInstance = Instantiate(deathFX, transform.position, Quaternion.identity);
        deathFXInstance.Play();
        DisableEnemy();
    }
    void DestroyFX()
    {
        Destroy(deathFXInstance);
        Destroy(detonateFXInstance);
    }
    public void DetonateEnemy()
    {
        if (!gameManager.LevelWon)
        {
            TriggerScoringAnimation(damage, false);
        }
        UpdateHelathText(0);
        detonateFXInstance = Instantiate(detonateFX, transform.position, Quaternion.identity);
        detonateFXInstance.Play();
        DisableEnemy();
    }
    private void DisableEnemy()
    {
        isDead = true;
        PauseEnemy();
        ToggleVisibility();
        Invoke("DestroyEnemy", destroyDelay);
    }
    public void PauseEnemy()
    {
        ToggleMovement();
        ToggleCollisions();
    }
    private void ToggleVisibility()
    {
        boosters.gameObject.SetActive(false);
        MeshRenderer meshRenderer = body.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        foreach (TextMeshProUGUI healthTextMesh in healthTextMeshes)
        {
            healthTextMesh.enabled = false;
        }
        healthBar.gameObject.SetActive(false);
        healthBarUnderlay.enabled = false;
    }
    private void ToggleMovement()
    {
        EnemyMovement enemyMovement = gameObject.GetComponent<EnemyMovement>();
        enemyMovement.IsEnabled = !enemyMovement.enabled;
    }
    private void ToggleCollisions()
    {
        MeshCollider meshCollider = transform.Find("Enemy_A").GetComponent<MeshCollider>();
        meshCollider.enabled = !meshCollider.enabled;
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
