using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class CalcularRango : MonoBehaviour
{
    [SerializeField] Image image;
    public Sprite[] sprites;

    [SerializeField] private Score score;

    private void Start()
    {
        float value = score.GetScore();

        print("Rango: " + value);

        if (value >= 50000.0f)
        {
            image.sprite = sprites[3];
        }
        else if (value >= 40000.0f && value <= 50000.0f)
        {
            image.sprite = sprites[2];
        }
        else if (value >= 30000.0f && value < 40000.0f)
        {
            image.sprite = sprites[1];
        }
        else if (value < 30000.0f)
        {
            image.sprite = sprites[0];
        }

        score.ResetScore();
    }

}
