using System;
using System.Collections;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    private PlayerControllerV2 movimientoJugador;

    private Animator animator;

    public event EventHandler MuerteJugador;

    public BarraDeTiempo barraDeTiempo;

    // Start is called before the first frame update
    void Start()
    {
        movimientoJugador = GetComponent<PlayerControllerV2>();
        barraDeTiempo.OnTimeExpired += MostrarPantalla;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void TomarDanio()
    {
        animator.SetTrigger("Muerte");
        MostrarPantalla();
        movimientoJugador.estaVivo = false;
        Physics2D.IgnoreLayerCollision(9, 10, true);
    }

    private void MostrarPantalla()
    {
        StartCoroutine(DelayPantallaDeMuerte(1));
    }
    IEnumerator DelayPantallaDeMuerte(int tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        movimientoJugador.estaVivo = false;
        Physics2D.IgnoreLayerCollision(9, 10, true);
        MuerteJugador?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator SlowMotionEffect()
    {
        Time.timeScale = 0.2f; // Ralentiza el tiempo (ejemplo: 0.2)
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Ajusta la f�sica para que coincida con el tiempo ralentizado
        yield return new WaitForSecondsRealtime(0.15f); // Usa WaitForSecondsRealtime para medir tiempo real
        Time.timeScale = 1f; // Vuelve a la velocidad normal
        Time.fixedDeltaTime = 0.02f; // Restaura el fixedDeltaTime original
    }

}
