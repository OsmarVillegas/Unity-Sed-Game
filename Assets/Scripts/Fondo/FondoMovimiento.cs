using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoMovimiento : MonoBehaviour
{
    [Header("Movimiento Fondo")]
    [SerializeField] private Vector2 velocidadMovimiento;

    private Vector2 offset;
    private Material material;
    private Transform camaraTransform;
    private Vector3 camaraPosicionAnterior;

    // Start is called before the first frame update
    private void Awake()
    {
        material = GetComponent<SpriteRenderer>().material;
        camaraTransform = Camera.main.transform; // Obtener la referencia a la c�mara principal
        camaraPosicionAnterior = camaraTransform.position; // Guardar la posici�n inicial de la c�mara
    }

    // Update is called once per frame
    void Update()
    {
        // Calcular el desplazamiento de la c�mara
        Vector3 camaraDesplazamiento = camaraTransform.position - camaraPosicionAnterior;

        // Actualizar el offset del fondo basado en el movimiento de la c�mara
        offset = new Vector2(camaraDesplazamiento.x, camaraDesplazamiento.y) * velocidadMovimiento;
        material.mainTextureOffset += offset;

        // Actualizar la posici�n anterior de la c�mara
        camaraPosicionAnterior = camaraTransform.position;
    }
}
