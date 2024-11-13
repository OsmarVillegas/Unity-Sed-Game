using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public BarraDeTiempo barraDeTiempo;

    private void Start()
    {
        if (barraDeTiempo != null)
        {
            // Suscribirse al evento de finalizaci�n de tiempo del Timer
            barraDeTiempo.OnTimeExpired += TiempoTerminado;
        }
    }

    private void TiempoTerminado()
    {
        // Aqu� defines lo que sucede cuando el tiempo se acaba
        Debug.Log("�El tiempo ha terminado!");
        // Ejemplo: cargar otra escena o mostrar pantalla de fin de nivel
    }

    private void OnDestroy()
    {
        // Aseg�rate de desuscribirte del evento para evitar problemas
        if (barraDeTiempo != null)
        {
            barraDeTiempo.OnTimeExpired -= TiempoTerminado;
        }
    }
}
