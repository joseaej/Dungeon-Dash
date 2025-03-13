using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float health = 3f;
    public float points = 0;
    public Animator animator;

    [Header("Attack Settings")]
    public float attackRange = 1f; // Rango del ataque
    public LayerMask enemyLayer; // Capa del enemigo
    public int attackDamage = 1; // Daño del ataque

    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detectar si el jugador presiona el botón de ataque (por ejemplo, la tecla "X")
        if (Input.GetKeyDown(KeyCode.X) && !isAttacking)
        {
            Attack();
        }
    }

    private void Attack()
    {
        // Activar la animación de ataque
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        // Detectar enemigos en el rango de ataque
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        // Aplicar daño a todos los enemigos dentro del rango
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemyController>().TakeDamage(attackDamage);
        }

        // Reiniciar el estado de ataque después de un breve tiempo
        StartCoroutine(ResetAttackAfterDelay(0.5f)); // Ajusta el tiempo según la duración de la animación
    }

    private IEnumerator ResetAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Desactivar la animación de ataque
        isAttacking = false;
        animator.SetBool("isAttacking", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            points++;
            Debug.Log("Points: " + points);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1f);
        }
    }

    private void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Die();
        }

        Debug.Log("Player health: " + health);
    }

    private void Die()
    {
        SceneManager.LoadScene("Platform");
        Destroy(gameObject);
    }

    // Dibujar el rango de ataque en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}