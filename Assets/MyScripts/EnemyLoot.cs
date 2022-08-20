using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyHealth))]
public class EnemyLoot : MonoBehaviour
{
    [Header("Loot Prefabs")]
    [SerializeField] GameObject coin;

    [Header("Enemy Attributes")]
    [Tooltip("This character's chance to drop a coin on death.")]
    [SerializeField] float coinDropChance = 0.5f;

    EnemyHealth enemyHealth;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void DropLoot()
    {
        float chance = Random.Range(0, 1.0f);
        Quaternion rot = new Quaternion(0, 0, 0, 0);
        if (chance <= coinDropChance)
        {
            Instantiate(coin, transform.position, rot);
        }
    }
}
