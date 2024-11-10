using System;
using System.Collections;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    private PlayerControllerV2 movimientoJugador;

    private Animator animator;

    public event EventHandler MuerteJugador;

    public BarraDeTiempo barraDeTiempo;

    public event Action OnTimeExpired;

    // Start is called before the first frame update
    void Start()
    {
        movimientoJugador = GetComponent<PlayerControllerV2>();
        barraDeTiempo.OnTimeExpired += MostrarPantalla;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void TomarDanio(Vector2 posicion)
    {
        animator.SetTrigger("Muerte");
        MostrarPantalla();
        movimientoJugador.estaVivo = false;
        Physics2D.IgnoreLayerCollision(9, 10, true);
        movimientoJugador.Rebote(posicion);
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

}
