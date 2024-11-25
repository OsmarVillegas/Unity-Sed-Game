using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerCombate : MonoBehaviour
{
    [Header("Ataque")]

    [SerializeField] private Transform controladorGolpe;

    [SerializeField] private float radioGolpe;

    [SerializeField] private float dañoGolpe;

    private Vector3 mouseWorldPosition;

    [SerializeField] private float maxDistanceFromPlayer;

    private Vector3 targetPosition;

    [SerializeField] private AdministradorDeTutorial administradorTutorial;
    [SerializeField] private bool tutorialFinalizado = false;

    [Header("Dash")]

    [SerializeField] private float dashingPower;

    [SerializeField] private float dashingTime;

    [SerializeField] private float dashingCooldown;

    [SerializeField] private TrailRenderer tr;

    private Rigidbody2D rb;

    private Vector2 dashDirection;

    private bool isDashing;

    private bool canDash = true;

    private Animator animator;

    [Header("Muerte")]

    private PlayerControllerV2 playerControllerV2;

    [SerializeField] private EjecutarCinematica ejecutarCinematica;

    [Header("Sonido")]

    [SerializeField] private AudioClip ataqueSonido;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("Golpe", false);
        playerControllerV2 = GetComponent<PlayerControllerV2>();
        ejecutarCinematica.detenerJugador += DesactivarMovimiento;

        administradorTutorial.TutorialSaltado += ActivarMovimiento;
    }

    private void Update()
    {

        if (tutorialFinalizado)
        {

            if (playerControllerV2.estaVivo)
            {

                if (isDashing)
                {
                    return;
                }

                if (Input.GetButtonDown("Fire1") && canDash)
                {
                    Golpe();
                    StartCoroutine(Dash());
                }

                mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorldPosition.z = 0f;

                if (Vector3.Distance(transform.position, mouseWorldPosition) <= maxDistanceFromPlayer)
                {
                    targetPosition = mouseWorldPosition;
                }
                else
                {
                    Vector3 direction = (mouseWorldPosition - transform.position).normalized;
                    targetPosition = transform.position + direction * maxDistanceFromPlayer;
                }

                float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

                controladorGolpe.transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToTarget);

            }

        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void ActivarMovimiento()
    {
        tutorialFinalizado = true;
    }
    private void DesactivarMovimiento()
    {
        tutorialFinalizado = false;
    }

    private void Golpe()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo") && !colisionador.isTrigger)
            {
                colisionador.transform.GetComponent<EnemigoEstandar>().TomarDanio(dañoGolpe);
            }

            if (colisionador.CompareTag("Jefe") && !colisionador.isTrigger)
            {
                colisionador.transform.GetComponent<Jefe>().TomarDaño(dañoGolpe);
            }
        }
    }

    private IEnumerator Dash()
    {
        animator.SetBool("Golpe", true);
        canDash = false;
        isDashing = true;
        ControladorSonido.instance.EjecutarSonido(ataqueSonido);
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 10;
        Vector3 dashDirection = (mouseWorldPosition - transform.position).normalized;
        dashDirection.z = 0f;
        rb.velocity = dashDirection * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        animator.SetBool("Golpe", false);
        tr.emitting = false;
        isDashing = false;
        rb.gravityScale = originalGravity;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
