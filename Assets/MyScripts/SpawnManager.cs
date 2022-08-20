using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemies")]                                             //References to enemy assets to be spawned and their spawn weights.
    [SerializeField] GameObject enemy1;
    [SerializeField] int enemy1SpawnWeight;
    [SerializeField] GameObject enemy2;
    [SerializeField] int enemy2SpawnWeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SortEnemies();   
    }

    /// <summary>
    /// Takes referenced enemy prefabs and sorts them based on inputted spawn weight.
    /// </summary>
    void SortEnemies()
    {

    }
}
