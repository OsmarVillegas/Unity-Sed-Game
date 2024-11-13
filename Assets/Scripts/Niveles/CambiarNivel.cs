using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarNivel : MonoBehaviour {
  
  public void CambiarEscena(string nombre) {
    ControladorNiveles.instancia.AumentarNiveles();
   SceneManager.LoadScene(nombre);
  }
}
