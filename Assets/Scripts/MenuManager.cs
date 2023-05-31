using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public GameObject htpClose, creditClose;

    public GameObject htpMenu;
    public GameObject creditMenu;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void HowToPlayClose()
    {
        //sets the object to true
        htpMenu.SetActive(false);

        //clears selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(htpClose);
    }

    public void CreditClose()
    {
        //sets the object to true
        creditMenu.SetActive(false);

        //clears selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(creditClose);
    }
}
