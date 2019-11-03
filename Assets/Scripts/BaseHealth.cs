using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseHealth : MonoBehaviour
{
    int baseMaxHealth = 100;
    private GameManager gameManager;
    [SerializeField] float destroyDelay = 2f;
    [SerializeField] ParticleSystem deathFX;
    private Image healthBarFill;
    TextMeshProUGUI healthTextMesh;
    private int health;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        health = baseMaxHealth = gameManager.BaseMaxHealth;
        healthTextMesh = GameObject.FindGameObjectWithTag("BaseHealthText").GetComponent<TextMeshProUGUI>();
        healthBarFill = GameObject.FindGameObjectWithTag("HealthBarFill").GetComponent<Image>();
        UpdateHelathText(health);
    }
    public void Damage(int damage)
    {
        health -= damage;
        if (health <= 0 && !gameManager.LevelWon)
        {
            health = 0;
            EndGame();
        }
        UpdateHelathText(health);
    }
    private void UpdateHelathText(int health)
    {
        healthTextMesh.text = health + "/" + baseMaxHealth;
        float healthFill = (float)health / (float)baseMaxHealth;
        healthBarFill.fillAmount = healthFill;
    }
    private void EndGame()
    {
        DestroyBase();
        gameManager.EndGame(false);
    }
    private void DestroyBase()
    {
        ParticleSystem deathFXInstance = Instantiate(deathFX, transform.position, Quaternion.identity);
        deathFXInstance.transform.localScale = new Vector3(5,5,5);
        ToggleVisibility();
        Invoke("DestroyBaseGO", destroyDelay);
    }
    private void ToggleVisibility()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
    private void DestroyBaseGO()
    {
        Destroy(gameObject);
    }
}
