using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static List<int> scores = new List<int>();

    public static void CreateScores()
    {
        PlayerPrefs.SetInt("Score 1", 0);
        PlayerPrefs.SetInt("Score 2", 0);
        PlayerPrefs.SetInt("Score 3", 0);
        PlayerPrefs.SetInt("Score 4", 0);
        PlayerPrefs.SetInt("Score 5", 0);
        PlayerPrefs.SetInt("New Score", 0);
    }

    public static void SaveScore(int score)
    {
        int score1 = PlayerPrefs.GetInt("Score 1");
        int score2 = PlayerPrefs.GetInt("Score 2");
        int score3 = PlayerPrefs.GetInt("Score 3");
        int score4 = PlayerPrefs.GetInt("Score 4");
        int score5 = PlayerPrefs.GetInt("Score 5");
        int newScore = score;

        //Put scores into a list.

        scores.Add(score1);
        scores.Add(score2);
        scores.Add(score3);
        scores.Add(score4);
        scores.Add(score5);
        scores.Add(newScore);

        //Sort the list in descending order.
        scores.Sort();
        scores.Reverse();

        //Remove the last (sixth) score.
        scores.RemoveAt(5);

        //Set PlayerPrefs to new scores.
        PlayerPrefs.SetInt("Score 1", scores.ToArray()[0]);
        PlayerPrefs.SetInt("Score 2", scores.ToArray()[1]);
        PlayerPrefs.SetInt("Score 3", scores.ToArray()[2]);
        PlayerPrefs.SetInt("Score 4", scores.ToArray()[3]);
        PlayerPrefs.SetInt("Score 5", scores.ToArray()[4]);
    }

    public static List<int> LoadScores()
    {
        return scores;
    }
}
