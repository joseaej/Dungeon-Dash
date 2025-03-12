using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float health = 3f; // Vida inicial del jugador

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si el objeto que colisiona tiene la etiqueta "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1f); // El jugador recibe 1 de daño
        }
    }

    private void TakeDamage(float damageAmount)
    {
        health -= damageAmount; // Reducir la vida del jugador

        // Verificar si la vida del jugador llegó a 0 o menos
        if (health <= 0)
        {
            Die(); // Llamar a la función de muerte
        }

        Debug.Log("Player health: " + health); // Mostrar la vida actual en la consola
    }

    private void Die()
    {
        Debug.Log("Player has died!"); // Mensaje de muerte en la consola
        Destroy(gameObject); // Destruir el objeto del jugador
    }
}