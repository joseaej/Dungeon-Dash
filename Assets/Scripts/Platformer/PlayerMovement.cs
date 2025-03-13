using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rg;
    private bool mirarDerecha = true; 
    private bool isGrounded = true;
    private bool jumping = false;
    private int saltosRestantes = 2; 
    public float rayCast = 0.1f;

    Animator animator;
    [Header("Movimiento Personaje")]
    [SerializeField] private float velocidad = 1.5f;
    [SerializeField] private float velocidadCorriendo = 2.5f;

    [Header("Salto Personaje")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Vector2 caja;
    [SerializeField] private Transform suelo;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    void Update()
    {
        CheckGrounded();
        HandleJumpInput();
        Movement();
        HandleFall();
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapBox(suelo.position, caja, 0, ground);

        if (isGrounded)
        {
            saltosRestantes = 2; 
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump") && saltosRestantes > 0)
        {
            jumping = true;
        }
    }

    void Movement()
    {
        float input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                rg.linearVelocity = new Vector2(input * velocidadCorriendo, rg.linearVelocity.y);
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
                rg.linearVelocity = new Vector2(input * velocidad, rg.linearVelocity.y);
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", false);
        }

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

    void HandleFall()
    {
        if (!isGrounded && rg.linearVelocity.y < 0)
        {
            animator.SetBool("isFalling", true);
            animator.SetBool("isJumping", false);
        }
    }

    void FixedUpdate()
    {
        if (jumping)
        {
            Jump();
            jumping = false;
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rg.linearVelocity = new Vector2(rg.linearVelocity.x, 0);
            rg.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            saltosRestantes--;
            animator.SetBool("isJumping", true);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(suelo.position, caja);
    }
}