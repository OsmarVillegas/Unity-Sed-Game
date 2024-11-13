using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNiveles : MonoBehaviour {

  public void SeleccionNivel(int level) {
    SceneManager.LoadScene(level);
  }
}
