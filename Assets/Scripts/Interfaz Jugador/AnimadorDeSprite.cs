using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimadorDeSprite : MonoBehaviour
{
    [Header("Sprite")]
    [SerializeField] private Image imageComponent;         // El componente Image donde cambiarás los sprites
    [SerializeField] private Sprite[] animationFrames;     // Arreglo de sprites para la animación
    [SerializeField] private float frameDuration = 0.1f;   // Duración de cada frame en segundos

    private int currentFrame = 0;        // Frame actual de la animación
    private float timer = 0f;            // Temporizador para cambiar de frame

    void Start()
    {
        if (animationFrames.Length > 0)
        {
            imageComponent.sprite = animationFrames[currentFrame];
        }
    }

    void Update()
    {
        if (animationFrames.Length == 0) return; // Asegúrate de que hay sprites en el arreglo

        // Actualizar el temporizador
        timer += Time.deltaTime;

        if (timer >= frameDuration)
        {
            // Avanzar al siguiente frame
            currentFrame = (currentFrame + 1) % animationFrames.Length;
            imageComponent.sprite = animationFrames[currentFrame];

            // Reiniciar el temporizador
            timer = 0f;
        }
    }

}
