using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMap : MonoBehaviour
{
    [Header ("Bosses")]
    public GameObject redDragon;
    public GameObject greenDragon;
    public GameObject blueDragon;
    public GameObject purpleDragon;
    public GameObject yellowDragon;
    public GameObject darkLightDragon;

    [Header ("Minibosses")]
    public GameObject wyrmBoss;
    public GameObject drakeBoss;
    public GameObject wyvernBoss;

    [Header ("Spawner")]
    public GameObject spawner;

    [Header("Player and Camera")]
    public GameObject player;
    public Camera camera;

    [Header("Level Set")]
    public int maxLevelCount = 3;
    public int level = 0;
    public List<string> levelTypes = new List<string>();
    int levelCount = 0;
    GameObject finalMap;
    bool final = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        camera = Camera.main;
        finalMap = GameObject.Find("Final Map");
        levelTypes.Add("red");
        levelTypes.Add("green");
        levelTypes.Add("blue");
        levelTypes.Add("purple");
        levelTypes.Add("yellow");

        Set();
        //Debug.Log(level);
    }


    public void Set()
    {
        levelCount++;
        #region clear
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            int layer = g.layer;
            if(layer == 6 || layer == 7 || layer == 8 || layer == 10 || layer == 11)
                Destroy(g);
        }
        #endregion

        #region final level
        if (levelCount > maxLevelCount)
        {
            final = true;
            Destroy(gameObject.GetComponent<Boundaries>());
            foreach (Transform child in finalMap.transform)
            {
                string name = child.gameObject.name;
                GameObject spawn;
                if (name.IndexOf("Spawner") != -1)
                {
                    spawn = Instantiate(spawner, child.position, child.rotation);
                }
                else if (name.IndexOf("Player") != -1)
                {
                    player.transform.position = child.position;
                    camera.transform.position = new Vector3(child.position.x, child.position.y, camera.transform.position.z);
                }
                else if (name.IndexOf("Dragon") != -1)
                {
                    spawn = Instantiate(darkLightDragon, child.position, child.rotation);
                }
            }
        }
        #endregion

        #region normal level
        else
        {
            foreach (Transform child in gameObject.transform)
            {
                level = (int)(Random.Range(0, levelTypes.Count - 1));
                string name = child.gameObject.name;

                GameObject spawn;
                if (name.IndexOf("Spawner") != -1)
                {
                    spawn = Instantiate(spawner, child.position, child.rotation);
                }
                else if (name.IndexOf("Player") != -1)
                {
                    player.transform.position = child.position;
                    camera.transform.position = new Vector3(child.position.x, child.position.y, camera.transform.position.z);
                }
                else if (name.IndexOf("Dragon") != -1)
                {
                    string color = levelTypes[level];
                    if (color == "red")
                        spawn = Instantiate(redDragon, child.position, child.rotation);
                    if (color == "green")
                        spawn = Instantiate(greenDragon, child.position, child.rotation);
                    if (color == "blue")
                        spawn = Instantiate(blueDragon, child.position, child.rotation);
                    if (color == "yellow")
                        spawn = Instantiate(yellowDragon, child.position, child.rotation);
                    if (color == "purple")
                        spawn = Instantiate(purpleDragon, child.position, child.rotation);
                }
            }
        }
        #endregion
    }

    public Color32 GetColor()
    {
        if (final)
            return Color.black;
        string color = levelTypes[level];
        if(color == "red")
            return Color.red;
        if(color == "green")
            return Color.green;
        if(color == "blue")
            return Color.blue;
        if (color == "yellow")
            return Color.yellow;
        if (color == "purple")
            return new Color32(164, 0, 255, 255);


        return Color.white;
    }
}
