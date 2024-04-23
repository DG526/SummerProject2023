using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    //Different Canvases
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject howToPlay;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;

    //Buttons
    [SerializeField] private GameObject mainMenuButton;
    [SerializeField] private GameObject howToPlayButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject creditsButton;


    public Texture2D cursor;

    //Starts the game
    public void Start()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);

        //calls the function and holds that function for that many seconds
        mainMenu.SetActive(true);
        settings.SetActive(false);
        howToPlay.SetActive(false);
        credits.SetActive(false);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Close the game
    public void EndGame()
    {
        Application.Quit();
    }

    #region Canvas Activation/Button
    private void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        howToPlay.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(false);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(mainMenuButton);
    }

    private void OpenHTP()
    {
        mainMenu.SetActive(false);
        howToPlay.SetActive(true);
        settings.SetActive(false);
        credits.SetActive(false);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(howToPlayButton);
    }

    private void OpenSettings()
    {
        mainMenu.SetActive(false);
        howToPlay.SetActive(false);
        settings.SetActive(true);
        credits.SetActive(false);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(settingsButton);
    }

    private void OpenCredits()
    {
        mainMenu.SetActive(false);
        howToPlay.SetActive(false);
        settings.SetActive(false);
        credits.SetActive(true);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(creditsButton);
    }
    #endregion

    #region How to Play
    public void ClickMenu()
    {
        OpenMainMenu();
    }
    public void ClickHowtoPlay()
    {
        OpenHTP();
    }
    #endregion

    #region Settings
    public void ClickSettings()
    {
        OpenSettings();
    }
    public void Change()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void ClickKeyboardConfig()
    {

    }

    public void ClickControllerConfig()
    {

    }
    #endregion

    #region Credits
    public void ClickCredits()
    {
        OpenCredits();
    }
    #endregion

    #region Highscores
    public void OpenHighScores()
    {
        HighScoreBehavior.recentScore = -1;
        SceneManager.LoadScene("HighScores");
    }
    #endregion
}
