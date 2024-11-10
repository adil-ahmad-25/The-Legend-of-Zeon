using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject menuScreen;
    public GameObject winScreen;
    public GameObject gameOverScreen;

    private void Start()
    {
        menuScreen.SetActive(false);
        winScreen.SetActive(false);
        gameOverScreen.SetActive(false);
    }
    public void OpenMenu()
    {
        menuScreen.SetActive(true);
    }

    public void CloseMenu()
    {
        menuScreen.SetActive(false);
    }

    public void WinGame()
    {
        winScreen.SetActive(true);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
