using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Shopping : MonoBehaviour
{
    public PlayerHealth pHealth;
    public Shooting shoot;
    public PlayerMovement move;
    public GameObject player;

    [SerializeField] private GameObject shopCanvas;

    public Button healthButton;
    public Button speedButton;
    public Button ultButton;

    public Button fireButton;
    public Button waterButton;
    public Button earthButton;
    public Button windButton;
    public Button poisonButton;
    public Button lightingButton;

    //to be removed
    private bool isPaused;

    public TMP_Text speedText;
    public TMP_Text healthText;
    public TMP_Text ultText;
    public TMP_Text fireText;
    public TMP_Text waterText;
    public TMP_Text earthText;
    public TMP_Text windText;
    public TMP_Text poisonText;
    public TMP_Text lightingText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        pHealth = player.GetComponent<PlayerHealth>();
        move = player.GetComponent<PlayerMovement>();
        shoot = player.GetComponent<Shooting>();

        shopCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        //For Testing, make it so the shop shows at round 2 and before the final round
        //Checks if it is paused or not
        if (InputManagerPause.instance.MenuInput)
        {
            if (!isPaused)
            {
                OpenShop();

                //freezes everything
                Time.timeScale = 0f;
            }
        }
    }

    public void OpenShop()
    {
        shopCanvas.SetActive(true);

        //sets the first button when menu opens
        EventSystem.current.SetSelectedGameObject(healthButton.gameObject);
        Time.timeScale = 0;
    }

    public void AddHealth()
    {
        pHealth.numOfHearts = pHealth.numOfHearts + 1;
        if(pHealth.numOfHearts == 8)
        {
            healthText.text = "Sold Out!";
            healthButton.interactable = false;
        }
    }

    public void AddSpeed()
    {
        move.moveSpeed = move.moveSpeed + 1.5f;
        
        if(move.moveSpeed <= 9)
        {
            speedText.text = "Sold Out!";
            speedButton.interactable = false;
        }
    }
    public void AddUltCharge()
    {
        //One more ult charge but it's very expensive
        if (move.moveSpeed <= 9)
        {
            ultText.text = "Sold Out!";
            ultButton.interactable = false;
        }
    }
    public void FireUpgrade()
    {
        // tighter spread, more damage
        fireText.text = "Sold Out!";
        fireButton.interactable = false;
    }

    public void WaterUpgrade()
    {
        //Wider spread, more shots
        waterText.text = "Sold Out!";
        waterButton.interactable = false;
    }
    public void EarthUpgrade()
    {
        //Lower cooldown, more damage
        earthText.text = "Sold Out!";
        earthButton.interactable = false;
    }
    public void WindUpgrade()
    {
        //More force, larger spread
        windText.text = "Sold Out!";
        windButton.interactable = false;
    }
    public void PoisonUpgrade()
    {
        //Bigger cloud, tick rate faster, (maybe)slows enemy

        poisonText.text = "Sold Out!";
        poisonButton.interactable = false;
    }
    public void LightingUpgrade()
    {
        //more chains, more damage, *small stun
        lightingText.text = "Sold Out!";
        lightingButton.interactable = false;
    }

}
