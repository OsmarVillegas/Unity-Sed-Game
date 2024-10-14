using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class PlayerController_2 : MonoBehaviour
{
    private Rigidbody2D rb2d;

    [Header("Movimiento")]

    private float movimientoHorizontal = 0f;

    [SerializeField] private float velocidadDeMovimiento;

    [Range(0, 0.3f)][SerializeField] private float suavisadoDeMovimiento;

    private Vector3 velocidad = Vector3.zero;

    private bool mirandoDerecha;

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

    private float ultimoValorPared;

    [Header("Animacion")]

    private Animator animator;

    [Header("Esquivar")]

    [SerializeField] private float distanciaEsquive;

    [SerializeField] private float cooldownEsquive;

    private bool esquivar = false;

    [Header("Collider")]

    private PolygonCollider2D polygonCollider;

    private Vector2[] originalPoints;

    private Vector2[] agacharsePoints;

    [Header("Ataque")]

    private bool estaAtacando;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        escalaGravedadNormal = rb2d.gravityScale;

        // Obtén la referencia al PolygonCollider2D
        polygonCollider = GetComponent<PolygonCollider2D>();

        // Guarda los puntos originales del collider
        originalPoints = polygonCollider.points;



        for (int i = 0; i < originalPoints.Length; i++)
        { 
            print(originalPoints[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movimiento

        if (!seRealizaSalto && !esquivar) { 
            movimientoHorizontal = Input.GetAxisRaw("Horizontal") * velocidadDeMovimiento;
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
            BotonSaltoArriba();
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

        if (Input.GetButtonDown("Esquivar"))
        {
            if (enSuelo) { 
                esquivar = true;
                animator.SetBool("Esquivando", esquivar);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            estaAtacando = true;
        }

        if (Input.GetButtonUp("Fire1")){
            estaAtacando = false;
        }

        // Animator
        animator.SetFloat("Horizontal", Mathf.Abs(movimientoHorizontal));
        animator.SetFloat("VelocidadY", rb2d.velocity.y);
        animator.SetBool("Deslizando", deslizando);
        animator.SetBool("Esquivando", esquivar);
    }

    void FixedUpdate()
    {
        enPared = Physics2D.OverlapBox(controladorPared.position, dimensionesCajaPared, 0f, Pared);
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCajaSalto, 0f, Suelo);

        animator.SetBool("enSuelo", enSuelo);

        // Movimiento
        MovimientoLateral(movimientoHorizontal * Time.fixedDeltaTime);

        // Salto
        Salto();

        // Deslizar
        Deslizar();

        // Esquive
        Esquivar();

        // Salto desde la pared
        // saltoDeParedVerificadorCondicional
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

    void Esquivar()
    {
        if (enSuelo && !enPared && esquivar)
        {
            if (!mirandoDerecha)
            {
                rb2d.velocity = new Vector2(distanciaEsquive, rb2d.velocity.y);
            }
            else if (mirandoDerecha)
            {
                rb2d.velocity = new Vector2(-distanciaEsquive, rb2d.velocity.y);
            }

            StartCoroutine(CooldownEsquive());
        }
    }

    void MovimientoLateral(float velocidadLateral)
    {
        if (!saltoDePared && !estaAtacando)
        {
            rb2d.velocity = new Vector2(velocidadLateral, rb2d.velocity.y);
        }
        
        GirarPersonaje(velocidadLateral);
    }

    void Salto()
    {
        // Salto
        if (salto && enSuelo)
        {
            Saltar();
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

    private void Saltar()
    {
        rb2d.AddForce(new Vector2(0f, fuerzaDeSalto));
        salto = false;
    }

    private void BotonSaltoArriba()
    {
        if (rb2d.velocity.y > 0)
        {
            rb2d.AddForce(Vector2.down * rb2d.velocity.y * (1 - multiplicadorCancelarSalto), ForceMode2D.Impulse);
        }

        salto = false;
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

    IEnumerator CooldownEsquive()
    {
        yield return new WaitForSeconds(cooldownEsquive);

        esquivar = false;
    }

    IEnumerator CambioSaltoPared()
    {
        saltoDePared = true;
        yield return new WaitForSeconds(tiempoSaltoPared);
        saltoDePared = false;
        seRealizaSalto = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCajaSalto);
        Gizmos.DrawWireCube(controladorPared.position, dimensionesCajaPared);
        Gizmos.DrawWireCube(controladorEspaldaPared.position, dimensionesCajaPared);
    }

}
