using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdministradorDeTutorial : MonoBehaviour
{
    [Header("Canvas")]
    [SerializeField] private GameObject canvas;

    public event Action TutorialSaltado;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Space"))
        {   
            TutorialSaltado?.Invoke();
            canvas.SetActive(false);
            Destroy(canvas);
        }
    }
}
