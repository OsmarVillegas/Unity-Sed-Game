using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : Entidad
{
    public float velocidadDeMovimiento;

    public bool estaVivo = true;

    protected Animator animator;

    protected Rigidbody2D rb2D;

    protected bool mirandoLaDerecha = true;

    [SerializeField] private ParticleSystem particulas;

    public void TomarDaño(float daño)
    {
        vida -= daño;

        if (vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        GameObject.FindGameObjectWithTag("Meta").GetComponent<Meta>().EnemigoEliminado();
        animator.SetTrigger("Muerte");
        estaVivo = false;
        particulas.Play();
    }
    private IEnumerator SlowMotionEffect()
    {
        Time.timeScale = 0.2f; // Ralentiza el tiempo (ejemplo: 0.2)
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Ajusta la f�sica para que coincida con el tiempo ralentizado
        yield return new WaitForSecondsRealtime(0.15f); // Usa WaitForSecondsRealtime para medir tiempo real
        Time.timeScale = 1f; // Vuelve a la velocidad normal
        Time.fixedDeltaTime = 0.02f; // Restaura el fixedDeltaTime original
    }

    public void Girar()
    {
        mirandoLaDerecha = !mirandoLaDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        velocidadDeMovimiento *= -1;
    }

}
