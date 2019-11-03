using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinLose : MonoBehaviour
{
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseText;
    [SerializeField] GameObject highScoresModule;
    private bool won;
    public void SetGameOverPopupMessage(bool win)
    {
        won = win;
        SetMessage();
    }
    private void SetMessage()
    {
        if (won)
        {
            winText.SetActive(true);
            // highScoresModule.SetActive(true);
            loseText.SetActive(false);
        }
        else
        {
            winText.SetActive(false);
            // highScoresModule.SetActive(false);
            loseText.SetActive(true);
        }
    }
}
