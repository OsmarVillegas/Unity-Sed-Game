using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Formats.Alembic.Timeline;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Personajes> personajes;
    public BarraDeTiempo barraDeTiempo;
    private Enemigo[] enemigo;
    private Jefe jefe;
    [SerializeField] private GameObject alertaNext;
    [SerializeField] private GameObject ejecutarCinematica;
    private bool alertaActivada = false;

    public event Action nivelFinalizado;

    private bool seEnvioEvento=false;

    private void Start()
    {
        // Encuentra todos los objetos con el script Enemy
        enemigo = FindObjectsOfType<Enemigo>();
        jefe = FindObjectOfType<Jefe>();

        // Muestra la cantidad de enemigos
        Debug.Log("Cantidad de enemigos en la escena: " + (enemigo.Length));

    }

    private void Update()
    {

        if (jefe == null)
        {
            if (enemigo.Length == 0 && !alertaActivada)
            {
                if (!seEnvioEvento)
                {
                    nivelFinalizado?.Invoke();
                    seEnvioEvento = true;
                }

                ejecutarCinematica.SetActive(true);
                StartCoroutine(ActivarAlertaNext());
            }else if (enemigo.Length >= 1){
                alertaActivada = false;
            }

        }
        else
        {
            if (jefe == null && !alertaActivada)
            {
                if (!seEnvioEvento)
                {
                    nivelFinalizado?.Invoke();
                    ejecutarCinematica.SetActive(true);
                    seEnvioEvento = true;
                }
            }
        }

        enemigo = FindObjectsOfType<Enemigo>();
        jefe = FindObjectOfType<Jefe>();
    }

    private void EnemigoEstandar_muerteEnemigo()
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator ActivarAlertaNext()
    {
        alertaActivada = true;
        alertaNext.SetActive(true);
        yield return new WaitForSeconds(2.1f);
        alertaNext.SetActive(false);
        yield return new WaitForSeconds(0.6f);
        alertaActivada = false;
    }
}
