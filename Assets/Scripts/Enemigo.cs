using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float vida;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TomarDa�o(float da�o)
    {
        vida -= da�o;

        if(vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        // Animator muerte
    }
}
