using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int currentHealth;

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityTime = 1f;
    private bool isInvincible = false;

    [Header("Events")]
    public UnityEvent onDamageTaken;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damageAmount)
    {
        if (isInvincible) return;

        currentHealth -= damageAmount;
        Debug.Log($"Player took {damageAmount} damage! Current health: {currentHealth}");

        onDamageTaken?.Invoke();

        StartCoroutine(InvincibilityCoroutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        Debug.Log($"Player healed {healAmount} points! Current health: {currentHealth}");
    }


    private void Die()
    {
        SceneManager.LoadScene("Platform");
        Destroy(gameObject);
    }


    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }
}