using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

public class HighScoreTable : MonoBehaviour
{
    public static HighScoreTable Instance;
    [SerializeField] float templateHeight = 56f;
    [SerializeField] int maxRecords = 10;
    [SerializeField] Transform entryContainer, entryTemplate;
    public bool isDelete = false;
    private List<Transform> highScoreEntryTransformList;
    void Awake()
    {
        CreateList();
        DeleteList();
    }
    private void CreateList()
    {
        foreach (Transform transform in entryContainer)
        {
            GameObject.Destroy(transform.gameObject);
        }
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
        if (highScores != null)
        {
            SortHighScores(highScores);
            highScoreEntryTransformList = new List<Transform>();
            foreach (HighScoreEntry highScoreEntry in highScores.highScoreEntryList)
            {
                CreateHighScoreEntryTransform(highScoreEntry, entryContainer, highScoreEntryTransformList);
            }
        }
    }

    private void DeleteList()
    {

        // if (CrossPlatformInputManager.GetButton("DeleteList") && isDelete)
        // {
        //     Debug.LogWarning("highScoreTable Deleted!!");
        //     PlayerPrefs.DeleteKey("highScoreTable");
        // }
    }
    private void SortHighScores(HighScores highScores)
    {
        if (highScores != null)
        {
            // sort entry list by score
            for (int i = 0; i < highScores.highScoreEntryList.Count; i++)
            {
                for (int j = i + 1; j < highScores.highScoreEntryList.Count; j++)
                {
                    if (highScores.highScoreEntryList[j].score > highScores.highScoreEntryList[i].score)
                    {
                        HighScoreEntry tmp = highScores.highScoreEntryList[i];
                        highScores.highScoreEntryList[i] = highScores.highScoreEntryList[j];
                        highScores.highScoreEntryList[j] = tmp;
                    }
                }
            }
        }
    }

    private void CreateHighScoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * (transformList.Count));
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }
        TextMeshProUGUI rankTextMesh = entryTransform.Find("PosText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreTextMesh = entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameTextMesh = entryTransform.Find("NameText").GetComponent<TextMeshProUGUI>();
        rankTextMesh.text = rankString;
        int score = highScoreEntry.score;
        scoreTextMesh.text = score.ToString();
        string name = highScoreEntry.name;
        nameTextMesh.text = name;
        rankTextMesh.enabled = true;
        scoreTextMesh.enabled = true;
        nameTextMesh.enabled = true;

        transformList.Add(entryTransform);
    }
    public void AddHighScoreEntry(int score, string name)
    {
        // Creat new entry
        HighScoreEntry highScoreEntry = new HighScoreEntry { score = score, name = name };
        // Load saved entry list
        string jsonString = PlayerPrefs.GetString("highScoreTable");
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);
        if (highScores == null)
        {
            highScores = new HighScores();
            highScores.highScoreEntryList = new List<HighScoreEntry>();
        }
        // Add new entry to list
        highScores.highScoreEntryList.Add(highScoreEntry);
        // Sort
        SortHighScores(highScores);
        // Control list length
        while (highScores.highScoreEntryList.Count > maxRecords)
        {
            highScores.highScoreEntryList.RemoveAt(highScores.highScoreEntryList.Count - 1);
        }
        // Save updated list 
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString("highScoreTable", json);
        PlayerPrefs.Save();
        if (highScoreEntryTransformList != null)
        {
            highScoreEntryTransformList.Clear();
        }
        CreateList();
    }
    public void ToggleHighScoreTable()
    {
        Canvas mainCanvas = transform.parent.GetComponent<Canvas>();
        mainCanvas.enabled = !mainCanvas.enabled;
    }
    private class HighScores
    {
        public List<HighScoreEntry> highScoreEntryList;
    }
    /*
    Representing a single highscore entry
     */
    [System.Serializable]
    private class HighScoreEntry
    {
        public int score;
        public string name;
    }
}
