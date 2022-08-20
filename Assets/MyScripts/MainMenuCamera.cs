using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [Header("Camera Attributes")]
    [Tooltip("This camera's side scroll speed.")]
    [SerializeField] float moveSpeed = 5.0f;                                //Camera scroll speed.

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.right * Time.deltaTime * moveSpeed;
    }
}
