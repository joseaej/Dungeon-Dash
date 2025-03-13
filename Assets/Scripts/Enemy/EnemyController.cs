using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f; 
    [SerializeField] private float raycastDistance = 0.5f; 
    [SerializeField] private LayerMask groundLayer; 

    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 1; 
    [SerializeField] private float knockbackForce = 5f; 

    private bool movingRight = true; 
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        CheckForEdges();
    }

    private void Move()
    {
        
        float direction = movingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    private void CheckForEdges()
    {
        
        Vector2 rayOrigin = movingRight ? 
            new Vector2(transform.position.x + 0.5f, transform.position.y) : 
            new Vector2(transform.position.x - 0.5f, transform.position.y);

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, raycastDistance, groundLayer);

        
        if (!hit.collider)
        {
            movingRight = !movingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);

                
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Vector2 rayOrigin = movingRight ? 
            new Vector2(transform.position.x + 0.5f, transform.position.y) : 
            new Vector2(transform.position.x - 0.5f, transform.position.y);
        Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.down * raycastDistance);
    }
}