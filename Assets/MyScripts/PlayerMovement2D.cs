using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("References")]
    public PlayerController2D controller;
    Animator animator;

    [Header("Player Stats")]
    [Tooltip("The player's base movement speed.")]
    [SerializeField] float walkSpeed = 5f;              //The player's default movement speed.
    [Tooltip("The percentage of the player's default speed that sprinting will increase movement speed by. 0.3 = 30% increase, or 130% speed.")]
    [SerializeField] float sprintMultiplier = 0.5f;     //The percentage of the player's default speed that sprinting will increase movement speed by. 0.3 = 30% increase, or 130% speed.

    float horizontalMoveSpeed = 0f;
    float isSprinting = 0f;
    bool jump = false;
    bool sprint = false;
    bool walk = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController2D>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(horizontalMoveSpeed));

        float baseSpeed = Input.GetAxisRaw("Horizontal") * walkSpeed;                       //Base speed from which subsequent calculations will be made.
        horizontalMoveSpeed = baseSpeed + (isSprinting * sprintMultiplier * baseSpeed);     //"Final" speed with sprint modifier applied.
        Debug.Log("<color=blue>Player Speed: </color>" + horizontalMoveSpeed);
        Debug.Log("Is shift being pressed: " + Input.GetKeyDown(KeyCode.LeftShift));

        //Detects if player pressed the jump key.
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;

            animator.SetBool("Jump", true);
            animator.SetTrigger("Jump");

            Debug.Log("Pressed jump key.");
        }

        //Detects if player is pressing the sprint key.
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Horizontal") != 0)
        {
            walk = false;
            sprint = true;
            isSprinting = 1.0f;
            animator.SetBool("Sprint", true);
            Debug.Log("Pressed sprint key. isSprinting: " + isSprinting);
        }
        else
        {
            //If the player is moving horizontally but not sprinting, they are walking. Otherwise, they are idle/not walking.
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                walk = true;
                Debug.Log("Is walking: " + walk);
            }
            else
            {
                walk = false;
            }
            sprint = false;
            isSprinting = 0f;
            animator.SetBool("Sprint", false);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMoveSpeed * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    /// <summary>
    /// Stops the jump animation when called.
    /// </summary>
    public void OnLanding()
    {
        animator.SetBool("Jump", false);
    }

    /// <summary>
    /// Returns whether or not player is sprinting.
    /// </summary>
    /// <returns></returns>
    public bool IsSprinting()
    {
        return sprint;
    }

    /// <summary>
    /// Returns whether or not player is walking.
    /// </summary>
    /// <returns></returns>
    public bool IsWalking()
    {
        return walk;
    }
}
