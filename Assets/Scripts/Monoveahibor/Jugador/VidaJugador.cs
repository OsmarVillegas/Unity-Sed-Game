using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    private PlayerControllerV2 movimientoJugador;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        movimientoJugador = GetComponent<PlayerControllerV2>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void TomarDanio(Vector2 posicion)
    {
        animator.SetTrigger("Muerte");
        movimientoJugador.estaVivo = false;
        Physics2D.IgnoreLayerCollision(9, 10, true);
        movimientoJugador.Rebote(posicion);
    }
}
