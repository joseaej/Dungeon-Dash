using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rg;
    private bool mirarDerecha = true;

    [Header("Parametros de movimiento")]
    [SerializeField] private float velocidadMovimiento = 10f;
    [SerializeField] private float suavizado = 0.05f;
    [SerializeField] private float fuerzaSalto = 12f;

    [Header("Detección de suelo")]
    [SerializeField] private Transform checkSuelo;
    [SerializeField] private float radioCheckSuelo = 0.2f;
    [SerializeField] private LayerMask capaSuelo;

    private float movHorizontal;
    private bool enSuelo = true;
    private Vector3 velocidad = Vector3.zero;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Obtener la entrada horizontal del jugador
        movHorizontal = Input.GetAxisRaw("Horizontal") * velocidadMovimiento;

        // Detectar si el jugador está en el suelo
        enSuelo = Physics2D.OverlapCircle(checkSuelo.position, radioCheckSuelo, capaSuelo);

        // Permitir salto solo si está en el suelo
        if (enSuelo && Input.GetButtonDown("Jump"))
        {
            Saltar();
        }
    }

    void FixedUpdate()
    {
        // Mover al jugador
        Move(movHorizontal * Time.fixedDeltaTime);

        // Invertir la dirección del sprite si es necesario
        if ((movHorizontal > 0 && !mirarDerecha) || (movHorizontal < 0 && mirarDerecha))
        {
            InvertirDireccion();
        }
    }

    private void Move(float mover)
    {
        // Calcular la velocidad objetivo
        Vector3 velocidadObjetivo = new Vector2(mover * 10f, rg.linearVelocity.y);

        // Suavizar el movimiento
        rg.linearVelocity = Vector3.SmoothDamp(rg.linearVelocity, velocidadObjetivo, ref velocidad, suavizado);
    }

    private void Saltar()
    {
        enSuelo = true;
        rg.linearVelocity = new Vector2(rg.linearVelocity.x, 0f);
        rg.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        enSuelo = false;
    }

    private void InvertirDireccion()
    {
        // Cambiar la dirección del sprite
        mirarDerecha = !mirarDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmosSelected()
    {
        // Dibujar el área de detección de suelo en el editor
        if (checkSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(checkSuelo.position, radioCheckSuelo);
        }
    }
}