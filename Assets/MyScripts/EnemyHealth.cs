using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class EnemyHealth : MonoBehaviour
{
    Animator animator;
    WaveSpawner waveSpawner;

    [Header("Enemy Attributes")]
    [Tooltip("This character's base maximum health.")]
    [SerializeField] int maxHealth = 50;                                //This enemy's maximum health.
    [Tooltip("The time until the body of this character is removed after death.")]
    [SerializeField] float timeTillRemoval = 5f;                        //The time till a dead enemy is removed, in seconds.

    int currentHealth;

    [Header("Events")]
    public UnityEvent OnEnemyKilled;
    public UnityEvent OnEnemyRemoved;
    public UnityEvent OnTookDamage;

    private void Awake()
    {
        if (OnEnemyKilled == null)
            OnEnemyKilled = new UnityEvent();
        if (OnEnemyRemoved == null)
            OnEnemyRemoved = new UnityEvent();
        if (OnTookDamage == null)
            OnTookDamage = new UnityEvent();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        waveSpawner = GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (transform.position.y <= -12.0f)
        {
            waveSpawner.UpdateEnemyCount(-1);
            Debug.Log("Removed excess enemies and updated current enemy count.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Deals damage to the enemy.
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="player"></param>
    public void TakeDamage(int damage, Transform player)
    {
        animator.SetTrigger("Hurt");
        OnTookDamage.Invoke();

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die(player);
            waveSpawner.UpdateEnemiesKilled();
        }
    }

    /// <summary>
    /// Kills the enemy, plays death animation, and invokes the OnEnemyKilled event.
    /// </summary>
    /// <param name="player"></param>
    void Die(Transform player)
    {
        OnEnemyKilled.Invoke();

        Debug.Log("<color=maroon>" + transform.name + "</color> died.");

        //Disables this enemy's colliders and physics.
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        GetComponent<Rigidbody2D>().gravityScale = 0.0f;
        foreach (Collider2D col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }

        StartCoroutine(RemoveObject(timeTillRemoval));        
    }

    /// <summary>
    /// Removes the enemy object after the specified period of time.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    IEnumerator RemoveObject(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Deathbox")
        {
            OnEnemyRemoved.Invoke();
            Debug.Log("Removed.");
            Destroy(gameObject);
        }
    }
}
