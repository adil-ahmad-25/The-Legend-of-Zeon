using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    // Added patrol points to move between
    public Transform[] patrolPoints;
    public int patrolDestination;

    private bool facingLeft = true;
    public float moveSpeed;

    public Transform playerPosition;
    public float attackRange;
    public float retrieveDistance;
    private Animator ani;

    public Transform attackPoint;
    public float attackRadius;
    public LayerMask attackLayer;

    public int maxHealth;
    private bool isAttacking = false;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    { 
        if (maxHealth <= 0)
        {
            Die();
        }

        if (Vector2.Distance(transform.position, playerPosition.position) <= attackRange)
        {
            HandleAttack();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        ani.SetTrigger("Walk");
        if (patrolDestination == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, patrolPoints[0].position) < 0.5f)
            {
                patrolDestination = 1;
                Flip();
            }
        }

        if (patrolDestination == 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, patrolPoints[1].position) < 0.5f)
            {
                patrolDestination = 0;
                Flip();
            }
        }
    }



    // New method to flip the enemy's direction
    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    void HandleAttack()
    {
        // Update facing direction based on player position
        if (playerPosition.position.x < transform.position.x && !facingLeft)
        {
            Flip();
        }
        else if (playerPosition.position.x > transform.position.x && facingLeft)
        {
            Flip();
        }

        // Start attack coroutine if not already attacking
        if (!isAttacking)
        {
            if (Vector2.Distance(transform.position, playerPosition.position) > retrieveDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, moveSpeed * Time.deltaTime);
                
            }
            else
            {
                
                StartCoroutine(AttackRoutine());
            }
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        ani.SetBool("Attack", true);
        ani.SetBool("Idle", false);

        // Play attack animation
        yield return new WaitForSeconds(0.5f); // Adjust this duration to match the length of your attack animation

        // Perform the attack and then wait
        Attack();
        ani.SetBool("Attack", false);
        ani.SetBool("Idle", true);

        yield return new WaitForSeconds(2f);

        isAttacking = false;
    }


    public void Attack()
    {
        Collider2D collisionInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collisionInfo?.GetComponent<PlayerMovement>() != null)
        {
            collisionInfo.GetComponent<PlayerMovement>().TakeDamage(1);
        }
    }

    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return;

        maxHealth -= damage;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    // Updated Gizmos to draw patrol path between patrol points
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, retrieveDistance);

        if (attackPoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
