using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    void Start()
    {
        Invoke("RandomRotateObjectOnY", 0.1f);
    }

    private void RandomRotateObjectOnY()
    {
        float desiredYRot = Random.rotation.eulerAngles.y;
        transform.Rotate(Quaternion.identity.eulerAngles.x, desiredYRot, Quaternion.identity.eulerAngles.z);
    }
}
