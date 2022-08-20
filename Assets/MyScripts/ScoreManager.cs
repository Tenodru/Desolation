using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScoreManager : MonoBehaviour
{
    private List<Score> testList;
    private List<Score> scoresList;
    private ScoreList scores;

    // Start is called before the first frame update
    void Start()
    {
        testList = new List<Score>() {
            new Score { score = 0, name = "Max" },
            new Score { score = 0, name = "Mike" },
            new Score { score = 0, name = "Mark" },
        };

        ScoreList newList = new ScoreList { scoreList = testList };
        string json = JsonUtility.ToJson(SortList(newList.scoreList));

        if (PlayerPrefs.HasKey("scoreList") == false)
        {
            PlayerPrefs.SetString("scoreList", json);
            PlayerPrefs.Save();
        }
    }

    public void SaveScore(int score, string name)
    {
        //Creates new Score entry
        Score newScore = new Score { score = score, name = name };
        ScoreList newList = new ScoreList();

        //Load saved Scores
        string jsonString = PlayerPrefs.GetString("scoreList");
        newList = JsonUtility.FromJson<ScoreList>(jsonString);

        //Add new entry to Scores
        newList.scoreList.Add(newScore);
        newList.scoreList = SortList(newList.scoreList);

        //Sort updated Scores and save
        string json = JsonUtility.ToJson(newList);
        PlayerPrefs.SetString("scoreList", json);
        PlayerPrefs.Save();
        Debug.Log("Scores :" + json);
        scores = newList;
    }

    public List<Score> LoadScores()
    {
        scoresList = scores.scoreList;
        return scoresList;
    }

    /// <summary>
    /// Sorts and orders the given scorelist from highest to lowest scores, up to five.
    /// </summary>
    /// <param name="scores"></param>
    /// <returns></returns>
    private List<Score> SortList(List<Score> scores)
    {
        if (scores.Count == 0)
        {
            return null;
        }
        else
        {
            for (int i = 0; i < scores.Count; i++)
            {
                for (int j = 0; j < scores.Count; j++)
                {
                    if (scores[j].score < scores[i].score)
                    {
                        //Swap Score entries
                        Score temp = scores[i];
                        scores[i] = scores[j];
                        scores[j] = temp;
                    }
                }
            }

            if (scores.Count > 5)
            {
                scores.RemoveAt(scores.Count - 1);
            }

            return scores;
        }
    }

    public List<Score> GetScores()
    {
        if (testList.Count == 0)
        {
            Debug.Log("No score list found!");
            return null;
        }

        else return testList;
    }

    /// <summary>
    /// A List of Score objects.
    /// </summary>
    private class ScoreList
    {
        public List<Score> scoreList;
    }
}
