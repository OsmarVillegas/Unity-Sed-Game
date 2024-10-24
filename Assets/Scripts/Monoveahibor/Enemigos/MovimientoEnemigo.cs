using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class MovimientoEnemigo : MonoBehaviour
{
    [Header("Movimiento")]
    
    private Rigidbody2D rb2D;

    private Animator animator;

    [SerializeField] private float velocidadDeMovimiento;

    [SerializeField] private LayerMask capaAbajo;

    [SerializeField] private LayerMask capaEnfrente;

    [SerializeField] private float distanciaAbajo;

    [SerializeField] private float distanciaEnFrente;

    [SerializeField] private Transform controladorAbajo;

    [SerializeField] private Transform controladorEnFrente;

    private bool informacionAbajo;

    private bool informacionEnFrente;

    private bool mirandoLaDerecha = true;

    private int comportamientoAleatorio;

    [Header("Detectar Jugador")]

    private bool jugadorEnFrente;

    private bool jugadorAtras;

    private bool vivo;

    // Start is called before the first frame update
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(ComportamientoEnemigo());

    }

    private void FixedUpdate()
    {

        if (vivo)
        {
            switch (comportamientoAleatorio)
            {
                case 0:
                    PatrullarEscenario();
                    break;
                case 1:
                    Detenerse();
                    break;
            }
        }
        else
        {
            Detenerse();
        }
    }

    private IEnumerator ComportamientoEnemigo()
    {
        while (true)
        {
            comportamientoAleatorio = Random.Range(0, 2);
            print(comportamientoAleatorio);

            yield return new WaitForSeconds(5f);

            if (!vivo)
            {
                break;
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

    private void Girar()
    {
        mirandoLaDerecha = !mirandoLaDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        velocidadDeMovimiento *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controladorAbajo.transform.position, controladorAbajo.transform.position + transform.up * -1 * distanciaAbajo);
        Gizmos.DrawLine(controladorEnFrente.transform.position, controladorEnFrente.transform.position + transform.right * distanciaEnFrente);
    }
}
