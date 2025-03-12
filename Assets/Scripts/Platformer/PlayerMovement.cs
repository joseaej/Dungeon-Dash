using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rg;
    private bool mirarDerecha = true; 
    private bool isGrounded = true;
    private bool jumping = false;
    private int saltosRestantes = 2; 
    public float rayCast = 0.1f;

    [Header("Movimiento Personaje")]
    [SerializeField] private float velocidad = 5f;

    [Header("Salto Personaje")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Vector2 caja;
    [SerializeField] private Transform suelo;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    void Update()
    {
        
        if (Input.GetButtonDown("Jump") && saltosRestantes > 0)
        {
            jumping = true;
        }

        
        isGrounded = Physics2D.OverlapBox(suelo.position, caja, 0, ground);

        
        if (isGrounded)
        {
            saltosRestantes = 2; 
        }

        
        Movement();
    }

    void FixedUpdate()
    {
        
        if (jumping)
        {
            Jump();
            jumping = false; 
        }
    }

    void Movement()
    {
        float input = Input.GetAxis("Horizontal");
        rg.linearVelocity = new Vector2(input * velocidad, rg.linearVelocity.y);
        ChangeOrientation(input);
    }

    void ChangeOrientation(float input)
    {
        if ((mirarDerecha && input < 0) || (!mirarDerecha && input > 0))
        {
            mirarDerecha = !mirarDerecha;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
    }

    void Jump()
    {
        
        rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        saltosRestantes--; 
        isGrounded = false; 
    }

    void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(suelo.position, caja);
    }
}