using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMusic : MonoBehaviour
{
    private void Start()
    {
        if (MusicLevelManager.Instance != null)
        {
            MusicLevelManager.Instance.OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
    }

}
