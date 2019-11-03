using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThis : MonoBehaviour
{
    public float delay;
    void Start()
    {
        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
        }
        Invoke("DestroyFX", delay);
    }
    void DestroyFX()
    {
        Destroy(gameObject);
    }
}
