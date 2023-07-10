using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Net.NetworkInformation;

public class Shopping : MonoBehaviour
{
    [Header ("Player")]
    public GameObject player;

    public PlayerHealth pHealth;
    public Shooting shoot;
    public PlayerMovement move;
    public Ultimate ult;
    public PlayerPoints points;
    public AudioManager audio;

    [SerializeField] private GameObject shopCanvas;

    [Header("Upgrade Costs")]
    public int healthCost;
    public int healthCostIncrement;
    public int speedCost;
    public int speedCostIncrement;
    public int ultimateCost;
    public int fireCost;
    public int waterCost;
    public int earthCost;
    public int windCost;
    public int poisonCost;
    public int lightningCost;

    [Header("Price Listings")]
    public TMP_Text healthCostText;
    public TMP_Text speedCostText;
    public TMP_Text ultCostText;
    public TMP_Text fireCostText;
    public TMP_Text waterCostText;
    public TMP_Text earthCostText;
    public TMP_Text windCostText;
    public TMP_Text poisonCostText;
    public TMP_Text lightingCostText;

    public int maxUltCharges = 2;

    [Header ("Player Buttons")]
    public Button healthButton;
    public Button speedButton;
    public Button ultButton;

    [Header ("Spell Buttons")]
    public Button fireButton;
    public Button waterButton;
    public Button earthButton;
    public Button windButton;
    public Button poisonButton;
    public Button lightingButton;

    //to be removed
    private bool isPaused;
    public TMP_Text scoreText;

    [Header ("Button Text")]
    public TMP_Text speedText;
    public TMP_Text healthText;
    public TMP_Text ultText;
    public TMP_Text fireText;
    public TMP_Text waterText;
    public TMP_Text earthText;
    public TMP_Text windText;
    public TMP_Text poisonText;
    public TMP_Text lightingText;

    public float speedCap = 15f;
    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        pHealth = player.GetComponent<PlayerHealth>();
        move = player.GetComponent<PlayerMovement>();
        shoot = player.GetComponent<Shooting>();
        ult = player.GetComponent<Ultimate>();
        points = player.GetComponent<PlayerPoints>();
        audio = GameObject.Find("Audio").GetComponent<AudioManager>();

        shopCanvas.SetActive(false);

        //Costs
        healthCostText.text = "" + healthCost;
        speedCostText.text = "" + speedCost;
        ultCostText.text = "" + ultimateCost;
        fireCostText.text = "" + fireCost;
        waterCostText.text = "" + waterCost;
        earthCostText.text = "" + earthCost;
        windCostText.text = "" + windCost;
        poisonCostText.text = "" + poisonCost;
        lightingCostText.text = "" + lightningCost;
    }

    public void Update()
    {
        scoreText.text = "" + points.GetPoints();
    }

    public void PlaySound()
    {
        audio.PlaySFX(audio.buy);
    }


    public void OpenShop()
    {
        shopCanvas.SetActive(true);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(healthButton.gameObject);
        Time.timeScale = 0f;
    }

    public void CloseShop()
    {
        shopCanvas.SetActive(false);
    }

    public void AddHealth()
    {
        if (points.GetPoints() >= healthCost)
        {
            points.RemovePoints(healthCost);
            healthCost += healthCostIncrement;
            healthCostText.text = "" + healthCost;

            pHealth.numOfHearts = pHealth.numOfHearts + 1;
            pHealth.health = pHealth.health + 1;
            if (pHealth.numOfHearts == 8)
            {
                healthText.text = "Sold Out!";
                healthButton.interactable = false;
            }
        }
    }

    public void AddSpeed()
    {
        if (points.GetPoints() >= speedCost)
        {
            points.RemovePoints(speedCost);
            speedCost += speedCostIncrement;
            speedCostText.text = "" + speedCost;

            move.moveSpeed = move.moveSpeed + 1.5f;
            if (move.moveSpeed >= speedCap)
            {
                speedText.text = "Sold Out!";
                speedButton.interactable = false;
            }
        }
    }
    public void AddUltCharge()
    {
        //One more ult charge but it's very expensive
        if (points.GetPoints() >= ultimateCost)
        {
            points.RemovePoints(ultimateCost);
            ult.maxCharges = ult.maxCharges + 1;
            if (ult.maxCharges == maxUltCharges)
            {
                ultText.text = "Sold Out!";
                ultButton.interactable = false;
            }
        }
    }
    public void FireUpgrade()
    {
        // tighter spread, more damage
        if (points.GetPoints() >= fireCost)
        {
            points.RemovePoints(fireCost);
            shoot.circleSpread = shoot.circleSpread * 0.5f;
            shoot.circleUpgraded = true;
            fireText.text = "Sold Out!";
            fireButton.interactable = false;
        }
    }

    public void WaterUpgrade()
    {
        //Wider spread, more shots
        if (points.GetPoints() >= waterCost)
        {
            points.RemovePoints(waterCost);
            shoot.numBullets = shoot.numBullets + 4;
            shoot.triangleSpreadInterval = shoot.triangleSpreadInterval + 0.05f;
            waterText.text = "Sold Out!";
            waterButton.interactable = false;
        }
    }
    public void EarthUpgrade()
    {
        //Lower cooldown, more damage
        if (points.GetPoints() >= earthCost)
        {
            points.RemovePoints(earthCost);
            shoot.rockCDI = shoot.rockCDI * 0.75f;
            shoot.rockUpgraded = true;

            earthText.text = "Sold Out!";
            earthButton.interactable = false;
        }
    }
    public void WindUpgrade()
    {
        //More force, larger spread
        if (points.GetPoints() >= windCost)
        {
            points.RemovePoints(windCost);
            shoot.windUpgraded = true;

            windText.text = "Sold Out!";
            windButton.interactable = false;
        }
    }
    public void PoisonUpgrade()
    {
        //Bigger cloud, tick rate faster, (maybe)slows enemy
        if (points.GetPoints() >= poisonCost)
        {
            points.RemovePoints(poisonCost);
            shoot.poisonUpgraded = true;
            poisonText.text = "Sold Out!";
            poisonButton.interactable = false;
        }
    }
    public void LightingUpgrade()
    {
        //more chains, more damage, *small stun
        if (points.GetPoints() >= lightningCost)
        {
            points.RemovePoints(lightningCost);
            shoot.lightningUpgraded = true;
            lightingText.text = "Sold Out!";
            lightingButton.interactable = false;
        }
    }

}
