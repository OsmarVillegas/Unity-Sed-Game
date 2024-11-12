using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditorInternal;
using UnityEngine;

public class EnemigoEstandar : Enemigo
{

    [Header("Movimiento")]

    [SerializeField] private Transform controladorAbajo;

    [SerializeField] private Transform controladorEnFrente;

    [SerializeField] private LayerMask capaAbajo;

    [SerializeField] private LayerMask capaEnfrente;

    [SerializeField] private float distanciaAbajo;

    [SerializeField] private float distanciaEnFrente;

    private bool informacionAbajo;

    private bool informacionEnFrente;

    private int comportamientoAleatorio;

    private int girarAleatorio;

    private BoxCollider2D boxCollider;

    [Header("Detectar Jugador")]

    [SerializeField] private bool GizmosJugador = false;

    [SerializeField] private Collider2D detectionCollider;

    [SerializeField] private Transform controladorEspaldaJugador;

    [SerializeField] private Transform controladorEnFrenteJugador;

    [SerializeField] private LayerMask capaJugador;

    [SerializeField] private float distanciaEnFrenteJugador;

    [SerializeField] private float distanciaEspaldaJugador;

    private bool jugadorDetectado = false;

    private float velocidadOriginal;

    [Header("Ataque")]

    [SerializeField] private Transform controladorDanio;

    [SerializeField] private Vector2 tamanioDanio;

    private bool estaEnArea;

    [SerializeField] private float tiempoAntesDeAtaque;

    [SerializeField] private float duracionAtaque;

    [SerializeField] private float coolDownAtaque;

    private bool puedeAtacar;

    private VidaJugador vidaJugador;

    private PlayerControllerV2 playerController;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        GameObject jugador = GameObject.FindWithTag("Player");
        vidaJugador = jugador.GetComponent<VidaJugador>();
        playerController = jugador.GetComponent<PlayerControllerV2>();

        detectionCollider.enabled = false;
        velocidadOriginal = velocidadDeMovimiento;
        puedeAtacar = true;

