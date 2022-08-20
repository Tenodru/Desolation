using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] UIManager uiManager;

    [Header("Player Attributes")]
    [Tooltip("This character's maximum health.")]
    [SerializeField] int maxHealth = 100;                       //The player's maximum health.

    int currentHealth;

    [Header("Events")]
    public UnityEvent OnTookDamage;
    public UnityEvent OnPlayerKilled;

    Animator animator;

    private void Awake()
    {
        if (OnTookDamage == null)
            OnTookDamage = new UnityEvent();
        if (OnPlayerKilled == null)
            OnPlayerKilled = new UnityEvent();
        currentHealth = maxHealth;
        uiManager.UpdateHealthDisplay(currentHealth);
        uiManager.UpdateMaxHealthDisplay(maxHealth, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Deals damage to the player.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hurt");
        OnTookDamage.Invoke();

        currentHealth -= damage;
        uiManager.UpdateHealthDisplay(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Kills the player, plays death animation, and invokes the OnPlayerKilled event.
    /// </summary>
    void Die()
    {
        OnPlayerKilled.Invoke();

        animator.SetBool("IsDead", true);

        //Disables this character's colliders and physics.
        GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
    }

    /// <summary>
    /// Alters the player's maximum health.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public int ChangeMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        uiManager.UpdateHealthDisplay(currentHealth);

        return maxHealth;
    }
}
