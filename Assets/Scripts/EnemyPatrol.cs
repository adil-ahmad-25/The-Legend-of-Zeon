using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    private bool facingLeft = true;
    public float moveSpeed;
    public Transform checkPoint;
    public float distance = 1f;
    public LayerMask layerMask;
    private bool playerInRange = false;
    public Transform playerPosition;
    public float attackRange;
    public float retrieveDistance;
    private Animator ani;
    
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPosition.position) <= attackRange)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if (playerInRange)
        {
            if (Vector2.Distance(transform.position, playerPosition.position) > retrieveDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, playerPosition.position, moveSpeed * Time.deltaTime);  
            }
            else
            {
                ani.SetBool("Attack", true);
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        RaycastHit2D rayHit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

        if (rayHit == false && facingLeft)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            facingLeft = false;
        }
        else if (rayHit == false && facingLeft == false)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingLeft = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (checkPoint == null)
        {
            return;
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
