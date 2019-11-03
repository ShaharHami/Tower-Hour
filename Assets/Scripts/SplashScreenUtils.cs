using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashScreenUtils : MonoBehaviour
{
    [SerializeField] SceneTransition sceneTransition;
    [SerializeField] Image blackScreen;
    EnvironmentRandomizer randomizer;
    private void Awake()
    {
        sceneTransition.FadeIn();
        randomizer = GetComponent<EnvironmentRandomizer>();
        randomizer.Randomize();
    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnPlayGame()
    {
        sceneTransition.FadeOut(1); // Load Game
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }
}
