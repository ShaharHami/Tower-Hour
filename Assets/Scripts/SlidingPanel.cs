using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingPanel : MonoBehaviour
{
    [SerializeField] Animator animator;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnSlidePanel();
        }
    }
    public void OnSlidePanel()
    {
        if (!gameManager.GameOver && !gameManager.LevelWon)
        {
            animator.SetBool("SlidePanel", !animator.GetBool("SlidePanel"));
            gameManager.ToggleUpgradeMenu();
        }
    }
}
