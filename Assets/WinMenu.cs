using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{

    public GameObject winMenuUI;

    public void gameOver()
    {
        winMenuUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Start Screen");
    }

    public void QuitGame()
    {
        Application.Quit();
        // Debug.Log("Quit!");
    }
}