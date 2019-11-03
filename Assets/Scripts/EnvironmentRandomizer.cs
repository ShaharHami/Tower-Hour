using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentRandomizer : MonoBehaviour
{
    [SerializeField] Material[] skyboxes;
    public Material worldMat;
    public void Randomize()
    {
        Material skybox = skyboxes[Random.Range(0, skyboxes.Length-1)];
        RenderSettings.skybox = skybox;
        Color color = Random.ColorHSV(0f, 0.4f, 0.5f, 0.7f, 0.5f, 0.7f, 1f, 1f);
        worldMat.color = color;
    }
}
