using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    public bool repeating;
    public Sentence[] sentences;
}
[System.Serializable]
public class Sentence
{
    [TextArea(3,10)]
    public string sentence;
    public float delay;
}
