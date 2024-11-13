using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuInicialSc : MonoBehaviour {
  public void Jugar(){
    SceneManager.LoadScene("MenuEleccionNivel");
  }

  public void Opciones(){
    SceneManager.LoadScene("MenuOpciones");
  }

  public void Salir(){
    Debug.Log("Saliendo...");
    Application.Quit();
  }
}
