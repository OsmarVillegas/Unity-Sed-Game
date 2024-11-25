using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionJefe : MonoBehaviour
{
    [SerializeField] private float daño;

    [SerializeField] private Vector2 dimensionesCaja;

    [SerializeField] private Transform posicionCaja;

    [SerializeField] private float tiempoDeVida;

    // Start is called before the first frame update

    [Header("Sonido")]
    [SerializeField] private AudioClip explosion;


    void Start()
    {
        ControladorSonido.instance.EjecutarSonido(explosion);

        Destroy(gameObject, tiempoDeVida);
    }

    public void Golpe()
    {
        Collider2D[] objetos = Physics2D.OverlapBoxAll(posicionCaja.position, dimensionesCaja, 0f);

        foreach(Collider2D colisiones in objetos)
        {
            if (colisiones.CompareTag("Player"))
            {
                colisiones.GetComponent<VidaJugador>().TomarDanio();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(posicionCaja.position, dimensionesCaja);
    }
}
