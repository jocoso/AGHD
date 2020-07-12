using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float movementInputDirection;
    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private int amountJumpsLeft;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    public int amountOfJumps = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent< Rigidbody2D >();
        animator = GetComponent< Animator >();
        amountJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate() {
        ApplyMovement();
        CheckSurroundings();
    }

    private void CheckSurroundings() {
        isGrounded = Physics2D.OverlapCircle( groundCheck.position, groundCheckRadius, whatIsGround );
    }

    private void CheckIfCanJump() {
        if( isGrounded && rb.velocity.y <= 0 )
            amountJumpsLeft = amountOfJumps;

        canJump = amountJumpsLeft > 0;
    }

    private void CheckMovementDirection() {
        if( isFacingRight && movementInputDirection < 0 ) {
            Flip();
        } else if( !isFacingRight && movementInputDirection > 0 ) {
            Flip();
        }

        if( rb.velocity.x != 0 ) {
            isWalking = true;
        } else {
            isWalking = false;
        }
    }

    private void UpdateAnimations() {
        if( isFacingRight && isWalking )
            animator.SetInteger( "walkingDirection", -1 );
        else if ( !isFacingRight && isWalking )
            animator.SetInteger( "walkingDirection", 1 );
        else 
            animator.SetInteger( "walkingDirection", 0 );
    }

    private void Flip() {
        isFacingRight = !isFacingRight;
    }

    private void CheckInput() {
        movementInputDirection = Input.GetAxisRaw( "Horizontal" );
        if( Input.GetButtonDown("Jump") ) {
           Jump();
        }
    }

    private void Jump() {
        if( canJump ) {
            rb.velocity = new Vector2( rb.velocity.x, jumpForce );
            amountJumpsLeft--;
        }
    }

    private void ApplyMovement() {
        rb.velocity = new Vector2( movementInputDirection * movementSpeed, rb.velocity.y );
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere( groundCheck.position, groundCheckRadius );
    }
}
