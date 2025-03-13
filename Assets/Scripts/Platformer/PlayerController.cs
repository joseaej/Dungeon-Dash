using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float health = 3f; 
    public float points =0 ;
    public Animator animator;
    void Start(){
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            points++;
            Debug.Log("" + points);
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
}