using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeTiempo : MonoBehaviour
{
    [Header("Tiempo Nivel")]
    [SerializeField] private float segundos;
    [SerializeField] private bool activarTimer;
    private float tiempoRestante;
    private Slider slider;

    [SerializeField] private AdministradorDeTutorial administradorTutorial;
    [SerializeField] private GameManager gameManager;

    public event Action OnTimeExpired;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.maxValue = segundos;
        tiempoRestante = segundos;
        slider.value = tiempoRestante;

        administradorTutorial.TutorialSaltado += ActivarTimer;
        gameManager.nivelFinalizado += DesactivarTimer;
    }

    // Update is called once per frame
    void Update()
    {
        if (activarTimer)
            {
            if (tiempoRestante > 0)
            {
               tiempoRestante -= Time.deltaTime;
               slider.value = tiempoRestante;
            }
            else
            {
                // Si el tiempo llega a cero, dispara el evento
                OnTimeExpired?.Invoke();
            }
        }
    }
    private void ActivarTimer()
    {
        activarTimer = true;
    }

    private void DesactivarTimer()
    {
        activarTimer = false;
    }

    public float GetTiempoRestante()
    {
        return Mathf.Max(tiempoRestante, 0);
    }
}
