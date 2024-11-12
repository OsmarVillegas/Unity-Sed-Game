using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuOpcionesSc : MonoBehaviour {
  [SerializeField] private AudioMixer audioMixer;
    public void PantallaCompleta(bool pantallCompleta) {
    Screen.fullScreen = pantallCompleta;
  }

  public void CambiarVolumen(float volumen) {
    audioMixer.SetFloat("Volumen", volumen);
  }

  public void CambiarCalidad(int index) {
    QualitySettings.SetQualityLevel(index);
  }

  public void Volver(){
    SceneManager.LoadScene("MenuInicial");
  }
}
