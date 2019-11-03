using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterialOffset : MonoBehaviour
{
    public Material material;
    void Update()
    {
        float offset = Time.time * 0.0001f;
        material.mainTextureOffset = new Vector2(offset, 0);
    }
}
