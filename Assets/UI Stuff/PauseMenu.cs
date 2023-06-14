using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //Pause Menu   
    [SerializeField] private GameObject pauseCanvas;

    //Settings Menu
    [SerializeField] private GameObject settingsCanvas;

    //sets the first button
    [SerializeField] private GameObject pauseFirstButton;
    [SerializeField] private GameObject settingFirstButton;

    private bool isPaused;

    private void Start()
    {
        //Makes sure the menus are deactivated
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }
    public void Change()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    private void Update()
    {  
        //Checks if it is paused or not
        if (InputManagerPause.instance.MenuInput)
        {
            if (!isPaused)
            {
                Pause();

                //freezes everything
                Time.timeScale = 0;
            }

            else
            {
                Unpause();

                //unfreezes
                Time.timeScale = 1;
            }
        }
    }

    #region Pausing
    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        OpenPauseMenu();
    }

    private void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        CloseAllMenus();
    }
    #endregion

    #region First Button Clicked
    private void OpenPauseMenu()
    {
        pauseCanvas.SetActive(true);
        Debug.Log("Paused");
        settingsCanvas.SetActive(false);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(pauseFirstButton);
    }
    private void OpenSettingMenu()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(true);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(settingFirstButton);
    }

    private void CloseAllMenus()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

        //sets the button to null 
        EventSystem.current.SetSelectedGameObject(null);
    }
    #endregion

    #region Navigation
    //Pause Menu Navigation
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ClickSettings()
    {
        OpenSettingMenu();
    }

    public void ClickResume()
    {
        Unpause();
    }

    //Setting Navigation
    public void ClickKeyboardConfig()
    {

    }

    public void ClickControllerConfig()
    {

    }

    public void OpenPause()
    {
        OpenPauseMenu();
    }
    #endregion
}
