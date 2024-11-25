using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSeleccionPersonaje : MonoBehaviour {

    [SerializeField] private Image imageComponent;
    [SerializeField] private Sprite[] personajes;
    private int indexPersonaje;

    private void Start() {
        indexPersonaje = 0;
        imageComponent.sprite = personajes[indexPersonaje];
    }

    public void derecha()
    {
        indexPersonaje = 1;
        imageComponent.sprite = personajes[indexPersonaje];
    }

    public void izquierda()
    {
        indexPersonaje = 0;
        imageComponent.sprite = personajes[indexPersonaje];
    }

    public void seleccionarPersonaje()
    {

        if (indexPersonaje == 0)
        {
            SceneManager.LoadScene(9);
        }

        if (indexPersonaje == 1)
        {
            SceneManager.LoadScene(14);
        }
    }

}
