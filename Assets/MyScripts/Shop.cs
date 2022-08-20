using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Shop : MonoBehaviour
{
    [Header("Shop Buttons")]
    [Tooltip("The damage stat display in the shop.")]
    [SerializeField] TextMeshProUGUI incDamageDisplay;
    [Tooltip("The max health stat display in the shop.")]
    [SerializeField] TextMeshProUGUI incMaxHealthDisplay;

    [Header("Other References")]
    [SerializeField] UIManager uiManager;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerCombat2D playerCombat;
    [SerializeField] PlayerHealth playerHealth;

    [Header("Attributes")]
    [Tooltip("How much damage will be increased when the upgrade is bought.")]
    [SerializeField] int damageIncreaseOnBuy = 5;
    [Tooltip("How much maximum health will be increased when the upgrade is bought.")]
    [SerializeField] int healthIncreaseOnBuy = 10;

    [Header("Events")]
    public UnityEvent OnIncreaseDamage;
    public UnityEvent OnIncreaseMaxHealth;

    private void Awake()
    {
        if (OnIncreaseDamage == null)
            OnIncreaseDamage = new UnityEvent();
        if (OnIncreaseMaxHealth == null)
            OnIncreaseMaxHealth = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseDamage()
    {
        if (playerStats.ChangeGold(0) >= 5)
        {
            playerStats.ChangeGold(-5);
            playerCombat.ChangeDamage(damageIncreaseOnBuy);
            uiManager.UpdateGoldDisplay(playerStats.ChangeGold(0));
            RefreshDisplays();
        }
    }

    public void IncreaseMaxHealth()
    {
        if (playerStats.ChangeGold(0) >= 5)
        {
            playerStats.ChangeGold(-5);
            playerHealth.ChangeMaxHealth(healthIncreaseOnBuy);
            uiManager.UpdateHealthDisplay(playerHealth.ChangeMaxHealth(0));
            uiManager.UpdateGoldDisplay(playerStats.ChangeGold(0));
            RefreshDisplays();
        }
    }

    public void RefreshDisplays()
    {
        incDamageDisplay.text = playerCombat.ChangeDamage(0).ToString();
        incMaxHealthDisplay.text = playerHealth.ChangeMaxHealth(0).ToString();
    }
}
