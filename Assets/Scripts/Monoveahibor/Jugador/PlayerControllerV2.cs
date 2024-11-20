using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerV2 : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;

    [Header("Movimiento")]
    [SerializeField] private float velocidadDeMovimiento;
    private float movimientoHorizontal = 0f;
    private bool mirandoDerecha;
    private bool enPlataforma;

    [SerializeField] private AdministradorDeTutorial administradorTutorial;
    [SerializeField] private bool tutorialFinalizado = false;

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
    private float escalaGravedadNormal;

    [Header("SaltoPared")]
    [SerializeField] private Transform controladorPared;
    [SerializeField] private Transform controladorEspaldaPared;
    [SerializeField] private LayerMask Pared;
    [SerializeField] private Vector3 dimensionesCajaPared;
    private bool enPared;
    private bool deslizando;
    [SerializeField] private float velocidadDeslizar;
    [SerializeField] private float fuerzaSaltoParedX;
    [SerializeField] private float fuerzaSaltoParedY;
    [SerializeField] private float tiempoSaltoPared;
    [Range(0, 1)][SerializeField] private float fuerzaSalto;
    private bool espaldaPared;
    private bool saltoDePared;
    private bool saltoDeParedVerificadorCondicional;

    [Header("Esquivando")]
    [SerializeField] private float tiempoEsquive;
    [SerializeField] private float cooldownEsquive;
    [SerializeField] private float fuerzaEsquive;
    private bool esquivando = false;
    private bool puedeEsquivar = true;

    [Header("Ataque")]
    [SerializeField] private float dashingTime;
    private bool atacando;


    [Header("Vida")]
    [SerializeField] public bool estaVivo;
    private float sePuedeRebotar;
    [SerializeField] private Vector2 velocidadRebote;

    [SerializeField] private EjecutarCinematica ejecutarCinematica;

    [Header("Sonido")]

    [SerializeField] private AudioClip saltoSonido;

    [SerializeField] private AudioClip esquiveSonido;

    [SerializeField] private AudioClip caminandoSonido;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        estaVivo = true;

        escalaGravedadNormal = rb2d.gravityScale;

        administradorTutorial.TutorialSaltado += ActivarMovimiento;
        ejecutarCinematica.detenerJugador += DesactivarMovimiento;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialFinalizado)
        {
            if (estaVivo)
            {
                // Movimiento lateral
                movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento * Time.fixedDeltaTime;

                // atacar
                if (Input.GetButtonDown("Fire1"))
                {
                    atacando = true;
                    StartCoroutine(AtaqueDash());
                }

                // Saltar
                if (Input.GetButtonDown("Jump"))
                {
                    if (enSuelo)
                    {
                        salto = true;
                    }

                    if (enPared)
                    {
                        saltoDeParedVerificadorCondicional = true;
                    }
                }
                if (Input.GetButtonUp("Jump"))
                {
                    SaltoVariable();
                }

                // Deslizar
                if (!enSuelo && enPared)
                {
                    deslizando = true;
                }
                else
                {
                    deslizando = false;
                }

                // Esquivar
                if (Input.GetButtonDown("Esquivar") && puedeEsquivar && enSuelo && !enPlataforma)
                {
                    Collider2D colisionador = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSalto, 0f, Suelo);

                    if (!colisionador.CompareTag("Plataforma"))
                    {
                        StartCoroutine(Esquive());
                    }
                }
            }

            animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
            animator.SetFloat("VelocidadY", rb2d.velocity.y);
            animator.SetBool("Deslizando", deslizando);
        }

        animator.SetBool("enSuelo", enSuelo);

    }

    private void FixedUpdate()
    {
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, Pared);
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSalto, 0f, Suelo);

        // Movimiento
        if (!saltoDePared && !atacando && !esquivando && estaVivo)
        {
            rb2d.velocity = new Vector2(movimientoHorizontal, rb2d.velocity.y);
        }

        if (!estaVivo)
        {
            rb2d.velocity = new Vector2(0, 0);
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("VelocidadY", 0);
        }

        GirarPersonaje(movimientoHorizontal);

        // salto
        Salto();

        // Deslizar
        Deslizar();

        // Salto desde la pared
        SaltoDesdePared();
    }

    public void Rebote(Vector2 puntoGolpe)
    {
        rb2d.velocity = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }

    private void ActivarMovimiento()
    {
        tutorialFinalizado = true;
    }
    private void DesactivarMovimiento()
    {
        tutorialFinalizado = false;
        movimientoHorizontal = 0;
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
    }

    private void SaltoDesdePared()
    {
        if (saltoDeParedVerificadorCondicional && enPared && deslizando)
        {

            espaldaPared = Physics2D.OverlapBox(controladorEspaldaPared.position, dimensionesCajaPared, 0f, Pared);
            if (!espaldaPared)
            {
                SaltoPared();
            }
            else
            {
                saltoDeParedVerificadorCondicional = false;
            }

        }
    }

    private void Salto()
    {
        if (salto && enSuelo)
        {
            ControladorSonido.instance.EjecutarSonido(saltoSonido);
            rb2d.AddForce(new Vector2(0f, fuerzaDeSalto));
            salto = false;
        }

        if (rb2d.velocity.y < 0 && !enSuelo)
        {
            rb2d.gravityScale = escalaGravedadNormal * multiplicadorDeGravedad;
        }
        else
        {
            rb2d.gravityScale = escalaGravedadNormal;
        }
    }

    private void SaltoVariable()
    {
        if (rb2d.velocity.y > 0)
        {
            rb2d.AddForce(Vector2.down * rb2d.velocity.y * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);
        }

        salto = false;
    }

    private IEnumerator AtaqueDash()
    {
        yield return new WaitForSeconds(dashingTime);
        atacando = false;
    }

    void GirarPersonaje(float movimientoLateral)
    {
        if (rb2d.velocity.x > 0 && mirandoDerecha)
        {
            Girar();
        }
        else if (rb2d.velocity.x < 0 && !mirandoDerecha)
        {
            Girar();
        }
    }

    void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void Deslizar()
    {
        if (deslizando)
        {
            rb2d.velocity = new Vector3(rb2d.velocity.x, Mathf.Clamp(rb2d.velocity.y, -velocidadDeslizar, float.MaxValue));
        }
    }
    private void SaltoPared()
    {
        // Se resetea el valor de salto para volver a saltar desde la pared
        saltoDePared = false;
        saltoDeParedVerificadorCondicional = false;

        if (!mirandoDerecha && deslizando)
        {

            rb2d.velocity = Vector2.zero;
            rb2d.velocity = new Vector2(fuerzaSaltoParedX * -fuerzaSalto, fuerzaSaltoParedY);
        }
        else if (mirandoDerecha && deslizando)
        {

            rb2d.velocity = Vector2.zero;
            rb2d.velocity = new Vector2(fuerzaSaltoParedX * fuerzaSalto, fuerzaSaltoParedY);
        }

        StartCoroutine(CambioSaltoPared());
    }
    IEnumerator CambioSaltoPared()
    {
        saltoDePared = true;
        yield return new WaitForSeconds(tiempoSaltoPared);
        saltoDePared = false;
    }

    IEnumerator Esquive()
    {
        esquivando = true;
        puedeEsquivar = false;
        animator.SetBool("Esquivando", esquivando);
        ControladorSonido.instance.EjecutarSonido(esquiveSonido);
        Physics2D.IgnoreLayerCollision(9, 10, true);

        if (!mirandoDerecha)
        {
            rb2d.velocity = new Vector2(fuerzaEsquive, rb2d.velocity.y);
        }
        else if (mirandoDerecha)
        {
            rb2d.velocity = new Vector2(-fuerzaEsquive, rb2d.velocity.y);
        }

        yield return new WaitForSeconds(tiempoEsquive);

        esquivando = false;
        animator.SetBool("Esquivando", esquivando);
        Physics2D.IgnoreLayerCollision(9, 10, false);

        yield return new WaitForSeconds(cooldownEsquive);
        puedeEsquivar = true;

    }

}
