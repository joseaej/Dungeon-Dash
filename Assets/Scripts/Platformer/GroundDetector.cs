using UnityEngine;
using UnityEngine.SceneManagement;

public class GroundDetector : MonoBehaviour
{
    public bool IsGrounded => _IsGrounded;

    bool _IsGrounded = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeadZone"))
        {
            Die();
        }
        Debug.Log("Contacto con el suelo");
        _IsGrounded = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Pérdida de contacto con el suelo");
        _IsGrounded = false;
    }
    private void Die()
    {
        SceneManager.LoadScene("Platform");
        Destroy(gameObject); 
    }
}
