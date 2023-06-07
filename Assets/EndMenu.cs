using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{

    public GameObject endMenuUI;

    public void gameOver()
    {
        endMenuUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
        // Debug.Log("Quit!");
    }
}