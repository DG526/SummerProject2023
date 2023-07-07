using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [Header("Scripts")]
    public Loadout openLoadout;
    public PlayerPoints playerPoints;

    //Menus
    [Header ("Canvas")]
    public GameObject textCanvas;
    public GameObject loseCanvas;
    public GameObject winCanvas;
    public GameObject warningCanvas;
    public GameObject congratsCanvas;

    //sets the first button
    [Header("Buttons")]
    public GameObject loseFirstButton;
    public GameObject winFirstButton;
    public GameObject warningButton;
    public GameObject congratsButton;

    [Header ("Misc")]
    public GameObject map;
    public TMP_Text scoreText;
    public TMP_Text enemiesDef;

    public int defeated = 0;

    private void Start()
    {
        openLoadout = GameObject.Find("LoadOut").GetComponent<Loadout>();
        //Makes sure the menus are deactivated
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
        warningCanvas.SetActive(false);
        textCanvas.SetActive(false);
        congratsCanvas.SetActive(false);

        map = GameObject.Find("Map");
    }

    #region Screen
    public void Win()
    {
        OpenWinMenu();
        Time.timeScale = 0f;
    }
    public void WinWaitStart(float seconds)
    {
        StartCoroutine(WinWait(seconds));
    }

    public void ShowText()
    {
        string score = "Final Score: " + playerPoints.GetPoints();
        string def = "Enemies Defeated: " + defeated;
        scoreText.text = score;
        enemiesDef.text = def;
    }
    IEnumerator WinWait(float seconds)
    {
        bool isWaiting = true;
        if (isWaiting)
        {
            isWaiting = false;
            yield return new WaitForSecondsRealtime(seconds);
        }
        Win();
    }

    public void Lose()
    {
        OpenLoseMenu();
        Time.timeScale = 0f;
    }

    public void OpenCongrats()
    {
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
        warningCanvas.SetActive(false);
        congratsCanvas.SetActive(true);
        textCanvas.SetActive(true);

        ShowText();
        Time.timeScale = 0f;
        
        EventSystem.current.SetSelectedGameObject(congratsButton);
    }
    #endregion

    #region First Button Clicked
    private void OpenLoseMenu()
    {
        loseCanvas.SetActive(true);
        winCanvas.SetActive(false);
        textCanvas.SetActive(false);
        ShowText();

            //sets the first button when menu opens
            EventSystem.current.SetSelectedGameObject(loseFirstButton);
    }
    private void OpenWinMenu()
    {
        loseCanvas.SetActive(false);
        winCanvas.SetActive(true);
        warningCanvas.SetActive(false);
        textCanvas.SetActive(true);

        ShowText();

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(winFirstButton);
    }
    public void OpenWarningMenu()
    {
        loseCanvas.SetActive(false);
        winCanvas.SetActive(false);
        warningCanvas.SetActive(true);
        textCanvas.SetActive(true);


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
        winCanvas.SetActive(false);
        textCanvas.SetActive(false);
        loseCanvas.SetActive(false);
        warningCanvas.SetActive(false);
        congratsCanvas.SetActive(false);
        openLoadout.OpenLoadout();
    }    
    #endregion
}
