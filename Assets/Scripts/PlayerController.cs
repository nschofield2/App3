using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10;
    public float jumpPower = 20;
    const float groundCheckRadius = 0.2f;
    const float wallJumpCheckRadius = 1.05f;
    public LayerMask groundLayer;
    public LayerMask wallJumpLayer;

    bool flipped;
    bool jumping;
    Animator m_Animator;
    Rigidbody2D m_Player;
    public Transform groundCheckCollider;
    public Transform wallJumpCheckCollider;
    Vector2 horizontalInput;

    public float jumpTimer;

    public bool isGrounded = true;
    public bool isOnWall = true;

    void Start()
    {
        m_Player = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        horizontalInput = new Vector2(Input.GetAxis("Horizontal"), 0).normalized;

        if(horizontalInput.x > 0)
        {
            flipped = true;
        }
        else if(horizontalInput.x < 0)
        {
            flipped = false;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jumping = false;
        }

        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
    }

    void FixedUpdate()
    {
        GroundCheck();
        WallJumpCheck();
        Move(horizontalInput, jumping);
        jumpTimer += Time.deltaTime;
        if(horizontalInput.x != 0) {
            m_Animator.SetBool("isWalking", true);
        }
        else {
            m_Animator.SetBool("isWalking", false);
        }
    }

    void GroundCheck()
    {
        //isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if(colliders.Length > 0) {
            isGrounded = true;
        }
    }

    void WallJumpCheck()
    {
        isOnWall = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallJumpCheckCollider.position, wallJumpCheckRadius, wallJumpLayer);
        if(colliders.Length > 0) {
            isOnWall = true;
        }
    }

    void Move(Vector2 direction, bool jumpFlag)
    {
        if(isGrounded && jumpFlag)
        {
            isGrounded = false;
            jumpFlag = false;
            m_Player.AddForce(new Vector2(0f, jumpPower));
        }
        else if (isOnWall && jumpFlag && jumpTimer > 0.5)
        {
            isOnWall = false;
            jumpFlag = false;
            m_Player.AddForce(new Vector2(0f, (jumpPower*1.65f)));
            jumpTimer = 0;
        }

        var xVal = direction.x * (speed * 3) * Time.deltaTime;
        this.transform.Translate(new Vector3(xVal, 0), Space.World);
    }
}
