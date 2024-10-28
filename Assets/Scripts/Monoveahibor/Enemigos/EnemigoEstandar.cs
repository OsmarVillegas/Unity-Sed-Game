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

    [Header("Detectar Jugador")]

    [SerializeField] private bool GizmosJugador = false;

    [SerializeField] private Collider2D detectionCollider;

    [SerializeField] private Transform controladorEspaldaJugador;

    [SerializeField] private Transform controladorEnFrenteJugador;

    [SerializeField] private LayerMask capaEnfrenteJugador;

    [SerializeField] private float distanciaEnFrenteJugador;

    [SerializeField] private float distanciaEspaldaJugador;

    private bool jugadorDetectado = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        detectionCollider.enabled = false;

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
            print("Comportamiento Default Activado");
        }
        else if(!estaVivo){
            Detenerse();
        }

    }


    private IEnumerator DetectarJugador()
    {
        while (true) { 
            RaycastHit2D hit = Physics2D.Raycast(controladorEnFrenteJugador.position, transform.right, distanciaEnFrenteJugador, capaEnfrenteJugador);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Objeto con el tag Player detectado.");
                    detectionCollider.enabled = true;
                    jugadorDetectado = true;
                }

            }
            else
            {
                Debug.Log("No se detectó ningún objeto.");
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

                if (comportamientoAleatorio >= 0 && comportamientoAleatorio <= 1)
                {
                    print("Patrullando...");
                }
                else
                {
                    print("Esperando...");
                }

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
        print("Comportamiento detenido");

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
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(controladorAbajo.transform.position, controladorAbajo.transform.position + transform.up * -1 * distanciaAbajo);
            Gizmos.DrawLine(controladorEnFrente.transform.position, controladorEnFrente.transform.position + transform.right * distanciaEnFrente);
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

            rb2D.velocity = new Vector2(velocidadDeMovimiento * 2, rb2D.velocity.y);

            animator.SetFloat("Horizontal", Mathf.Abs(velocidadDeMovimiento));

            print("Jugador Detectado");
            print(informacionEnFrente);
            print(informacionAbajo);
        }

    }

    void OnTriggerExit2D(Collider2D player)
    {
        jugadorDetectado = false;
        detectionCollider.enabled = false;
        if (player.CompareTag("Player"))
        {
            Debug.Log("El jugador ha salido del trigger");

        }
    }
}
