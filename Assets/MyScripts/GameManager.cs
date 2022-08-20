using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent OnGameOpen;
    public UnityEvent OnGameStart;
    public UnityEvent OnGameVictory;
    public UnityEvent OnGameLoss;

    bool isPaused = false;

    [Header("References")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] UIManager uiManager;

    private void Awake()
    {
        if (OnGameOpen == null)
            OnGameOpen = new UnityEvent();
        if (OnGameStart == null)
            OnGameStart = new UnityEvent();
        if (OnGameVictory == null)
            OnGameVictory = new UnityEvent();
        if (OnGameLoss == null)
            OnGameLoss = new UnityEvent();

        OnGameOpen.Invoke();
        GameStart();
    }

    public void GameStart()
    {
        //SaveManager.CreateScores();
    }

    public void GameLoss()
    {
        OnGameLoss.Invoke();
    }

    public void GameEndScreen()
    {
        scoreManager.SaveScore(playerStats.GetScore(), uiManager.GetNameInput());
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
}
