using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour {
  private Animator anim;
  [SerializeField] string nivel;
  public void Start() {
    anim = GetComponent<Animator>();
  }

  public void PasarNivel() {
    SceneManager.LoadScene(nivel);
  }

  public void HacerFade() {
    anim.SetTrigger("FadeOut");
  }
}
