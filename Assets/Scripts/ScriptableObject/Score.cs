using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


[CreateAssetMenu(menuName = "Score")]
public class Score : ScriptableObject
{
    public float puntuacion;
    public void AddScore(float amount)
    {
        puntuacion += amount;
    }

    public void ResetScore()
    {
        puntuacion = 0;
    }
    public string ToJson()
    {
        ScoreData data = new ScoreData
        {
            puntuacion = puntuacion
        };
        return JsonUtility.ToJson(data);
    }

    // Deserializar desde JSON
    public void FromJson(string json)
    {
        ScoreData data = JsonUtility.FromJson<ScoreData>(json);
        puntuacion = data.puntuacion;
    }
}
