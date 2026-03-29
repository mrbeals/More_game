using System.Data;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    // Leftright speed
    public float moveSpeed = 5f;
    // force of the jump
    public float jumpForce = 1.5f;

    // player input
    public float horizontal;

    // checks if on the ground
    public bool canJump;
    //for the entity under the player
    public Transform groundCheck;
    // uses the ground 
    public LayerMask groundLayer;


    // Runs on initialization
    private Animator animator;
    // reference to the renderer to allow for flipping
    private SpriteRenderer sr;
    // Brings in the txt element from the canvas
    public GameObject Victorytxt;

    void Start()
    {
        // stores the components into the attributes
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Runs every frame
    void Update()
    {
        
        horizontal = 0.0f;

        if (Keyboard.current.aKey.isPressed)
            {
                horizontal = -1.0f;
            }
            else if (Keyboard.current.dKey.isPressed)
            {
                horizontal = 1.0f;
            }   

        canJump = Physics2D.OverlapCircle(groundCheck.position, .15f, groundLayer);

        if (horizontal >= 0)
        {
            // defaults and faces right when moving right
            sr.flipX = false;
        }
        else if (horizontal < 0)
        {
            // Switches left when moving negative
            sr.flipX = true;
        }


        if (Keyboard.current.spaceKey.wasPressedThisFrame && canJump)
        {   
            // keeps x movement and replaces the y with our value for force
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        SetAnimation();

    }
    void FixedUpdate()
    {
        // changes x with input, changes y with jump
        rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);
        
    }
    // When reach grave this will trigger
    void OnTriggerEnter2D(Collider2D win)
    {
        if (win.CompareTag("Victory"))
        {
            Victorytxt.SetActive(true);
        }
    }

    // Checks movements and apply's correct animation
    private void SetAnimation()
    {
        if (canJump)
        {
            if (horizontal != 0)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Ghost_run_ani")) {
                animator.Play("ghost_run_ani");  }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Ghost_idle_ani")) {
                animator.Play("ghost_idle_ani");  }
            }
        }
    
        else {
            if (rb.linearVelocity.y > 0)
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Ghost_jump_ani")) {
                animator.Play("ghost_jump_ani");  }
            }
            else
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Ghost_fall_ani")) {
                animator.Play("ghost_fall_ani");  }
            }
        }
    }
}
