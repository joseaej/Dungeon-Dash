using UnityEngine;

public enum EnemyState
{
    Wait,
    Back,
    Follow,
    Attacking,
    Hurting,
    Dead
}

public class EnemyController : MonoBehaviour
{
    public float detectionRadius = 2f;
    public LayerMask playerLayer;
    private Transform playerTransform;
    public EnemyState state = EnemyState.Wait;
    private bool isFacingRight = true;

    [Header("Enemy Options")]
    public int enemyDamage = 1;
    public float speed = 2f;
    public float maxChaseDistance = 4f;
    private Vector3 startPosition;

    [Header("Enemy Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Animations")]
    private Animator animator;
    private PlayerHealth playerHealth;

    void Start()
    {
        startPosition = transform.position;
        currentHealth = maxHealth;
        playerHealth = FindAnyObjectByType<PlayerHealth>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (state == EnemyState.Dead) return;

        UpdateAnimator();
        HandleState();
    }

    private void UpdateAnimator()
    {
        animator.SetBool("isWalking", state == EnemyState.Follow || state == EnemyState.Back);
        animator.SetBool("isAttacking", state == EnemyState.Attacking);
        animator.SetBool("isHurting", state == EnemyState.Hurting);
        animator.SetBool("isDie", state == EnemyState.Dead);
    }

    private void HandleState()
    {
        switch (state)
        {
            case EnemyState.Wait:
                DetectPlayer();
                break;
            case EnemyState.Back:
                MoveToStartPosition();
                break;
            case EnemyState.Follow:
                ChasePlayer();
                break;
            case EnemyState.Attacking:
                AttackPlayer();
                break;
            case EnemyState.Hurting:
                // No se necesita lógica adicional aquí, ya que se maneja en TakeDamage
                break;
        }
    }

    private void DetectPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (playerCollider)
        {
            playerTransform = playerCollider.transform;
            state = EnemyState.Follow;
        }
    }
    

    private void MoveToStartPosition()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        FlipDirection(startPosition);

        if (Vector2.Distance(transform.position, startPosition) < 0.1f)
        {
            state = EnemyState.Wait;
        }
    }

    private void ChasePlayer()
    {
        if (playerTransform == null)
        {
            state = EnemyState.Back;
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        FlipDirection(playerTransform.position);

        if (Vector2.Distance(transform.position, startPosition) > maxChaseDistance ||
            Vector2.Distance(transform.position, playerTransform.position) > maxChaseDistance)
        {
            state = EnemyState.Back;
            playerTransform = null;
        }
        else if (Vector2.Distance(transform.position, playerTransform.position) <= detectionRadius)
        {
            state = EnemyState.Attacking;
        }
    }

    private void AttackPlayer()
    {
        if (playerTransform == null || playerHealth == null)
        {
            state = EnemyState.Back;
            return;
        }

        playerHealth.TakeDamage(enemyDamage);
        state = EnemyState.Follow;
    }

    public void TakeDamage(int damageAmount)
    {
        if (state == EnemyState.Dead) return;

        currentHealth -= damageAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            state = EnemyState.Hurting;
            Invoke("ResetState", 0.5f);
        }
    }
    

    private void ResetState()
    {
        if (state == EnemyState.Hurting)
        {
            state = EnemyState.Follow;
        }
    }

    private void Die()
    {
        state = EnemyState.Dead;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        Destroy(gameObject, 2f);
    }

    private void FlipDirection(Vector3 target)
    {
        if ((target.x > transform.position.x && !isFacingRight) || (target.x < transform.position.x && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            transform.eulerAngles = new Vector3(0, isFacingRight ? 0 : 180, 0);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}