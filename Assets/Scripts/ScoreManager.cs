using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private GameManager gameManager;
    [SerializeField] private BarraDeTiempo barradeTiempo;

    [SerializeField] private Score score;
    private string saveFilePath;

    [SerializeField]private bool salvarPuntaje = false;
    [SerializeField] private bool ObtenerPuntaje = false;

    void Start()
    {
        gameManager.nivelFinalizado += calcularPuntaje;
        if (ObtenerPuntaje)
        {
            LoadScore();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "score.json");
    }

    public void SaveScore()
    {
        string json = score.ToJson(); // Convierte el ScriptableObject a JSON
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Puntuación guardada en: " + saveFilePath);
    }

    public void LoadScore()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            score.FromJson(json); // Convierte el JSON al ScriptableObject
            Debug.Log("Puntuación cargada: " + score.puntuacion);
        }
        else
        {
            Debug.Log("No se encontró un archivo de puntuación.");
            score.ResetScore(); // Resetea la puntuación si no existe un archivo
        }
    }

    private void calcularPuntaje()
    {
        score.AddScore(barradeTiempo.GetTiempoRestante() * 50);
        if (salvarPuntaje)
        {
            SaveScore();
            score.ResetScore();
        }
        print(score);
    }

}
