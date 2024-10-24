using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float vida;

    private Animator animator;

    public bool estaVivo;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        estaVivo = true;
    }

    public void TomarDaño(float daño)
    {
        vida -= daño;

        if(vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        animator.SetTrigger("Muerte");
        estaVivo = false;
    }

}
