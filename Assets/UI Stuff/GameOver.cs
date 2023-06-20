using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //Lose Menu   
    [SerializeField] private GameObject loseCanvas;

    //Win Menu
    [SerializeField] private GameObject winCanvas;

    //sets the first button
    [SerializeField] private GameObject loseFirstButton;
    [SerializeField] private GameObject winFirstButton;


    private bool isGameOver;

    private void Start()
    {
        //Makes sure the menus are deactivated
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
    }

    #region Screen
    public void Win()
    {
        Time.timeScale = 0f;
        loseCanvas.SetActive(true);
        winCanvas.SetActive(true);

        OpenWinMenu();
    }

    public void Lose()
    {
        Time.timeScale = 0f;
        loseCanvas.SetActive(true);
        winCanvas.SetActive(true);

        OpenLoseMenu();
    }
    #endregion

    #region First Button Clicked
    private void OpenLoseMenu()
    {
        loseCanvas.SetActive(true);
        winCanvas.SetActive(false);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(loseFirstButton);
    }
    private void OpenWinMenu()
    {
        loseCanvas.SetActive(false);
        winCanvas.SetActive(true);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(winFirstButton);
    }
    #endregion

    #region Navigation
    //Pause Menu Navigation
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("PlayerStuff");
        Time.timeScale = 1f;

    }
    #endregion
}
