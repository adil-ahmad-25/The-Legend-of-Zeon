using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    
    private float movement;
    public int movementSpeed;
    public int jumpForce;

    private bool isFacingRight = true;
    private bool isGrounded = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        if (Mathf.Abs(movement) > 0.1f)
        {
            ani.SetFloat("Running", 1f);
        }
        else if (movement < 0.1f)
        {
            ani.SetFloat("Running", 0f);
        }

        FlipPlayer();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            {
                Jump();
                isGrounded = false;
                ani.SetBool("Jumping", true);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            AttackOne();
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * movementSpeed;
    }

    void FlipPlayer()
    {
        if (movement < 0f && isFacingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            isFacingRight = false;
        }
        else if(movement > 0f && isFacingRight == false)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            isFacingRight = true;
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    void AttackOne()
    {
        ani.SetTrigger("Attacking");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            ani.SetBool("Jumping", false);
        }
    }
}
