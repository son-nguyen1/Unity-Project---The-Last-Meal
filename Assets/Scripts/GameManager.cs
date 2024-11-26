using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int activeSceneIndex;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            HandleGameScene();
        }
    }

    private void HandleGameScene()
    {
        if (activeSceneIndex == 0)
        {
            SceneManager.LoadScene(1);
        }
        else if (activeSceneIndex == 1)
        {
            SceneManager.LoadScene(2);
        }
        else if(activeSceneIndex == 3)
        {
            SceneManager.LoadScene(0);
        }
    }
}