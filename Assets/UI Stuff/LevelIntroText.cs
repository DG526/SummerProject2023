using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroText : MonoBehaviour
{
    bool init = false;

    TextMeshProUGUI textDisp;
    Material fontMaterial;

    DragonColor dragonID;
    bool darkLight;
    Dictionary<DragonColor, string> texts = new Dictionary<DragonColor, string>()
    {
        {DragonColor.RED, "Ashgarn's\nBlistering Cavern"},
        {DragonColor.BLUE, "Silgara's\nDrowned Marsh"},
        {DragonColor.YELLOW, "Zilgyrn's\nCrackling Peaks"},
        {DragonColor.PURPLE, "Miastara's\nPutrid Bog"},
        {DragonColor.GREEN, "Whiillex's\nHowling Grove"}
    };
    Dictionary<DragonColor, Color> colors = new Dictionary<DragonColor, Color>()
    {
        {DragonColor.RED, new Color32(255, 84, 84, 255)},
        {DragonColor.BLUE, new Color32(39, 93, 248, 255)},
        {DragonColor.YELLOW, new Color32(255, 255, 50, 255)},
        {DragonColor.PURPLE, new Color32(164, 0, 255, 255)},
        {DragonColor.GREEN, new Color32(26, 92, 26, 255)}
    };

    float a;
    Color c;
    float timeAlive = 0;
    
    private void Awake()
    {
        textDisp = GetComponent<TextMeshProUGUI>();
        fontMaterial = textDisp.fontMaterial;

        textDisp.color = new Color(1, 1, 1, 0);

        fontMaterial.SetFloat("_FaceDilate", 0);

        textDisp.fontMaterial = fontMaterial;

        enabled = false;
    }
    /*
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.name.IndexOf("Dragon") == -1)
            {
                continue;
            }
            else
            {
                if (enemy.GetComponent<DarkLightDragonBehavior>() != null)
                    darkLight = true;
                else
                    dragonID = enemy.GetComponent<DragonBehavior>().color;
                break;
            }
        }

        if (darkLight)
        {

        }
        else
        {

        }
    }*/
    private void OnEnable()
    {
        a = 0;
        timeAlive = 0;

        textDisp.color = new Color(1, 1, 1, 0);

        fontMaterial.SetFloat("_FaceDilate", 0);


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.name.IndexOf("Dragon") == -1)
            {
                continue;
            }
            else
            {
                if (enemy.GetComponent<DarkLightDragonBehavior>() != null)
                    darkLight = true;
                else
                    dragonID = enemy.GetComponent<DragonBehavior>().color;
                break;
            }
        }

        if (darkLight)
        {
            textDisp.text = "???";
            c = Color.white;
        }
        else
        {
            textDisp.text = texts[dragonID];
            c = colors[dragonID];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeAlive > 4.5f)
        {
            textDisp.color = new Color(0, 0, 0, 0);
            enabled = false;
            return;
        }
        timeAlive += Time.deltaTime;
        if (timeAlive < 1f) //Fading in for the first second.
        {
            a = Mathf.Clamp01(timeAlive);
            textDisp.color = Color.Lerp(new Color(c.r, c.g, c.b, 0), c, a);
        }
        else if (timeAlive > 3f) //Fading out after 2 seconds of still.
        {
            a = Mathf.Clamp01(1 - (timeAlive - 3f) / 1.5f);
            textDisp.color = Color.Lerp(new Color(0, 0, 0, 0), c, a);
            fontMaterial.SetFloat("_FaceDilate", a - 1);
        }
        else //Still.
        {
            a = 1;
            textDisp.color = c;
        }
    }
}
