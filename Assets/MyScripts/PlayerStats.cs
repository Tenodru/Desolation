using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    #region Singleton
    public static PlayerStats instance;

    private void Awake()
    {
        instance = this;

        if (OnGoldCollect == null)
            OnGoldCollect = new UnityEvent();
        if (OnExpCollect == null)
            OnExpCollect = new UnityEvent();
        uiManager.UpdateGoldDisplay(gold);
    }
    #endregion                                      

    [Header("References")]
    [SerializeField] Transform pickupPoint;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] UIManager uiManager;

    [Header("Player Attribute Modifiers")]
    [Tooltip("How much the player's experience gain is multiplied by.")]
    [SerializeField] int expGain = 1;
    [Tooltip("How much the player's gold gain is multiplied by.")]
    [SerializeField] int goldGain = 1;
    [Tooltip("The player's pickup detection range for items.")]
    [SerializeField] float pickupRange = 1.0f;

    [Header("Events")]
    public UnityEvent OnGoldCollect;
    public UnityEvent OnExpCollect;

    int gold = 0;
    int experience = 0;
    int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Collects all detected objects within the drawn circle into a Colider2D array.
        Collider2D[] pickups = Physics2D.OverlapCircleAll(pickupPoint.position, pickupRange, pickupLayer);

        //Calls OnPickup method for each pickup within the detection range.
        foreach (Collider2D pickup in pickups)
        {
            pickup.GetComponent<Pickup>().OnPickup(transform);
        }
    }

    /// <summary>
    /// Collects the pickup. Type 0 = Coin.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="type"></param>
    public void Collect(int amount, int type)
    {
        switch (type)
        {
            case 0:
                gold++;
                score += 50;
                OnGoldCollect.Invoke();
                uiManager.UpdateGoldDisplay(gold);
                uiManager.UpdateScoreDisplay(score);

                Debug.Log("<color=brown>Picked up </color><color=maroon>Coin</color>");
                Debug.Log("Current Gold: " + gold);
                break;
            case 1:
                experience++;
                score += 5;
                OnExpCollect.Invoke();
                uiManager.UpdateScoreDisplay(score);

                Debug.Log("<color=brown>Picked up </color><color=maroon>Experience</color>");
                break;
        }
    }

    public int ChangeGold(int amount)
    {
        gold += amount;
        return gold;
    }

    /// <summary>
    /// Increases the player's experience by the specified amount.
    /// </summary>
    /// <param name="amount"></param>
    public void IncreaseExperience(int amount)
    {
        experience += amount;
    }

    public int GetScore()
    {
        return score;
    }

    private void OnDrawGizmosSelected()
    {
        if (pickupPoint == null)
            return;
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(pickupPoint.position, pickupRange);
    }
}
