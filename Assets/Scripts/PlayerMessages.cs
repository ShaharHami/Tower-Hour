using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMessages : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerMessageText;
    public bool messageDidEnd = true;
    private bool repeating = false;
    private Queue<Sentence> sentences;
    private GameManager gameManager;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        sentences = new Queue<Sentence>();
    }
    public void StartMessage(Dialogue dialogue)
    {
        messageDidEnd = false;
        sentences.Clear();
        foreach (Sentence sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence(float messageDelay = 0)
    {
        if (sentences.Count == 0)
        {
            EndDialogue(messageDelay);
            return;
        }
        Sentence sentence = sentences.Dequeue();
        SetPlayerMessage(sentence);
    }
    IEnumerator CheckPause(float delay)
    {
        yield return new WaitForSeconds(.5f);
        DisplayNextSentence(delay);
    }
    private void EndDialogue(float delay)
    {
        Invoke("ClearText", delay);
    }
    private void ClearText()
    {
        messageDidEnd = true;
        playerMessageText.text = "";
    }
    public void SetPlayerMessage(Sentence message)
    {
        if (gameObject.activeSelf)
        {
            StopAllCoroutines();
            StartCoroutine(TypeMessage(message));
        }
    }
    IEnumerator TypeMessage(Sentence message)
    {
        playerMessageText.text = "";
        char[] letters = message.sentence.ToCharArray();
        for (int i = 0; i < letters.Length; i++)
        {
            playerMessageText.text += letters[i];
            if (i == letters.Length - 1)
            {
                yield return new WaitForSecondsRealtime(message.delay);
                DisplayNextSentence(message.delay);
            }
            yield return null;
        }
    }
}
