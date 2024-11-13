using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Meta : MonoBehaviour {
  [SerializeField] private int cantidadEnemigos;
  [SerializeField] private int enemigosEliminados;
  void Start() {
    cantidadEnemigos = GameObject.FindGameObjectsWithTag("Enemigo").Length;
  }

  public void EnemigoEliminado(){
    enemigosEliminados += 1;
  }

  private void OnTriggerEnter2D(Collider2D other) {
    // Si el nivel es sin enemigos solo comparara si el jugador entra en la meta
    if(other.CompareTag("Player") && cantidadEnemigos == 0){
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Si el nivel tiene enemigos compara que el jugador entre en meta 
    // y la cantidad de enemigos eliminados sea igual a la cantidad de enemigos inicial
    if(other.CompareTag("Player") && cantidadEnemigos == enemigosEliminados){
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
  }
}
