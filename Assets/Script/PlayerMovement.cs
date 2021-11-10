using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 4f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    InputActionReference actionReference;
    Vector2 moveInput;
    [SerializeField] bool isAlive = true;
    new Rigidbody2D rigidbody2D;
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    BoxCollider2D boxCollider;
    [SerializeField] float climbSpeed = 4f;
    bool isExit = false;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!isAlive) return;
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                animator.SetBool("isShooting", true);
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                animator.SetBool("isShooting", false);
            }
            Run();
            FlipSprite();
            ClimbLadder();
            Die();
        }
    }
    //Function get move state in Input System
    void OnMove(InputValue value)
    {
        if (!isExit)
            moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (isAlive && !isExit)
        {
            if (boxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                if (value.isPressed)
                {
                    rigidbody2D.velocity += new Vector2(0f, jumpSpeed);
                }
            }
            else if (boxCollider.IsTouchingLayers(LayerMask.GetMask("SupperJump")))
            {
                if (value.isPressed)
                {
                    rigidbody2D.velocity += new Vector2(0f, jumpSpeed * 1.65f);
                }
            }
            else
            {
                return;
            }
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;
        }
        else
        {
            Instantiate(bullet, gun.position, transform.rotation);
        }
    }

    void Run()
    {
        Vector2 playerMove = new Vector2(moveInput.x * speed, rigidbody2D.velocity.y);
        rigidbody2D.velocity = playerMove;
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > 0;

        //Khi player di lui lai, gia tri velocity se < 0, Mathf.Sign se lay duoc dau cua gia tri am la -1, sprite se flip lai
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2D.velocity.x), 1f);
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void Die()
    {
        if (capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"))
            || boxCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"))
            || capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazard"))
            || boxCollider.IsTouchingLayers(LayerMask.GetMask("Hazard")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            rigidbody2D.velocity = new Vector2(0, jumpSpeed / 2);
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void ClimbLadder()
    {
        if (!capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            animator.SetBool("isClimb", false);
            rigidbody2D.gravityScale = 2;
            return;
        }
        else
        {
            bool playHasVerticalSpeed = Mathf.Abs(rigidbody2D.velocity.y) > 0;
            Vector2 playerMove = new Vector2(rigidbody2D.velocity.x, moveInput.y * climbSpeed);
            rigidbody2D.velocity = playerMove;
            rigidbody2D.gravityScale = 0;
            animator.SetBool("isClimb", playHasVerticalSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Exit")
        {
            isExit = true;
        }
    }
}