using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.target.gameObject != null)
            {
                PrepareSound(sound, null);
            }
        }
    }

    private static void PrepareSound(Sound sound, Transform target)
    {
        if (sound.target == null)
        {
            sound.source = target.gameObject.AddComponent<AudioSource>();
        }
        else
        {
            if (sound.target.gameObject.GetComponent<AudioSource>() == null)
            {
                sound.source = target.gameObject.AddComponent<AudioSource>();
            }
            else
            {
                sound.source = sound.target.gameObject.AddComponent<AudioSource>();
            }
        }

        sound.source.clip = sound.clip;
        sound.source.volume = sound.volume;
        sound.source.loop = sound.loop;
        sound.source.pitch = sound.pitch;
        sound.source.spatialBlend = sound.spatial;
        sound.source.minDistance = sound.minDistance;
        sound.source.maxDistance = sound.maxDistance;
        sound.source.spatialize = sound.spatialize;
        sound.source.SetCustomCurve(AudioSourceCurveType.SpatialBlend, sound.spatialCurve);
    }

    public void AttachSound(string name, Transform target)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        PrepareSound(s, target);
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        // s.source.playcl
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }
}
