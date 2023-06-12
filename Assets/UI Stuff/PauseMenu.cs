using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //Pause Menu   
    [SerializeField] private GameObject pauseCanvas;

    //Settings Menu
    [SerializeField] private GameObject settingsCanvas;

    private bool isPaused;

    private void Start()
    {
        //Makes sure the menus are deactivated
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    private void Update()
    {  
        //Checks if it is paused or not
        if (InputManagerPause.instance.MenuInput)
        {
            if (!isPaused)
            {
                Pause();
            }

            else
            {
                Unpause();
            }
        }
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        OpenMainMenu();
    }

    private void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        CloseAllMenus();
    }

    private void OpenMainMenu()
    {
        pauseCanvas.SetActive(true);
        Debug.Log("Paused");
        settingsCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    private void CloseAllMenus()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
        Time.timeScale = 1;
    }
}
