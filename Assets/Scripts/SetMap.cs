using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMap : MonoBehaviour
{
    public bool first = true;
    public HUD hud;

    [Header("Bosses")]
    public GameObject redDragon;
    public GameObject greenDragon;
    public GameObject blueDragon;
    public GameObject purpleDragon;
    public GameObject yellowDragon;
    public GameObject darkLightDragon;

    [Header("Minibosses")]
    public GameObject wyrmBoss;
    public GameObject drakeBoss;
    public GameObject wyvernBoss;

    [Header("Maps")]
    public Sprite redMap;
    public Sprite greenMap;
    public Sprite blueMap;
    public Sprite purpleMap;
    public Sprite yellowMap;

    [Header("Spawner")]
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
    public bool final = false;
    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<HUD>();
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
        Spawner.canSpawn = true;
        if (first)
        {
            StartCoroutine(MapCheck());
        }

        

        level = (int)(UnityEngine.Random.Range(0, levelTypes.Count));
        levelCount++;
        #region clear
        object[] obj = GameObject.FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj)
        {
            GameObject g = (GameObject)o;
            int layer = g.layer;
            if (layer == 6 || layer == 7 || layer == 8 || layer == 10 || layer == 11)
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
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else if (name.IndexOf("Dragon") != -1)
                {
                    spawn = Instantiate(darkLightDragon, child.position, child.rotation);
                    hud.bossHealth = spawn.GetComponent<EnemyHealth>();
                }
            }
        }
        #endregion

        #region normal level
        else
        {
            foreach (Transform child in gameObject.transform)
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
                    player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                else if (name.IndexOf("boss") != -1)
                {
                    int target = (int)(UnityEngine.Random.Range(1, 3));
                    switch (target)
                    {
                        case 1:
                            spawn = Instantiate(wyrmBoss, child.position, child.rotation);
                            spawn.GetComponent<SpriteRenderer>().color = GetColor();
                            break;
                        case 2:
                            spawn = Instantiate(drakeBoss, child.position, child.rotation);
                            spawn.GetComponent<SpriteRenderer>().color = GetColor();
                            break;
                        case 3:
                            spawn = Instantiate(wyvernBoss, child.position, child.rotation);
                            spawn.GetComponent<SpriteRenderer>().color = GetColor();
                            break;
                    }
                }
                else if (name.IndexOf("Dragon") != -1)
                {
                    string color = levelTypes[level];
                    if (color == "red")
                    {
                        spawn = Instantiate(redDragon, child.position, child.rotation);
                        gameObject.GetComponent<SpriteRenderer>().sprite = redMap;
                        hud.bossHealth = spawn.GetComponent<EnemyHealth>();
                    }
                    if (color == "green")
                    {
                        spawn = Instantiate(greenDragon, child.position, child.rotation);
                        gameObject.GetComponent<SpriteRenderer>().sprite = greenMap;
                        hud.bossHealth = spawn.GetComponent<EnemyHealth>();
                    }
                    if (color == "blue")
                    {
                        spawn = Instantiate(blueDragon, child.position, child.rotation);
                        gameObject.GetComponent<SpriteRenderer>().sprite = blueMap;
                        hud.bossHealth = spawn.GetComponent<EnemyHealth>();
                    }
                    if (color == "yellow")
                    {
                        spawn = Instantiate(yellowDragon, child.position, child.rotation);
                        gameObject.GetComponent<SpriteRenderer>().sprite = yellowMap;
                        hud.bossHealth = spawn.GetComponent<EnemyHealth>();
                    }
                    if (color == "purple")
                    {
                        spawn = Instantiate(purpleDragon, child.position, child.rotation);
                        gameObject.GetComponent<SpriteRenderer>().sprite = purpleMap;
                        hud.bossHealth = spawn.GetComponent<EnemyHealth>();
                    }
                }
            }
        }
        #endregion
    }

    public Color32 GetColor()
    {
        if (final)
        {
            int colPick = UnityEngine.Random.Range(0, 5);
            switch (colPick)
            {
                case 0:
                    return new Color32(255, 84, 84, 255);
                case 1:
                    return new Color32(107, 132, 116, 255);
                case 2:
                    return new Color32(39, 93, 248, 255);
                case 3:
                    return new Color32(255, 255, 50, 255);
                case 4:
                    return new Color32(164, 0, 255, 255);
            }
        }
        string color = levelTypes[level];
        if (color == "red")
            return new Color32 (255,84,84,255);
        if (color == "green")
            return new Color32(107, 132, 116, 255);
        if (color == "blue")
            return new Color32 (39,93,248,255);
        if (color == "yellow")
            return new Color32 (255,255,50,255);
        if (color == "purple")
            return new Color32(164,0,255,255);


        return Color.white;
    }
    public IEnumerator MapCheck()
    {
        yield return new WaitForSeconds(5);
        first = false;
    }
}