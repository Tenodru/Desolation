using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyMovement2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] LayerMask detectLayer;                             //The layers on which this enemy will look for objects of interest.

    Animator animator;
    Rigidbody2D rb;
    Transform target;

    [Header("Enemy Attributes")]
    [Tooltip("This character's base move speed.")]
    [SerializeField] float moveSpeed = 10.0f;                           //This enemy's movespeed.
    [Tooltip("This character's base detection radius for objects of interest.")]
    [SerializeField] float detectRadius = 4.0f;                         //This enemy's detection radius.
    [Tooltip("This character's stun time (i-frames) between taking damage.")]
    [SerializeField] float stunTime = 0.5f;                             //How long this enemy will be stunned for when attacked.

    bool reachedTarget = false;
    bool foundTarget = false;
    bool isDead = false;
    bool tookDamage = false;
    bool facingRight = true;
    Collider2D[] detectedObjects;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Detect();
        reachedTarget = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Uses detectRadius to see if there is anything of interest, then moves the enemy.
    /// </summary>
    void Detect()
    {
        //Collects all detected objects within the drawn circle into a Collider2D array.
        detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectRadius, detectLayer);

        foreach (Collider2D col in detectedObjects)
        {
            if (col.tag == "Player" && !isDead)
            {
                target = col.transform;
                foundTarget = true;
                Debug.Log("<color=brown>" + transform.name + " found </color><color=maroon>" + col.name + "</color>");
            }
            else foundTarget = false;

            if (Vector2.Distance(transform.position, target.position) > detectRadius)
            {
                foundTarget = false;
            }
        }
    }

    /// <summary>
    /// Moves this character.
    /// </summary>
    private void Move()
    {
        if (reachedTarget == true)
            return;
        if (foundTarget == false)
        {
            animator.SetBool("FoundTarget", false);
            Debug.Log("No target.");
            return;
        }
        if (tookDamage == true)
        {
            return;
        }

        animator.SetBool("FoundTarget", true);
        animator.SetBool("ReachedTarget", false);

        Debug.Log("Took damage: " + tookDamage);
        Debug.Log("Found target: " + foundTarget);

        Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);

        /*
        //Briefly stuns the enemy after being attacked.
        if (tookDamage)
        {
            newPos = new Vector2(rb.position.x, rb.position.y);
        }*/

        rb.MovePosition(newPos);

        float distance = target.position.x - rb.position.x;
        if (distance > 0 && facingRight == false)
        {
            Flip();
        }
        else if (distance < 0 && facingRight == true)
        {
            Flip();
        }
    }

    /// <summary>
    /// Flips this character's sprite to face the target.
    /// </summary>
    private void Flip()
    {
        //Switch the way this character is currently labelled as facing.
        facingRight = !facingRight;

        //Flip this character's sprite by multiplying the sprite's x scale by -1.
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    /// <summary>
    /// Stops character movement and plays appropriate animations.
    /// </summary>
    public void StopMovement()
    {
        reachedTarget = true;
        animator.SetBool("FoundTarget", false);
        animator.SetBool("ReachedTarget", true);
        Debug.Log("Stopped movement.");
    }

    /// <summary>
    /// Sets isDead to true.
    /// </summary>
    public void IsDead()
    {
        isDead = true;
    }

    public void TookDamage()
    {
        tookDamage = true;
        StartCoroutine(StunTime(stunTime));
    }

    IEnumerator StunTime(float time)
    {
        yield return new WaitForSeconds(time);
        tookDamage = false;
    }

    public float GetStunTime()
    {
        return stunTime;
    }

    /// <summary>
    /// Draws a circle representing this enemy's detection area.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}