        StartCoroutine(ComportamientoEnemigo());
        StartCoroutine(DetectarJugador());
    }

    private void FixedUpdate()
    {
        if (estaVivo && !jugadorDetectado)
        {
            switch (comportamientoAleatorio)
            {
                case 0:
                    PatrullarEscenario();
                    break;
                case 1:
                    PatrullarEscenario();
                    break;
                case 2:
                    Detenerse();
                    break;
            }
            //print("Comportamiento Default Activado");
        }
        else if(!estaVivo){
            Detenerse();
            jugadorDetectado = false;
            detectionCollider.enabled = false;
            Destroy(boxCollider);
            StartCoroutine(DestruirEnemigo());
        }
    }

    private IEnumerator DestruirEnemigo()
    {
        yield return new WaitForSeconds(7);

        Destroy(gameObject);
    }


    private IEnumerator DetectarJugador()
    {
        while (true) { 
            RaycastHit2D hitEnFrente = Physics2D.Raycast(controladorEnFrenteJugador.position, transform.right, distanciaEnFrenteJugador, capaJugador);

            if (hitEnFrente.collider != null)
            {
                if (hitEnFrente.collider.CompareTag("Player"))
                {
                    detectionCollider.enabled = true;
                    jugadorDetectado = true;
                }

            }

            RaycastHit2D hitEspalda = Physics2D.Raycast(controladorEspaldaJugador.position, transform.right * -1, distanciaEspaldaJugador, capaJugador);

            if (hitEspalda.collider != null)
            {
                if (hitEspalda.collider.CompareTag("Player"))
                {
                    detectionCollider.enabled = true;
                    jugadorDetectado = true;
                }

            }

            yield return null;
        }
    }

    private IEnumerator ComportamientoEnemigo()
    {
        while (true)
        {
            if (!jugadorDetectado) { 
                comportamientoAleatorio = Random.Range(0, 3);

                //if (comportamientoAleatorio >= 0 && comportamientoAleatorio <= 1)
                //{
                //    print("Patrullando...");
                //}
                //else
                //{
                //    print("Esperando...");
                //}

                yield return new WaitForSeconds(5f);

                if (!estaVivo)
                {
                    break;
                }

            }else
            {
                yield return new WaitForSeconds(5f);
            }
        }
        //print("Comportamiento detenido");

    }

    private void Detenerse()
    {
        rb2D.velocity = new Vector2(0,0);
        animator.SetFloat("Horizontal", Mathf.Abs(0));
    }

    private void PatrullarEscenario()
    {
        rb2D.velocity = new Vector2(velocidadDeMovimiento, rb2D.velocity.y);
        animator.SetFloat("Horizontal", Mathf.Abs(velocidadDeMovimiento));
        informacionEnFrente = Physics2D.Raycast(controladorEnFrente.position, transform.right, distanciaEnFrente, capaEnfrente);
        informacionAbajo = Physics2D.Raycast(controladorEnFrente.position, transform.up * -1, distanciaAbajo, capaAbajo);

        if (!informacionAbajo || informacionEnFrente)
        {
            Girar();
        }
    }

    private void OnDrawGizmos()
    {
        if (GizmosJugador)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(controladorEnFrenteJugador.transform.position, controladorEnFrenteJugador.transform.position + transform.right * distanciaEnFrenteJugador);
            Gizmos.DrawLine(controladorEspaldaJugador.transform.position, controladorEnFrenteJugador.transform.position + transform.right * -1 * distanciaEspaldaJugador);
        }
        else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(controladorAbajo.transform.position, controladorAbajo.transform.position + transform.up * -1 * distanciaAbajo);
            Gizmos.DrawLine(controladorEnFrente.transform.position, controladorEnFrente.transform.position + transform.right * distanciaEnFrente);
        }


        if (estaEnArea)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.gray;
        }

        Gizmos.DrawWireCube(controladorDanio.position, tamanioDanio);


    }

    private IEnumerator Golpear()
    {
        yield return new WaitForSeconds(tiempoAntesDeAtaque);

        //print("Golpeando...");
        animator.SetBool("Golpe", true);

        yield return new WaitForSeconds(duracionAtaque);

        animator.SetBool("Golpe", false);
        yield return new WaitForSeconds(coolDownAtaque);
        puedeAtacar = true;
    }

    private void Danio()
    {
        RaycastHit2D hit = Physics2D.Raycast(controladorEnFrente.position, transform.right, distanciaEnFrente - 0.6f, capaJugador);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                vidaJugador.TomarDanio(new Vector2(transform.position.x, transform.position.y).normalized);
            }
        }
    }

    void OnTriggerStay2D(Collider2D player)
    {
        if (player.CompareTag("Player"))
        {
            // Obtener la posición del jugador
            Vector3 playerPosition = player.transform.position;

            // Calcular la dirección hacia la posición X del jugador
            Vector2 direction = new Vector2(playerPosition.x - transform.position.x, 0).normalized;

            if (direction.x == 1 && !mirandoLaDerecha)
            {
                Girar();
            }
            else if (direction.x == -1 && mirandoLaDerecha)
            {
                Girar();
            }

            // Aplicar velocidad para mover el objeto hacia la posición X del jugador
            informacionEnFrente = Physics2D.Raycast(controladorEnFrente.position, transform.right, distanciaEnFrente, capaEnfrente);

            rb2D.velocity = new Vector2(velocidadDeMovimiento * 2, rb2D.velocity.y);

            RaycastHit2D hit = Physics2D.Raycast(controladorEnFrente.position, transform.right, distanciaEnFrente - 1, capaJugador);

            bool jugadorEnArea = false;

            if (hit.collider != null)
            {
                if(hit.collider.CompareTag("Player"))
                {
                    jugadorEnArea = true;
                    estaEnArea = true;
                    if (puedeAtacar && playerController.estaVivo)
                    {
                        puedeAtacar = false;
                        StartCoroutine(Golpear());
                    }
                }
            }
            else
            {
                estaEnArea = false;
            }

            if (informacionEnFrente || jugadorEnArea || !puedeAtacar || !playerController.estaVivo)
            {
                velocidadDeMovimiento = 0;
            }
            else
            {
                if(mirandoLaDerecha)
                {
                    velocidadDeMovimiento = velocidadOriginal;
                }

                if (!mirandoLaDerecha)
                {
                    velocidadDeMovimiento = velocidadOriginal * -1;
                }

            }

            animator.SetFloat("Horizontal", Mathf.Abs(velocidadDeMovimiento));
        }

    }

    void OnTriggerExit2D(Collider2D player)
    {
        jugadorDetectado = false;
        detectionCollider.enabled = false;
    }
}
