using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement2D))]
public class PlayerCombat2D : MonoBehaviour
{
    Animator animator;
    PlayerMovement2D playerMovement;

    [Header("References")]
    [SerializeField] Transform attackPoint;                             //The Transform from which attackRange will be calculated (attack hitbox).
    [SerializeField] LayerMask enemyLayers;                             //The layers on which attacking will check for enemies.
    [SerializeField] UIManager uiManager;

    [Header("Player Attributes")]
    [Tooltip("Default player attack range.")]
    [SerializeField] float attackRange = 1.0f;                          //Default player attack range.
    [Tooltip("Default player attack damage. Integer only.")]
    [SerializeField] int attackDamage = 20;                             //Default player attack damage.
    [Tooltip("How fast the player can attack per second by default.")]
    [SerializeField] float attackRate = 1f;                             //How fast the player can attack by default.
    [Space]
    [Tooltip("How much the default attack range is modified when sprinting. Additive.")]
    [SerializeField] float sprintRangeModifier = -0.4f;                 //How much the default attack range is modified when sprinting. Additive.
    [Tooltip("How much the default attack range is modified when walking. Additive.")]
    [SerializeField] float walkRangeModifier = -0.2f;                   //How much the default attack range is modified when walking. Additive.
    [Tooltip("How much the default attack damage is modified when sprinting. Additive. Integer only.")]
    [SerializeField] int sprintDamageModifier = -10;                    //How much the default attack damage is modified when sprinting. Additive.
    [Tooltip("How much the default attack damage is modified when walking. Additive. Integer only.")]
    [SerializeField] int walkDamageModifier = -5;                       //How much the default attack damage is modified when walking. Additive.
    [Tooltip("How much the default attack rate is increased when sprinting. Multiplier.")]
    [SerializeField] float sprintRateModifier = 2f;                     //How much the default attack rate is increased when sprinting. Multiplier.
    [Tooltip("How much the default attack rate is increased when walking. Multiplier.")]
    [SerializeField] float walkRateModifier = 1.5f;                     //How much the default attack rate is increased when walking. Multiplier.

    float nextAttackTime = 0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement2D>();
        uiManager.UpdateDamageDisplay(attackDamage, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Calls basic attack function if left mouse button is clicked.
        if (Input.GetButtonDown("Fire1"))
        {
            //The different attack "moves" the player can make based on the actions they are already taking.
            if (playerMovement.IsWalking())
            {
                if (Time.time >= nextAttackTime)
                {
                    WalkAttack();
                    nextAttackTime = Time.time + 1f / (attackRate * walkRateModifier);
                }
            }
            else if (playerMovement.IsSprinting())
            {
                if (Time.time >= nextAttackTime)
                {
                    SprintAttack();
                    nextAttackTime = Time.time + 1f / (attackRate * sprintRateModifier);
                }
            }
            else
            {
                if (Time.time >= nextAttackTime)
                {
                    BasicAttack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }                
            }
        }
    }

    /// <summary>
    /// Called when player uses a basic attack.
    /// </summary>
    void BasicAttack()
    {
        animator.SetTrigger("BasicAttack");

        //Collects all detected objects within the drawn circle into a Collider2D array.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("<color=brown>Player hit </color><color=maroon>" + enemy.name + "</color>");
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage, transform);
            StartCoroutine(BasicAttackReturn(0.5f, enemy));
        }

        Debug.Log("<color=red>Player used basic attack.</color>");
    }

    /// <summary>
    /// Called when player attacks while walking.
    /// </summary>
    void WalkAttack()
    {
        animator.SetTrigger("WalkAttack");

        //Collects all detected objects within the drawn circle into a Collider2D array.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange + walkRangeModifier, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("<color=brown>Player hit </color><color=maroon>" + enemy.name + "</color>");
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage + walkDamageModifier, transform);
        }

        Debug.Log("<color=red>Player used walk attack.</color>");
    }

    /// <summary>
    /// Called when player attacks while sprinting.
    /// </summary>
    void SprintAttack()
    {
        animator.SetTrigger("SprintAttack");

        //Collects all detected objects within the drawn circle into a Colider2D array.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange + sprintRangeModifier, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("<color=brown>Player hit </color><color=maroon>" + enemy.name + "</color>");
            enemy.GetComponent<EnemyHealth>().TakeDamage(attackDamage + sprintDamageModifier, transform);
        }

        Debug.Log("<color=red>Player used sprint attack.</color>");
    }

    public int ChangeDamage(int amount)
    {
        attackDamage += amount;
        return attackDamage;
    }

    /// <summary>
    /// Calls the second half of the basic attack's swing damage.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator BasicAttackReturn(float time, Collider2D target)
    {
        yield return new WaitForSeconds(time);
        target.GetComponent<EnemyHealth>().TakeDamage(attackDamage, transform);
    }

    /// <summary>
    /// Draws a circle representing the player's attack range in the Editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange + walkRangeModifier);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange + sprintRangeModifier);
    }
}
