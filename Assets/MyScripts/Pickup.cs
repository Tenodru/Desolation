using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Pickup : MonoBehaviour
{
    public enum PickupType { Coin, Experience };

    Animator animator;
    Rigidbody2D rb;

    [Header("Pickup Attributes")]
    [Tooltip("The type of item this is.")]
    [SerializeField] PickupType type;
    [Tooltip("How fast this item moves to a collector by default.")]
    [SerializeField] float moveSpeed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnPickup(Transform target)
    {
        if (type == PickupType.Coin)
        {
            Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, moveSpeed * Time.fixedDeltaTime);

            rb.MovePosition(newPos);

            //Pickup is actually collected by character.
            if (Mathf.Abs (rb.position.x - targetPos.x) < 0.1)
            {
                target.GetComponent<PlayerStats>().Collect(1, 0);
                Destroy(gameObject);
            }
        }
    }
}
