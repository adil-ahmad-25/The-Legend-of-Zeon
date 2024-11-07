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
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
    }
}
