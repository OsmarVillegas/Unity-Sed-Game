using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlataforma : MonoBehaviour
{
    PlatformEffector2D pE2D;

    public bool leftPlatform;

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
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        pE2D.rotationalOffset = 0;

        gameObject.layer = 6;

        leftPlatform = false;
    }

}
