using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerController : MonoBehaviour
{
    public AudioFollowTarget[] audioFollowTargets;
    void Update()
    {
        foreach (AudioFollowTarget audioFollowTarget in audioFollowTargets)
        {
            GameObject target = GameObject.FindGameObjectWithTag(audioFollowTarget.tag);
            if (target != null)
            {
                transform.localPosition = target.transform.localPosition + audioFollowTarget.offset;
                // transform.rotation = target.transform.rotation;
                transform.LookAt(Vector3.zero);
            }
        }
    }
}
