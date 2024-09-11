using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [Header("Movimiento")]

    private float movimientoHoritontal = 0f;

    [SerializeField] private float velocidadDeMovimiento;

    [Range(0, 0.3f)][SerializeField] private float suavisadoDeMovimiento;

    private Vector3 velocidad = Vector3.zero;

    private bool mirandoDerecha = true;

    [Header("Salto")]

    [SerializeField] private float fuerzaDeSalto;

    [SerializeField] private LayerMask Suelo;

    [SerializeField] private Transform controladorSuelo;

    [SerializeField] private Vector3 dimensionesCajaSalto;

    private bool enSuelo;

    private bool salto;

    [Header("SaltoRegulable")]

    [Range(0, 1)][SerializeField] private float multiplicadorCancelarSalto;

    [SerializeField] private float multiplicadorDeGravedad;

    private float escalaGravedad;

    private bool botonSaltoArriba = true;

    [Header("SaltoPared")]

    [SerializeField] private Transform controladorPared;

    [SerializeField] private LayerMask Pared;

    [SerializeField] private Vector3 dimensionesCajaPared;

    private bool enPared;

    private bool deslizando;

    [SerializeField] private float velocidadDeslizar;

    [SerializeField] private float fuerzaSaltoParedX;

    [SerializeField] private float fuerzaSaltoParedY;

    [SerializeField] private float tiempoSaltoPared;

    [Range(0, 1)] [SerializeField] private float fuerzaSalto;

    private bool saltoDePared;

    // Para evitar errores en el salto se genero esta segunda variable.
    private bool saltarPared;

    bool saltoContinuo;

    [Header("Animacion")]

    private Animator animator;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        escalaGravedad = rb2d.gravityScale;
    }
    void Update()
    {



        movimientoHoritontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;

        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHoritontal));

        animator.SetFloat("VelocidadY", rb2d.velocity.y);

        animator.SetBool("Deslizando", deslizando);

        if (Input.GetButtonDown("Jump"))
        {
            if (enSuelo) {
                salto = true;
            }
            
            if (enPared)
            {
                saltarPared = true;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            BotonSaltoArriba();
        }

        if (!enSuelo && enPared && movimientoHoritontal != 0)
        {
            deslizando = true;
        }
        else
        {
            deslizando = false;
        }

    }

    private void FixedUpdate()
    {
        
        animator.SetBool("enSuelo", enSuelo);
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSalto, 0f, Suelo);
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, Pared);

        // Movimiento
        Mover(movimientoHoritontal * Time.fixedDeltaTime, salto);

        if (deslizando)
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -velocidadDeslizar, float.MaxValue));
        }
    }

    private bool spriteSaltoPared;
    private float valor;
    private bool saltoParedUnaVez;

    private void Mover(float mover, bool salto)
    {
        if (!saltoDePared) {
            Vector3 velocidadObjetivo = new Vector2(mover, rb2d.velocity.y);
            rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, velocidadObjetivo, ref velocidad, suavisadoDeMovimiento);
        }

        if (enSuelo)
        {
            saltoParedUnaVez = false;
        }

        if (spriteSaltoPared)
        {
            mover *= -1;
            valor = mover;

            if (valor > 0 && !mirandoDerecha && !saltoParedUnaVez)
            {
                // Girar
                Girar();
                saltoParedUnaVez = true;
            }

            else if (valor < 0 && mirandoDerecha && !saltoParedUnaVez)
            {
                // Girar
                Girar();
                saltoParedUnaVez = true;
            }
        }

        if (spriteSaltoPared)
        {
            mover = 0;
        }

        print(mover);
        if (mover > 0 && !mirandoDerecha && !spriteSaltoPared)
        {
            // girar
            Girar();

        }
        else if (mover < 0 && mirandoDerecha && !spriteSaltoPared)
        {
            // girar
            Girar();

        }

        // Salto
        if (salto && enSuelo && botonSaltoArriba && !deslizando)
        {
            Salto();
        }

        if (rb2d.velocity.y < 0 && !enSuelo)
        {
            rb2d.gravityScale = escalaGravedad * multiplicadorDeGravedad;
        }
        else
        {
            rb2d.gravityScale = escalaGravedad;
        }

        // SaltoPared
        if (saltarPared && enPared && deslizando)
        {
            spriteSaltoPared = true;
            SaltoPared();

        }

    }

    private void Salto()
    {
        rb2d.gravityScale = 0;
        rb2d.AddForce(new Vector2(0f, fuerzaDeSalto));
        rb2d.gravityScale = escalaGravedad;
        enSuelo = false;
        salto = false;
        botonSaltoArriba = false;
        saltarPared = false;
    }

    private void BotonSaltoArriba()
    {
        if (rb2d.velocity.y > 0)
        {
            rb2d.AddForce(Vector2.down * rb2d.velocity.y * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);
        }

        botonSaltoArriba = true;
        salto = false;
    }

    private void SaltoPared()
    {
        saltarPared = false;
        enPared = false;


        if (movimientoHoritontal < 0)
        {
            rb2d.velocity = new Vector2(fuerzaSaltoParedX * fuerzaSalto, fuerzaSaltoParedY);
        }
        else if (movimientoHoritontal > 0)
        {   
            rb2d.velocity = new Vector2(fuerzaSaltoParedX * -fuerzaSalto, fuerzaSaltoParedY);
        }

        // Esperar
        StartCoroutine(CambioSaltoPared());

    }

    IEnumerator CambioSaltoPared()
    {
        saltoDePared = true;

        yield return new WaitForSeconds(tiempoSaltoPared);

        saltoDePared = false;
        spriteSaltoPared = false;
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCajaSalto);
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
    }
}
