using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public bool loop;
    public string name;
    public AudioClip clip;
    public GameObject target;
    [Range(0f, 1f)] public float volume;
    [Range(0.1f, 3f)] public float pitch;
    public bool spatialize;
    [Range(0f, 1f)] public float spatial;
    public float minDistance;
    public float maxDistance;
    public AnimationCurve spatialCurve;
    [HideInInspector] public AudioSource source;
}
