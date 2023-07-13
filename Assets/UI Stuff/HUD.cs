using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    [Header ("Scripts")]
    public Shooting shooting;
    public Ultimate ultimate;
    public PlayerHealth playerHealth;
    public GameObject gem;
    public PlayerPoints playerPoints;
    public EnemyHealth bossHealth;
    public Texture2D cursor;

    GameObject player;

    [Header ("Loadout")]
    string fire1;
    string fire2;
    string ult;

    Image primaryImage;
    Image secondaryImage;
    Image ultImage;

    [Header ("Spells")]
    public Sprite fire;
    public Sprite water;
    public Sprite lightning;
    public Sprite rock;
    public Sprite poison;
    public Sprite wind;

    [Header ("Ultimate")]
    public Sprite ultBeam;
    public Sprite ultAOE;
    public Sprite ultClone;

    [Header ("Health")]
    public int health;
    public int numOfHearts;

    public TMP_Text scoreText;

    public GameObject[] enemies;
    public bool isHealthBar = false;
    public GameObject healthBar;
    public TMP_Text enemyName;

    [Header ("Cooldown")]
    [SerializeField] private Image cooldownImage;
    public TMP_Text charge;
    float cooldown = 0f;


    // Start is called before the first frame update
    void Start()
    {
        //For Ult Cooldown
        cooldownImage.fillAmount = 0.0f;

        //Changes cusor's appearance
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);

        //finds player object and resets points
        player = GameObject.FindWithTag("Player");
        playerPoints.ResetPoints();

        //grab scripts from player object
        shooting = player.GetComponent<Shooting>();
        ultimate = player.GetComponent<Ultimate>();
        playerHealth = player.GetComponent<PlayerHealth>();

        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.name.IndexOf("Dragon") == -1)
            {
                continue;
            }
            else
            {
                bossHealth = enemy.GetComponent<EnemyHealth>();
                break;
            }
        }

        //finds what the player shoots
        fire1 = shooting.Fire1;
        fire2 = shooting.Fire2;
        ult = ultimate.ultimate;

        gem = player.transform.GetChild(2).gameObject;

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "PrimaryFire")
            {
                primaryImage = child.gameObject.GetComponent<Image>();
            }

            if (child.gameObject.name == "SecondaryFire")
            {
                secondaryImage = child.gameObject.GetComponent<Image>();
            }

            if(child.gameObject.name == "Ultimate")
            {
                ultImage = child.gameObject.GetComponent<Image>();
            }
        }

        //sets the images depending the choosen loadout
        SetFireImages();
        SetUltImages();

    }

    // Update is called once per frame
    public void Update()
    {
        if (fire1 != shooting.Fire1 || fire2 != shooting.Fire2)
        {
            fire1 = shooting.Fire1;
            fire2 = shooting.Fire2;

            SetFireImages();
        }

        if (ult != ultimate.ultimate)
        {
            ult = ultimate.ultimate;

            SetUltImages();
        }

        UpdateScore();
        charge.text = "" + ultimate.charges;

        CheckCooldown();

        //for the boss health bar
        if (isHealthBar && bossHealth != null)
        {
            if (!healthBar.gameObject.activeInHierarchy)
            {
                healthBar.gameObject.SetActive(true);
            }

            //Names of the Dragons
            if (bossHealth.gameObject.name.IndexOf("Red") != -1)
            {
                enemyName.text = "Fire Dragon: Ashgarn";
                healthBar.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 84, 84, 255);
            }
            if (bossHealth.gameObject.name.IndexOf("Blue") != -1)
            {
                enemyName.text = "Water Dragon: Silgara";
                healthBar.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(39, 93, 248, 255);
            }
            if (bossHealth.gameObject.name.IndexOf("Yellow") != -1)
            {
                enemyName.text = "Lightning Dragon: Zilgyrn";
                healthBar.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(255, 255, 50, 255);
            }
            if (bossHealth.gameObject.name.IndexOf("Purple") != -1)
            {
                enemyName.text = "Poison Dragon: Miastara";
                healthBar.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(164, 0, 255, 255);
            }
            if (bossHealth.gameObject.name.IndexOf("Green") != -1)
            {
                enemyName.text = "Typhoon Dragon: Whiillex";
                healthBar.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color32(154, 255, 154, 255);
            }
            if (bossHealth.gameObject.name.IndexOf("Dark") != -1)
            {
                enemyName.text = "Dark-Light Dragon: Rex Grauzenn";
                healthBar.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;
            }

            healthBar.transform.GetChild(1).GetComponent<Image>().fillAmount = (float)bossHealth.health / (float)bossHealth.maxHealth;
        }
    }

    void SetFireImages()
    {
        #region PrimaryFire
        if (fire1 == "circle")
        {
            primaryImage.sprite = fire;
            gem.GetComponent<SpriteRenderer>().color = new Color32(243, 78, 4, 255);
        }
        else if(fire1 == "triangle")
        {
            primaryImage.sprite = water;
            gem.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else if (fire1 == "rockDrop")
        {
            primaryImage.sprite = rock;
            gem.GetComponent<SpriteRenderer>().color = new Color32(237, 72, 241, 150);
        }
        #endregion

        #region SecondaryFire
        if (fire2 == "lightning")
        {
            secondaryImage.sprite = lightning;
            secondaryImage.color = new Color32(255,255, 255, 255);
        }
        else if (fire2 == "poison")
        {
            secondaryImage.sprite = poison;
            secondaryImage.color = new Color32(237, 72, 241, 150);
        }
        else if (fire2 == "wind")
        {
            secondaryImage.sprite = wind;
            secondaryImage.color = new Color32(255, 255, 255, 255);
        }
        #endregion
    }

    void SetUltImages()
    {
        if (ult == "aoe")
        {
            ultImage.sprite = ultAOE;
        }
        if (ult == "clone")
        {
            ultImage.sprite = ultClone;
        }
        if (ult == "beam")
        {
            ultImage.sprite = ultBeam;
        }
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + playerPoints.GetPoints();
    }

    public void CheckCooldown()
    {

        if (ult == "aoe")
        {
            cooldown = ultimate.aoeCD - Time.time;
            cooldown = (float)(cooldown/ultimate.aoeCDI);
        }
        if (ult == "clone")
        {
            cooldown = ultimate.cloneCD - Time.time;
            cooldown = (float)(cooldown / ultimate.cloneCDI);
        }
        if (ult == "beam")
        { 
            cooldown = ultimate.beamCD - Time.time;
            cooldown = (float)(cooldown / ultimate.beamCDI);
        }

        cooldownImage.fillAmount = cooldown;
    }
}
