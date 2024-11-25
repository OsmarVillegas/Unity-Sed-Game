using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicLevelManager : MonoBehaviour
{
    public static MusicLevelManager Instance; // Instancia única del MusicManager
    private AudioSource audioSource;


    [SerializeField] private AudioClip MenuMusic; // Música para el Menu
    [SerializeField] private AudioClip defaultMusic; // Música para nivel 1
    [SerializeField] private AudioClip level2Music;  // Música para nivel 2
    [SerializeField] private AudioClip level3Music; // Música para nivel 3

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
            audioSource = GetComponent<AudioSource>();
            PlayMusic(defaultMusic); // Reproducir música inicial
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Escuchar cambios de escena
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Dejar de escuchar cambios de escena
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int sceneIndex = scene.buildIndex;

        print("Escena Index: " + sceneIndex);

        if (sceneIndex == 0)
        {
            PlayMusic(MenuMusic);
        }

        // Cambiar música Buck
        if (sceneIndex == 2)
        {
            PlayMusic(defaultMusic);
        }

        if (sceneIndex == 5)
        {
            PlayMusic(level2Music);
        }

        else if (sceneIndex == 10)
        {
            PlayMusic(level3Music);
        }

        // Cambiar música Zero
        if (sceneIndex == 15)
        {
            PlayMusic(defaultMusic);
        }

        if (sceneIndex == 18)
        {
            PlayMusic(level2Music);
        }

        else if (sceneIndex == 21)
        {
            PlayMusic(level3Music);
        }


    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
