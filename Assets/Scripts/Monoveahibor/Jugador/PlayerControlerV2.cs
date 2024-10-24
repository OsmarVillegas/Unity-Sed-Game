using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlerV2 : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator animator;

    [Header("Movimiento")]
    [SerializeField] private float velocidadDeMovimiento;
    private float movimientoHorizontal = 0f;
    private bool mirandoDerecha;

    private bool enPlataforma;

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
    private bool seRealizaSalto;

    [Header("Esquivando")]
    [SerializeField] private float tiempoEsquive;
    [SerializeField] private float cooldownEsquive;
    [SerializeField] private float fuerzaEsquive;
    private bool esquivando = false;
    private bool puedeEsquivar = true;


    [Header("Ataque")]
    [SerializeField]private float dashingTime;
    private bool atacando;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        escalaGravedadNormal = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento lateral
        movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento * Time.fixedDeltaTime;

        // atacar
        if (Input.GetButtonDown("Fire1"))
        {
            atacando = true;
            StartCoroutine(ataqueDash());
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

        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animator.SetFloat("VelocidadY", rb2d.velocity.y);
        animator.SetBool("enSuelo", enSuelo);
        animator.SetBool("Deslizando", deslizando);
    }

    private void FixedUpdate()
    {
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, Pared);
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSalto, 0f, Suelo);

        // Movimiento
        if (!saltoDePared && !atacando && !esquivando) { 
            rb2d.velocity = new Vector2(movimientoHorizontal, rb2d.velocity.y);
        }

        GirarPersonaje(movimientoHorizontal);

        // salto
        Salto();

        // Deslizar
        Deslizar();

        // Salto desde la pared
        SaltoDesdePared();
    }

    private void SaltoDesdePared()
    {
        if (saltoDeParedVerificadorCondicional && enPared && deslizando)
        {

            espaldaPared = Physics2D.OverlapBox(controladorEspaldaPared.position, dimensionesCajaPared, 0f, Pared);
            if (!espaldaPared)
            {
                seRealizaSalto = true;
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

    private IEnumerator ataqueDash()
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
        seRealizaSalto = false;
    }

    IEnumerator Esquive()
    {
        esquivando = true;
        puedeEsquivar = false;
        animator.SetBool("Esquivando", esquivando);

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

        yield return new WaitForSeconds(cooldownEsquive);
        puedeEsquivar = true;

    }

}
