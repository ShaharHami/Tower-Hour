using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogues;
    private LinkedList<Dialogue> dialogueList;
    [HideInInspector] public PlayerMessages playerMessages;
    private void Awake()
    {
        dialogueList = new LinkedList<Dialogue>();
        playerMessages = FindObjectOfType<PlayerMessages>();
    }

    // Queue up
    public void TriggerDialogue(string name)
    {
            foreach (Dialogue dialogue in dialogues)
            {
                if (dialogue.name == name && !dialogueList.Contains(dialogue))
                {
                    dialogueList.AddFirst(dialogue);
                }
                StartCoroutine(SendToPlayerMessages());
            }
    }
    // Cut queue
    public void CutQueue(string name)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.name == name)
            {
                dialogueList.AddLast(dialogue);
                StartCoroutine(SendToPlayerMessages());
            }
        }
    }
    public void RemoveFromQueue(string name)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.name == name)
            {
                dialogueList.Remove(dialogue);
                // StartCoroutine(SendToPlayerMessages());
            }
        }
    }
    public void ClearQueue()
    {
        dialogueList = new LinkedList<Dialogue>();
    }
    public void RemoveLast()
    {
        dialogueList.RemoveLast();
        StartCoroutine(SendToPlayerMessages());
    }
    public void SetMessageDirectly(string name)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            if (dialogue.name == name)
            {
                playerMessages.StartMessage(dialogue);
            }
        }
    }
    IEnumerator SendToPlayerMessages()
    {
        if (dialogueList.Count > 0)
        {
            if (playerMessages != null && playerMessages.messageDidEnd)
            {
                Dialogue dialogue = dialogueList.Last.Value;
                playerMessages.StartMessage(dialogue);
                RemoveFromQueue(dialogue.name);
                if (dialogue.repeating)
                {
                    TriggerDialogue(dialogue.name);
                }
            }
            else
            {
                yield return new WaitForSecondsRealtime(0.5f);
                StopAllCoroutines();
                StartCoroutine(SendToPlayerMessages());
            }
        }
    }
    void LogList()
    {
        foreach (var node in dialogueList)
        {
            print("node " + node.name);
        }
    }
}
