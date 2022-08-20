using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;

    private void Awake()
    {
        instance = this;

        if (OnPause == null)
            OnPause = new UnityEvent();
        if (OnUnpause == null)
            OnUnpause = new UnityEvent();
    }
    #endregion   

    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI goldDisplayText;
    [SerializeField] TextMeshProUGUI healthDisplayText;
    [SerializeField] TextMeshProUGUI scoreDisplayText;
    [SerializeField] TextMeshProUGUI damageDisplayText;
    [SerializeField] TextMeshProUGUI maxHealthDisplayText;
    [SerializeField] Transform shop;
    [SerializeField] Transform scoreboard;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI[] scoreTexts;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] TMP_InputField nameInput;

    [Header("References")]
    [SerializeField] ScoreManager scoreManager;

    [Header("Events")]
    public UnityEvent OnPause;
    public UnityEvent OnUnpause;

    bool shopEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverText.gameObject.SetActive(false);
        scoreboard.gameObject.SetActive(false);
        shop.gameObject.SetActive(false);
        nameInput.gameObject.SetActive(false);

        if (scoreTexts != null)
        {
            for (int i = 0; i < scoreTexts.Length; i++)
            {
                scoreTexts[i].gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (shopEnabled == false)
            {
                shop.gameObject.SetActive(true);
                shopEnabled = true;
                OnPause.Invoke();
            }
            else
            {
                shop.gameObject.SetActive(false);
                shopEnabled = false;
                OnUnpause.Invoke();
            }
        }
    }

    public void UpdateGoldDisplay(int amount)
    {
        goldDisplayText.text = "Gold: " + amount;
    }

    public void UpdateHealthDisplay(int amount)
    {
        healthDisplayText.text = "HP: " + amount;
    }

    public void UpdateScoreDisplay(int amount)
    {
        scoreDisplayText.text = "Score: " + amount;
    }

    public void UpdateDamageDisplay(int orig, int amount)
    {
        int newAmount = orig + amount;
        damageDisplayText.text = newAmount.ToString();
    }

    public void UpdateMaxHealthDisplay(int orig, int amount)
    {
        int newAmount = orig + amount;
        maxHealthDisplayText.text = newAmount.ToString();
    }

    public void ShowGameOverScreen()
    { 
        gameOverText.gameObject.SetActive(true);
        nameInput.gameObject.SetActive(true);   
    }

    public void ShowScoreboard()
    {
        nameInput.gameObject.SetActive(false);
        StartCoroutine(ShowScoreboard(1.0f, scoreManager.LoadScores()));
    }

    public string GetNameInput()
    {
        return nameInput.text;
    }

    IEnumerator ShowScoreboard(float time, List<Score> scoreList)
    {
        yield return new WaitForSeconds(time);

        scoreboard.gameObject.SetActive(true);

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            scoreTexts[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < scoreList.Count; i++)
        {
            scoreTexts[i].text = "" + scoreList.ToArray()[i].name + ":   " + scoreList.ToArray()[i].score;
        }
    }
}
