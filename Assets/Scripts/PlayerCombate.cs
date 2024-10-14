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

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("Golpe", false);
    }

    private void Update(){

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

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void Golpe()
    {

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);

        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                colisionador.transform.GetComponent<Enemigo>().TomarDaño(dañoGolpe);
            }
        }
    }
    private IEnumerator Dash()
    {
        animator.SetBool("Golpe", true);
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        Vector3 dashDirection = (mouseWorldPosition - transform.position).normalized;
        rb.velocity = dashDirection * dashingPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        animator.SetBool("Golpe", false); 
        tr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        rb.gravityScale = originalGravity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
