using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlataforma : MonoBehaviour
{
    PlatformEffector2D pE2D;

    private bool leftPlatform;

    // Start is called before the first frame update
    void Start()
    {
        pE2D = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Esquivar") && !leftPlatform)
        {
            pE2D.rotationalOffset = 180;

            leftPlatform = true;

            gameObject.layer = 2;

            StartCoroutine(CooldownReiniciar());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        RegresarPlataforma();
    }

    private IEnumerator CooldownReiniciar()
    {
        yield return new WaitForSeconds(0.5f);
        RegresarPlataforma();
    }


    private void RegresarPlataforma()
    {
        pE2D.rotationalOffset = 0;

        gameObject.layer = 6;

        leftPlatform = false;
    }

}
