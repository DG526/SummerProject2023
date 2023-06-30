using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //Menus
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private GameObject warningCanvas;

    //sets the first button
    [SerializeField] private GameObject loseFirstButton;
    [SerializeField] private GameObject winFirstButton;
    [SerializeField] private GameObject warningButton;

    public Loadout openLoadout;

    private bool isGameOver;

    private void Start()
    {
        openLoadout = GameObject.Find("LoadOut").GetComponent<Loadout>();
        //Makes sure the menus are deactivated
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
        warningCanvas.SetActive(false);
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
        warningCanvas.SetActive(false);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(winFirstButton);
    }
    public void OpenWarningMenu()
    {
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
        warningCanvas.SetActive(true);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(winFirstButton);
    }
    #endregion

    #region Navigation
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


    public void TryAgain()
    {
        SceneManager.LoadScene("PlayerStuff");
        Time.timeScale = 1f;

    }

    //goes to the next level
    public void NextMap()
    {
        openLoadout.OpenLoadout();
    }    
    #endregion
}
