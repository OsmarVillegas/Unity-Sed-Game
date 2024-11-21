using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        Debug.Log($"Inicializando Barra de Vida. Slider asignada: {slider != null}");
    }

    // Update is called once per frame
    public void CambiarVidaMaxima(float vidaMaxima)
    {
        if (slider == null) // Verifica si el slider o cualquier referencia está inicializado
        {
            Debug.LogError("El slider no está inicializado.");
            return;
        }
        slider.maxValue = vidaMaxima;
    }

    public void CambiarVidaActual(float cantidadVida)
    {
        if (slider == null) // Verifica si el slider o cualquier referencia está inicializado
        {
            Debug.LogError("El slider no está inicializado.");
            return;
        }
        slider.value = cantidadVida;
    }

    public void InicializarBarraDeVida(float cantidadVida)
    {
        CambiarVidaMaxima(cantidadVida);
        CambiarVidaActual(cantidadVida);
    }

}
