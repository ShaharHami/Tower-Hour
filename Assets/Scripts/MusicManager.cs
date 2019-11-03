using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public MusicTrack[] tracks;
    public AudioSource source;
    public AudioMixerGroup BGMusic;
    private Queue<MusicTrack> clipQueue;
    [HideInInspector] public bool paused;
    public DialogueTrigger dialogueTrigger;
    private MusicTrack track;
    void Awake()
    {
        clipQueue = new Queue<MusicTrack>();
        source.ignoreListenerPause = false;
        CreatQueue();
    }
    private void CreatQueue()
    {
        foreach (MusicTrack track in tracks)
        {
            clipQueue.Enqueue(track);
        }
        PlaySequence();
    }

    public void PlaySequence()
    {
        dialogueTrigger = FindObjectOfType<DialogueTrigger>();
        bool setDirectly = false;
        if (track != null && dialogueTrigger != null)
        {
            setDirectly = true;
            dialogueTrigger.RemoveFromQueue(track.credit);
        }
        track = clipQueue.Dequeue();
        if (setDirectly && dialogueTrigger != null)
        {
            dialogueTrigger.SetMessageDirectly(track.credit);
            HandleMessage();
        }
        source.clip = track.clip;
        source.Play();
        clipQueue.Enqueue(track);
    }

    public void HandleMessage()
    {
        if (dialogueTrigger == null)
        {
            dialogueTrigger = FindObjectOfType<DialogueTrigger>();
        }
        dialogueTrigger.TriggerDialogue(track.credit);
    }
    public void StopSequence()
    {
        source.Stop();
    }
    void Update()
    {
        if (paused)
        {
            return;
        }
        if (!source.isPlaying && clipQueue.Count > 0)
        {
            PlaySequence();
        }
    }
    public void OnSequencePause()
    {
        if (!paused)
        {
            source.Pause();
            paused = true;
        }
        else
        {
            source.UnPause();
            paused = false;
        }
    }

    [System.Serializable]
    public class MusicTrack
    {
        public AudioClip clip;
        public string credit;
    }
}
