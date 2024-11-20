using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EjecutarCinematicaFinal : MonoBehaviour
{
    [SerializeField] private float delay;

    [SerializeField] private GameObject transicionSalida;
    [SerializeField] private Animator AnimatorTransicion;
    [SerializeField] private AnimationClip animacionFinal;
    [SerializeField] private int sceneIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(cargarConDelay(delay));
        }
    }

    private IEnumerator cargarConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        transicionSalida.SetActive(true);
        AnimatorTransicion.SetTrigger("Iniciar");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneIndex);
        gameObject.SetActive(false);
    }
}
