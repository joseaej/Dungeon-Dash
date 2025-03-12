using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [Header("Referencias")]
    public Image fillBar; // Referencia a la imagen de la barra de vida
    public PlayerController playerController; // Referencia al script PlayerController

    private float maxHealth; // Vida máxima del jugador

    void Start()
    {
        // Verificar si la referencia al PlayerController es válida
        if (playerController == null)
        {
            Debug.LogError("PlayerController no está asignado en el Inspector.");
            return;
        }

        // Inicializar la vida máxima
        maxHealth = playerController.health;

        // Verificar que maxHealth no sea 0 para evitar divisiones por cero
        if (maxHealth <= 0)
        {
            Debug.LogError("La vida máxima del jugador debe ser mayor que 0.");
            maxHealth = 1; // Asignar un valor por defecto para evitar errores
        }

        // Actualizar la barra de vida al inicio
        UpdateHealthBar();
    }

    void Update()
    {
        // Actualizar la barra de vida en cada frame
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // Calcular el porcentaje de vida actual
        float healthPercentage = playerController.health / maxHealth;

        // Asegurarse de que el valor esté entre 0 y 1
        healthPercentage = Mathf.Clamp01(healthPercentage);

        // Actualizar el fillAmount de la barra de vida
        fillBar.fillAmount = healthPercentage;
    }
}