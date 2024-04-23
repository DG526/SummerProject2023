using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum HSBehavStatus
{
    Checking,
    Inputting,
    Displaying
}
public class HighScoreBehavior : MonoBehaviour
{
    HSBehavStatus status = HSBehavStatus.Checking;
    public GameObject table, newScreen;
    public TMP_InputField nameInput;
    public Button inputConfirm, inputCancel;
    public GameObject levelOps; //Options to become visible if beat game
    public Button levelTryAgain; //Default button if viewing after beating game
    public GameObject mMenuOps; //Options to become visible if viewing from Main Menu
    public Button mMenuReturn; //Default button if viewing from main menu
    public HighScoreList scorelist;
    public static int recentScore = 20;
    string path;

    private void Awake()
    {
        path = Application.persistentDataPath + "/highscores.sav";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            scorelist = JsonUtility.FromJson<HighScoreList>(json);
        }
        else
        {
            scorelist = new HighScoreList();
            for (int i = 0; i < 10; i++)
            {
                HighScore s = new HighScore("??????", 0);
                scorelist.scores.Add(s);
            }
            string json = JsonUtility.ToJson(scorelist);
            System.IO.File.WriteAllText(path, json);
            Debug.LogWarning("High Scores not found, created a new one at: " + path);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(recentScore <= 0 || !CheckNewHighScore())
        {
            status = HSBehavStatus.Displaying;
            newScreen.SetActive(false);
            table.SetActive(true);
            if(recentScore < 0)
            {
                levelOps.SetActive(false);
                EventSystem.current.SetSelectedGameObject(mMenuReturn.gameObject);
            }
            else
            {
                mMenuOps.SetActive(false);
                EventSystem.current.SetSelectedGameObject(levelTryAgain.gameObject);
            }
            DisplayHighScores();
        }
        else
        {
            newScreen.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = "Your Score: " + recentScore;
            newScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(nameInput.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayHighScores()
    {
        if (scorelist == null)
            return;
        //List<GameObject> places = new List<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            table.transform.Find("Place" + (i + 1)).Find("Name").GetComponent<TextMeshProUGUI>().text = scorelist.scores[i].GetName();
            table.transform.Find("Place" + (i + 1)).Find("Score").GetComponent<TextMeshProUGUI>().text = scorelist.scores[i].GetScore().ToString();
        }
    }

    public bool CheckNewHighScore()
    {
        return recentScore > scorelist.scores[9];
    }
    public void RecordHighScore(string name)
    {
        if (name == "")
            return;
        HighScore newScore = new HighScore(name, recentScore);
        for (int i = 0; i < 10; i++)
        {
            if(newScore > scorelist.scores[i])
            {
                scorelist.scores.Insert(i, newScore);
                while (scorelist.scores.Count > 10) //prune places 11 and beyond, just in case.
                {
                    scorelist.scores.RemoveAt(10);
                }
                string json = JsonUtility.ToJson(scorelist);
                System.IO.File.WriteAllText(path, json);
                Debug.LogWarning("High Scores saved at: " + path);
                return;
            }
        }
    }

    #region Buttons

    #region At start of screen
    public void ConfirmScore()
    {
        RecordHighScore(nameInput.text);
        DisplayHighScores();
        newScreen.SetActive(false);
        table.SetActive(true);
        mMenuOps.SetActive(false);
        EventSystem.current.SetSelectedGameObject(levelTryAgain.gameObject);
    }
    public void CancelScore()
    {
        DisplayHighScores();
        newScreen.SetActive(false);
        table.SetActive(true);
        mMenuOps.SetActive(false);
        EventSystem.current.SetSelectedGameObject(levelTryAgain.gameObject);
    }
    #endregion

    #region Exits
    public void ReturnToMMenu()
    {
        recentScore = -1;
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;

    }
    public void ReplayGame()
    {
        recentScore = -1;
        SceneManager.LoadScene("PlayerStuff");
        Time.timeScale = 1f;

    }
    #endregion

    #endregion
}

[System.Serializable]
public struct HighScore
{
    public string name; //name of player
    public int score;

    public HighScore(string n, int s) 
    {
        name = n;
        score = s;
    }
    static public bool operator >(int a, HighScore b)
    {
        return a > b.score;
    }
    static public bool operator <(int a, HighScore b)
    {
        return a < b.score;
    }
    static public bool operator >(HighScore a, HighScore b)
    {
        return a.score > b.score;
    }
    static public bool operator <(HighScore a, HighScore b)
    {
        return (a.score < b.score);
    }
    public string GetName()
    {
        return name;
    }
    public int GetScore()
    {
        return score;
    }
}

[System.Serializable]
public class HighScoreList
{
    public List<HighScore> scores;
    public HighScoreList()
    {
        scores = new List<HighScore>();
    }
}