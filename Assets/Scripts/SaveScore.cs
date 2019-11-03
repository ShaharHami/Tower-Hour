using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveScore : MonoBehaviour
{
    public Button button;
    public TMP_InputField inputField;
    public void OnButtonClick()
    {
        int score = FindObjectOfType<GameManager>().Score;
        if (inputField.text != "" && inputField.text != null)
        {
            GameObject.FindObjectOfType<HighScoreTable>().AddHighScoreEntry(score, inputField.text);
            Destroy(button.transform.parent.gameObject);
        }
    }
}
