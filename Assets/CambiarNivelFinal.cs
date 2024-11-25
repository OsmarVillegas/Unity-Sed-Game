using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarNivelFinal : MonoBehaviour
{
    [SerializeField] private int indexLevel;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Space"))
        {
            CambiarEscena();
        }
    }

    public void CambiarEscena()
    {
        SceneManager.LoadScene(indexLevel);
    }
}
