using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    int score = 0;
    void Update()
    {
        Cheat();
    }
    public int GetScore()
    {
        return PlayerPrefs.GetInt("highScore");
    }
    public void SetScore(int _score)
    {
        score = _score;
        PlayerPrefs.SetInt("highScore", score);
    }
    private void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetScore(0);
            FindObjectOfType<GameManager>().UpdateScore(0);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            SetScore(1000);
            FindObjectOfType<GameManager>().UpdateScore(1000);
        }
    }
}
