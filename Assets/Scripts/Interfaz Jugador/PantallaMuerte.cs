using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PantallaMuerte : MonoBehaviour
{
    [SerializeField] private GameObject pantallaMuerte;

    private VidaJugador vidaJugador;

    // Start is called before the first frame update
    void Start()
    {
        vidaJugador = GameObject.FindGameObjectWithTag("Player").GetComponent<VidaJugador>();
        vidaJugador.MuerteJugador += ActivarPantalla;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void ActivarPantalla(object sender, EventArgs e)
    {
        pantallaMuerte.SetActive(true);
    }
}
