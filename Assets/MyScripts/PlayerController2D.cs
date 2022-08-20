using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Controller Stats")]
    [Tooltip("The base force applied to the player when they jump.")]
    [SerializeField] float jumpForce = 5f;                              //Jump force.
    [Tooltip("Percentage of player max speed applied to crouch.")]
    [Range(0, 1)] [SerializeField] float crouchSpeed = 0.4f;            //Percentage of player max speed applied to crouch.
    [Tooltip("How much movement will be smoothed.")]
    [Range(0, 0.5f)] [SerializeField] float movementSmoothTime = 0.05f; //How much movement will be smoothed.
    [Tooltip("Whether the player can maneuver while in the air.")]
    [SerializeField] bool airControl = false;                           //Whether a player can maneuver while jumping.
    [Tooltip("The platforms/ground layer. This is used when the controller checks if the player is grounded or not.")]
    [SerializeField] LayerMask ground;                                  //The layer of platforms/ground.

    [Header("References")]
    [SerializeField] Transform groundCheck;                             //Position marking where to check for ground.
    [SerializeField] Transform ceilingCheck;                            //Position marking where to check for ceilings.
    [SerializeField] Collider2D crouchDisableCollider;                  //Collider disabled while crouching.

    float groundedRadius = 0.2f;                                        //Radius of overlapCircle that detects whether player is grounded.
    bool isGrounded;                                                    //Whether the player is grounded.
    float ceilingRadius = 0.2f;                                         //Radius of overlapCircle that detects whether there is a ceiling right above the player.
    Rigidbody2D rb;                                                     //The player character's 2D rigidbody.
    bool facingRight = true;                                            //Whether the player is facing right.
    Vector3 velocity = Vector3.zero;

    public class BoolEvent : UnityEvent<bool> { }                       //Structures boolean event type.

    [Header("Events")]
    public UnityEvent OnLandEvent;                                      //Event invoked when the player lands after jumping.
    public UnityEvent OnSprintEvent;                                    //Event invoked when player is sprinting.                                 
    public BoolEvent OnCrouchEvent;                                     //Boolean event invoked when player is crouching.

    bool wasCrouching = false;                                          //Whether the player was previously crouching.

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
        if (OnSprintEvent == null)
            OnSprintEvent = new UnityEvent();
        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        //Draws a circle around the center of the groundCheck transform and detects all objects within.
        //Player is grounded if any of these objects are on the ground layer.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, ground);
        for (int i = 0; i < colliders.Length; i++)
        {
            //Checks if any of the detected colliders are within the player character's collider.
            if (colliders[i].gameObject != gameObject)                
            {
                isGrounded = true;
                Debug.Log("wasGrounded: " + wasGrounded);
                //Invokes OnLandEvent if player was previously in the air.
                if (wasGrounded == false && rb.velocity.y < 0)
                {
                    OnLandEvent.Invoke();
                    Debug.Log("OnLandEvent invoked.");
                }
            }
        }
    }

    /// <summary>
    /// Moves the player.
    /// </summary>
    /// <param name="moveSpeed"></param>
    /// <param name="crouch"></param>
    /// <param name="jump"></param>
    public void Move(float moveSpeed, bool isCrouching, bool isJumping)
    {
        //Check if player can stand up when crouching.
        if (isCrouching == false)
        {
            //If a ceiling is detected that would prevent player from standing up, keep the player crouching.
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, ground))
            {
                isCrouching = true;
            }
        }

        //Player can only be controlled when grounded or if AirControl is enabled.
        if (isGrounded == true || airControl == true)
        {
            if (isCrouching == true)
            {
                if (wasCrouching == false)
                {
                    wasCrouching = true;
                    OnCrouchEvent.Invoke(true);                 //Invoke OnCrouchEvent with value of true if player is crouching.
                }

                moveSpeed *= crouchSpeed;                       //Reduce the speed by the crouchSpeed multiplier.

                //Disable the CrouchDisableModifier, if it exists.
                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = false;
            }
            else
            {
                if (crouchDisableCollider != null)
                    crouchDisableCollider.enabled = true;
                if (wasCrouching == true)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }

            //Calculate player's new velocity.
            Vector3 newVelocity = new Vector2(moveSpeed * 10f, rb.velocity.y);
            //Smooth out the velocity change and apply to the character.
            rb.velocity = Vector3.SmoothDamp(rb.velocity, newVelocity, ref velocity, movementSmoothTime);

            //Flip player when appropriate based on movement direction.
            if (moveSpeed > 0 && facingRight == false)
            {
                FlipPlayer();
            }
            else if (moveSpeed < 0 && facingRight == true)
            {
                FlipPlayer();
            }
        }

        //If player jumps (and player is currently grounded), add a vertical force to the player.
        if (isGrounded == true && isJumping == true)
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }

        //Debug statement to check if player is grounded and is jumping.
        if (Input.GetKeyDown("]"))
        {
            Debug.Log("Player is grounded: " + isGrounded);
            Debug.Log("Player is jumping: " + isJumping);
        }
    }

    /// <summary>
    /// Flips the player sprite.
    /// </summary>
    private void FlipPlayer()
    {
        //Switch the way the player is currently labelled as facing.
        facingRight = !facingRight;

        //Flip the player sprite by multiplying the player's x scale by -1.
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
