using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private float velocidadDash;

    [SerializeField] private float distanciaDash;

    private Vector2 dashDirection;
    
    private bool isDashing;

    private float distanceTraveled = 0f;

    private void Update(){
        if (Input.GetButtonDown("Fire1"))
        {
            Golpe();
            Dash();
            RealizarDash();
        }

        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Verificar si la posición del mouse está dentro del radio permitido
        if (Vector3.Distance(transform.position, mouseWorldPosition) <= maxDistanceFromPlayer)
        {
            // Si está dentro del área permitida, actualizar la posición objetivo
            targetPosition = mouseWorldPosition;
        }
        else
        {
            // Si está fuera del área permitida, calcular la posición más cercana dentro del radio
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;
            targetPosition = transform.position + direction * maxDistanceFromPlayer;
        }

        // Calcular la distancia total entre la posición actual y la posición objetivo
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        controladorGolpe.transform.position = Vector3.MoveTowards(transform.position, targetPosition, distanceToTarget);

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

    private void Dash()
    {
        mouseWorldPosition.z = 0;

        dashDirection = (mouseWorldPosition - transform.position).normalized;

        isDashing = true;
        distanceTraveled = 0f;
    }

    private void RealizarDash()
    {
        if (isDashing)
        {
            float distanceToMove = velocidadDash * Time.deltaTime;

            transform.position += (Vector3)dashDirection * distanceToMove;

            distanceTraveled += distanceToMove;

            if (distanceTraveled >= distanciaDash)
            {
                isDashing = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
