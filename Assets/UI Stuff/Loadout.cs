using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Loadout : MonoBehaviour
{
    //Canvas
    [SerializeField] private GameObject loadOutCanvas;

    public SetMap map;

    //Button
    public Button fireButton;
    public Button waterButton;
    public Button earthButton;
    public Button windButton;
    public Button poisonButton;
    public Button lightningButton;
    public Button aoe;
    public Button beam;
    public Button clone;
    public Button confirm;

    //checks if buttons where choosen
    public bool fire1 = false;
    public bool fire2 = false;
    public bool ult = false;

    [Header("Selected Sprites")]
    //Change sprite
    public Sprite fireSelected;
    public Sprite waterSelected;
    public Sprite earthSelected;
    public Sprite windSelected;
    public Sprite poisonSelected;
    public Sprite lightningSelected;
    public Sprite aoeSelected;
    public Sprite beamSelected;
    public Sprite cloneSelected;

    [Header("Sprites")]
    //Change back
    public Sprite fireSprite;
    public Sprite waterSprite;
    public Sprite earthSprite;
    public Sprite windSprite;
    public Sprite poisonSprite;
    public Sprite lightningSprite;
    public Sprite aoeSprite;
    public Sprite beamSprite;
    public Sprite cloneSprite;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<SetMap>();

        Time.timeScale = 0f;
        loadOutCanvas.SetActive(true);
        OpenLoadout();
        confirm.interactable = false;
        player = GameObject.Find("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (fire1 && fire2 && ult)
        {
            confirm.interactable = true;
        }
    }
    public void OpenLoadout()
    {
        EventSystem.current.SetSelectedGameObject(fireButton.gameObject);
        Time.timeScale = 0f;

        if (!map.first)
        {
            GameObject.FindGameObjectWithTag("BGM Player").GetComponent<BGMLooper>().StopTrack();
            GameObject.FindGameObjectWithTag("BGM Player").GetComponent<BGMLooper>().PlayTrack(Track.Level);
            map.Set();
        }
    }
    public void CheckLoadout()
    {
        if (fire1 && fire2 && ult)
        {
            loadOutCanvas.SetActive(false);
            GameObject title = GameObject.Find("LevelTitle");
            if (title)
                title.GetComponent<LevelIntroText>().enabled = true;
            Time.timeScale = 1f;
        }
    }

    public void ChooseFire()
    {
        fireButton.image.sprite = fireSelected;
        waterButton.image.sprite = waterSprite;
        earthButton.image.sprite = earthSprite;
        player.GetComponent<Shooting>().Fire1 = "circle";
        fire1 = true;
    }

    public void ChooseWater()
    {
        fireButton.image.sprite = fireSprite;
        waterButton.image.sprite = waterSelected;
        earthButton.image.sprite = earthSprite;
        player.GetComponent<Shooting>().Fire1 = "triangle";
        fire1 = true;
    }
    public void ChooseEarth()
    {
        fireButton.image.sprite = fireSprite;
        waterButton.image.sprite = waterSprite;
        earthButton.image.sprite = earthSelected;
        player.GetComponent<Shooting>().Fire1 = "rockDrop";
        fire1 = true;
    }
    public void ChooseWind()
    {
        windButton.image.sprite = windSelected;
        poisonButton.image.sprite = poisonSprite;
        lightningButton.image.sprite = lightningSprite;
        player.GetComponent<Shooting>().Fire2 = "wind";
        fire2 = true;
    }
    public void ChoosePoison()
    {
        windButton.image.sprite = windSprite;
        poisonButton.image.sprite = poisonSelected;
        lightningButton.image.sprite = lightningSprite;
        player.GetComponent<Shooting>().Fire2 = "poison";

        fire2 = true;
    }
    public void ChooseLightning()
    {
        windButton.image.sprite = windSprite;
        poisonButton.image.sprite = poisonSprite;
        lightningButton.image.sprite = lightningSelected;
        player.GetComponent<Shooting>().Fire2 = "lightning";
        fire2 = true;
    }
    public void ChooseBeam()
    {
        beam.image.sprite = beamSelected;
        aoe.image.sprite = aoeSprite;
        clone.image.sprite = cloneSprite;
        player.GetComponent<Ultimate>().ultimate = "beam";
        ult = true;
    }
    public void ChooseAOE()
    {
        beam.image.sprite = beamSprite;
        aoe.image.sprite = aoeSelected;
        clone.image.sprite = cloneSprite;
        player.GetComponent<Ultimate>().ultimate = "aoe";
        ult = true;
    }
    public void ChooseClone()
    {
        beam.image.sprite = beamSprite;
        aoe.image.sprite = aoeSprite;
        clone.image.sprite = cloneSelected;
        player.GetComponent<Ultimate>().ultimate = "clone";
        ult = true;
    }

}