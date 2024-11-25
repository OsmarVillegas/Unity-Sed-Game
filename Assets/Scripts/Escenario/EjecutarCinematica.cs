using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EjecutarCinematica : MonoBehaviour
{
    public PlayableDirector playableDirector;
    [SerializeField] private float delay;

    public event Action detenerJugador;

    public Vector3 cinematicStartPoint; // Posición inicial de la cinemática
    public float transitionSpeed = 5f; // Velocidad de la transición
    public Transform player;


    [SerializeField] private GameObject transicionSalida;
    [SerializeField] private Animator AnimatorTransicion;
    [SerializeField] private AnimationClip animacionFinal;
    [SerializeField] private int sceneIndex;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            detenerJugador?.Invoke();
            StartCoroutine(MoveToCinematicStartPoint(player));
            StartCoroutine(cargarConDelay(delay));
            ControladorNiveles.instancia.AumentarNiveles();
        }
    }

    private IEnumerator cargarConDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playableDirector.Play();
        transicionSalida.SetActive(true);
        AnimatorTransicion.SetTrigger("Iniciar");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneIndex);
        gameObject.SetActive(false);
    }

    public IEnumerator MoveToCinematicStartPoint(Transform player)
    {
        while (Vector2.Distance(player.position, cinematicStartPoint) > 0.1f)
        {
            player.position = Vector2.Lerp(player.position, cinematicStartPoint, transitionSpeed * Time.deltaTime);
            yield return null;
        }
        player.position = cinematicStartPoint; // Ajustar posición exacta
    }

}
