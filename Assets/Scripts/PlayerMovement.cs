using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    
    private float movement;
    
    public int movementSpeed;
    public int jumpForce;
    public int maxHealth;

    private bool isFacingRight = true;
    private bool isGrounded = true;
    private bool canAttack = true;

    public Text healthValue;

    public Transform attackPoint;
    public float attackRadius;
    public LayerMask attackLayer;
    private CameraShake cameraShake;

    public KeyManager keyM;
    public UiManager uimanager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        cameraShake = FindAnyObjectByType<CameraShake>();
    }

    
    void Update()
    {
        UpdateHealth();
        
        if (maxHealth <= 0)
        {
            Die();
        }
        
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

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            StartCoroutine(TriggerAttack());    
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * movementSpeed;
    }

    private IEnumerator TriggerAttack()
    {
        canAttack = false; 
        ani.SetTrigger("Attacking");
        if (cameraShake != null)
        {
            cameraShake.ShakeCamera();
        }
        yield return new WaitForSeconds(0.4f); 
        canAttack = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
            ani.SetBool("Jumping", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.CompareTag("Key"))
        {
            Destroy(trig.gameObject);
            keyM.keyCount++;
        }
    }

    public void Attack()
    {
        Collider2D collisionInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collisionInfo)
        {
            if (collisionInfo.gameObject.GetComponent<EnemyPatrol>() != null)
            {
                collisionInfo.gameObject.GetComponent<EnemyPatrol>().TakeDamage(1); 
            }
        }
    }

    public void TakeDamage(int damage)
    {
        ani.SetBool("Hurt", true);
        if (maxHealth <= 0)
        {
            return;
        }
        maxHealth -= damage;
        StartCoroutine(ResetHurtAnimation());
    }

    private IEnumerator ResetHurtAnimation()
    { // Wait for the duration of the "Hurt" animation
        yield return new WaitForSeconds(0.3f); // Adjust the duration to match your "Hurt" animation length
        ani.SetBool("Hurt", false); // Reset the "Hurt" animation
    }

    public void UpdateHealth()
    {
        healthValue.text = maxHealth.ToString();
    }
    
    public void Die()
    {
        Debug.Log("Player Died");
        uimanager.GameOver();
        //Animation
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
