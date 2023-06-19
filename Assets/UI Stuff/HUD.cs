using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Shooting shooting;
    public Ultimate ultimate;
    public PlayerHealth playerHealth;

    List<Image> fullHearts = new List<Image>();
    List<Image> emptyHearts = new List<Image>();

    GameObject player;

    string fire1;
    string fire2;
    string ult;

    Image primaryImage;
    Image secondaryImage;
    Image ultImage;

    public Sprite fire;
    public Sprite water;
    public Sprite lightning;
    public Sprite rock;
    public Sprite poison;
    public Sprite wind;

    public Sprite ultBeam;
    public Sprite ultAOE;
    public Sprite ultClone;

    public int health;
    public int numOfHearts;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        shooting = player.GetComponent<Shooting>();
        ultimate = player.GetComponent<Ultimate>();
        playerHealth = player.GetComponent<PlayerHealth>();

        fire1 = shooting.Fire1;
        fire2 = shooting.Fire2;
        ult = ultimate.ultimate;

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
        SetFireImages();
        SetUltImages();
    }

    // Update is called once per frame
    void Update()
    {
        if (fire1 != shooting.Fire1 || fire2 != shooting.Fire2)
        {
            fire1 = shooting.Fire1;
            fire2 = shooting.Fire2;

            SetFireImages();
        }

        if(ult != ultimate.ultimate)
        {
            ult = ultimate.ultimate;

            SetUltImages();
        }
    }

    void SetFireImages()
    {
        #region PrimaryFire
        if (fire1 == "circle")
        {
            primaryImage.sprite = fire;
        }
        if(fire1 == "triangle")
        {
            primaryImage.sprite = water;
        }
        if (fire1 == "lightning")
        {
            primaryImage.sprite = lightning;
        }
        #endregion

        #region SecondaryFire
        if (fire2 == "rockDrop")
        {
            secondaryImage.sprite = rock;
        }
        if (fire2 == "poison")
        {
            secondaryImage.sprite = poison;
        }
        if (fire2 == "wind")
        {
            secondaryImage.sprite = wind;
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
}
