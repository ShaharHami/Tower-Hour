using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FollowBase : MonoBehaviour
{
    private CinemachineVirtualCamera baseCam;
    private Transform lookAtTransform;
    void Awake()
    {
        baseCam = GetComponent<CinemachineVirtualCamera>();
        lookAtTransform = GameObject.FindGameObjectWithTag("ZeroPoint").transform;
        baseCam.transform.LookAt(lookAtTransform);
    }
}
