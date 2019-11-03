using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public LevelPrefs[] levels;
    public int currentLevel;
    public LevelPrefs ReturnCurrentLevel()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (i == currentLevel)
            {
                return levels[i];
            }
        }
        return null;
    }
}
