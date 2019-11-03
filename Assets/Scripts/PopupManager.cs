using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PopupManager : MonoBehaviour
{
    [Header("Popups")]
    [SerializeField] Transform viewHighscoresPopup;
    [SerializeField] Transform gameOverPopup;
    [SerializeField] Transform levelWonPopup;
    [SerializeField] Transform howToPlayPopup;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Animator animator;
    [SerializeField] SceneTransition sceneTransition;
    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void OnBackToMain()
    {
        if (gameManager.IsPaused)
        {
            gameManager.ToggleUpgradeMenu();
        }
        gameManager.ResetGame();
        sceneTransition.FadeOut(0);
    }
    public void OnPlayAgain()
    {
        gameManager.ResetGame();
        sceneTransition.FadeOut(1);
    }
    public void OnClosePopup()
    {
        if (gameManager != null && gameManager.GameOver)
        {
            OnGameOver(gameManager.LevelWon);
        }
        else
        {
            animator.SetBool("ViewHighScores", false);
            animator.SetBool("HowToPlay", false);
            animator.SetBool("GameOver", false);
        }
    }
    public void OnViewHighscores()
    {
        ShowPopup("Highscores");
    }
    public void OnLevelWon()
    {
        ShowPopup("LevelWon");
    }
    public void OnHowToPlay()
    {
        howToPlayPopup.GetComponent<Pagination>().ResetPages();
        ShowPopup("HowToPlay");
    }
    public void OnGameOver(bool win)
    {
        gameOverPopup.gameObject.SetActive(true);
        ShowPopup("Game Over", win);
    }
    public void ShowPopup(string popup, bool win = false)
    {
        switch (popup)
        {
            case "Game Over":
                animator.SetBool("ViewHighScores", false);
                animator.SetBool("LevelWon", false);
                animator.SetBool("GameOver", true);
                scoreText.text = GameWideData.Instance.score.ToString();
                gameOverPopup.GetComponent<WinLose>().SetGameOverPopupMessage(win);
                break;
            case "Highscores":
                animator.SetBool("GameOver", false);
                animator.SetBool("HowToPlay", false);
                animator.SetBool("LevelWon", false);
                animator.SetBool("ViewHighScores", true);
                break;
            case "LevelWon":
                animator.SetBool("GameOver", false);
                animator.SetBool("HowToPlay", false);
                animator.SetBool("ViewHighScores", false);
                animator.SetBool("LevelWon", true);
                break;
            case "HowToPlay":
                animator.SetBool("HowToPlay", true);
                animator.SetBool("ViewHighScores", false);
                break;
            default:
                break;
        }
    }
}
