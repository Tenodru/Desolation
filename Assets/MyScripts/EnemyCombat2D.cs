using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyMovement2D))]
public class EnemyCombat2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask targetLayers;

    [Header("Enemy Attributes")]
    [Tooltip("This character's base attack damage.")]
    [SerializeField] int attackDamage = 10;
    [Tooltip("This character's base attack range.")]
    [SerializeField] float attackRange = 2.0f;
    [Tooltip("This character's base attack rate.")]
    [SerializeField] float attackRate = 3.0f;
    [Tooltip("This character's base attack detection range.")]
    [SerializeField] float attackDetectRange = 1.5f;                        //When a target is detected within this range, this enemy will stop to attack.
    [Tooltip("This character's base attack delay, in seconds.")]
    [SerializeField] float attackDelay = 2.0f;                              //How long this character will wait before attacking after detecting a target.

    float nextAttackTime = 2f;

    [Header("Events")]
    public UnityEvent TargetWithinAttackRange;

    EnemyMovement2D enemyMovement;
    Animator animator;

    IEnumerator co;

    bool isDead = false;
    bool tookDamage = false;

    private void Awake()
    {
        if (TargetWithinAttackRange == null)
            TargetWithinAttackRange = new UnityEvent();
        co = Attack(attackDelay);
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement2D>();
        animator = GetComponent<Animator>();
        co = Attack(attackDelay);
    }

    // Update is called once per frame
    void Update()
    {
        co = Attack(attackDelay);
        Detect();
        if (isDead)
        {
            StopCoroutine(co);
            return;
        }
    }

    /// <summary>
    /// Looks for objects on the target layers within a circle of radius attackDetectRange.
    /// </summary>
    void Detect()
    {
        if (isDead)
        {
            StopCoroutine(co);
            return;
        }

        //Stuns the enemy for stunTime when attacked.
        if (tookDamage == true)
        {
            StopCoroutine(co);
            return;
        }

        float delayTimer = 0f;

        //Collects all detected objects within the drawn circle into a Collider2D array.
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackDetectRange, targetLayers);

        //If any targets are detected, this event will be invoked and the enemy will attack after a short delay.
        if (hitTargets.Length > 0)
        {
            delayTimer = Time.time;
            TargetWithinAttackRange.Invoke();

            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(co);
                nextAttackTime = Time.time + 1f / attackRate;
            }   
        }
    }

    /// <summary>
    /// Deals damage to the target after the specified time.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator Attack(float time)
    {
        animator.SetBool("ReachedTarget", true);
        yield return new WaitForSeconds(time);

        //Collects all detected objects within the drawn circle into a Collider2D array again.
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackPoint.position, attackDetectRange, targetLayers);

        foreach (Collider2D target in hitTargets)
        {
            animator.SetTrigger("BasicAttack");

            //Stuns the enemy for stunTime when attacked.
            if (tookDamage == true)
            {
                yield break;
            }

            Debug.Log("<color=brown>Enemy hit </color><color=maroon>" + target.name + "</color>");
            target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

    /// <summary>
    /// Upon taking damage, stun this character for stunTime.
    /// </summary>
    public void TookDamage()
    {
        tookDamage = true;
        StartCoroutine(StunTime(enemyMovement.GetStunTime()));
        Debug.Log("Stunned enemy.");
    }

    /// <summary>
    /// After stunTime has passed, resume combat logic for this character.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator StunTime(float time)
    {
        yield return new WaitForSeconds(time);
        tookDamage = false;
        Update();
    }

    public void IsDead()
    {
        isDead = true;
        animator.SetBool("ReachedTarget", false);
        animator.SetBool("IsDead", true);
    }

    /// <summary>
    /// Draws a circle representing the enemy's attack range in the Editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPoint.position, attackDetectRange);
    }
}
