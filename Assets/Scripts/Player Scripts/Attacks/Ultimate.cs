using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ultimate : MonoBehaviour
{

    [Header ("Scripts and Prefabs")]
    public Shooting shooting;
    public GameObject clone;
    public PlayerHealth playerHealth;
    public AudioManager audio;

    [Header ("Clone")]
    public float cloneCDI = 30f;
    public float cloneCD = 0f;
    public float cloneDuration = 15f;
    public Vector3 cloneSpawnPos = Vector3.zero;

    [Header ("AoE Blast")]
    public float aoeCDI = 20f;
    public float aoeCD = 0f;
    public float aoeSize = 10f;
    public float aoeStart = 0.5f;
    public float aoeTime = 1f;

    [Header ("Beam")]
    public float beamCDI = 30f;
    public float beamCD = 0f;
    public float beamTickSpeed = 0.15f;
    public float beamTickDuration = 0.1f;
    public Vector2 beamSize = Vector2.zero;
    public Vector2 beamStart = new Vector2(0.1f, 1f);
    public float beamChargeTime = 1f;
    public float beamDuration = 8f;
    public float beamOffet = 0.9f;

    [Header ("Ultimate Charges")]
    public int maxCharges = 1;
    public int charges = 1;

    [Header("Selected Ultimate")]
    public string ultimate = "aoe";

    //script specific
    GameObject AoE;
    bool AoEactive = false;

    public bool cloneActive = false;

    GameObject Beam;
    bool beamActive;
    float beamEnd = 0f;
    

    Color beamColor;
    float beamAlpha;
    Vector2 beamStartPos;
    float beamTick = 0f;

    float nextCharge = 0f;
    bool firstUlt = true;

    public InputActionAsset inputs;
    InputAction IAUltiFire;
    GameObject loadout;
    //BoxCollider2D beamCollider;
    private void Awake()
    {
        IAUltiFire = inputs.FindAction("Level Actions/Fire Ultimate");
    }
    private void OnEnable()
    {
        IAUltiFire.Enable();
    }
    private void OnDisable()
    {
        IAUltiFire.Disable();
    }
    void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();

        shooting = GetComponent<Shooting>();

        audio = GameObject.Find("Audio").GetComponent<AudioManager>();

        charges = maxCharges;

        loadout = GameObject.Find("LoadOut");

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name == "AoE Ult")
            {
                AoE = child.gameObject;
            }

            if (child.gameObject.name == "Beam Ult")
            {
                Beam = child.gameObject;
                beamColor = Beam.GetComponent<SpriteRenderer>().color;
                beamAlpha = beamColor.a;

                beamStartPos = Beam.transform.localPosition;
                beamStart = Beam.transform.localScale;
                //beamCollider = Beam.GetComponent<BoxCollider2D>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(charges < maxCharges && nextCharge < Time.time && !firstUlt)
        {
            charges++;
            FindNextCharge();
        }

        if (!playerHealth.dead && !loadout.activeInHierarchy)
        {
            if (IAUltiFire.triggered)
            {
                FireUltimate(ultimate);
            }

            if (beamActive && Time.time > beamEnd)
            {
                //reset color
                Beam.GetComponent<SpriteRenderer>().color = beamColor;
                Debug.Log("Beam End");
                beamActive = false;

                //reset start position and size
                Beam.transform.localPosition = beamStartPos;
                Beam.transform.localScale = beamStart;

                //Turn off object
                Beam.SetActive(false);
            }
            else if (beamActive && Time.time > beamTick)
            {
                Beam.GetComponent<Collider2D>().enabled = true;
                //Beam.GetComponent<BoxCollider2D>().isTrigger = true;

                beamTick = Time.time + beamTickSpeed;

                StartCoroutine(BeamTickDamage(beamTickDuration));
            }
        }
    }

    void FireUltimate(string ult)
    {
        if (ult == "aoe" && charges > 0 && !AoEactive)
        {
            aoeCD = Time.time + aoeCDI;
            if (charges == maxCharges)
                FindNextCharge();
            charges--;

            //audio
            audio.PlaySFX(audio.light);
            if (firstUlt)
            {
                FindNextCharge();
                firstUlt = false;
            }
            AoEactive = true;
            //aoeCD = Time.time + aoeCDI;

            if (!AoE.activeInHierarchy)
            {
                AoE.SetActive(true);
            }
            StartCoroutine(ScaleOverTime(aoeTime));
        }

        if (ult == "clone" && charges > 0 && !cloneActive)
        {
            cloneCD = Time.time + cloneCDI;
            if (charges == maxCharges)
                FindNextCharge();
            charges--;

            //audio
            audio.PlaySFX(audio.light);
            if (firstUlt)
            {
                FindNextCharge();
                firstUlt = false;
            }
            //cloneCD = Time.time + cloneCDI;

            GameObject spawnClone = Instantiate(clone, gameObject.transform.position + cloneSpawnPos, gameObject.transform.rotation);

            Shooting cloneShoot = spawnClone.GetComponent<Shooting>();

            //change clone loadout to player loadout
            cloneShoot.Fire1 = shooting.Fire1;
            cloneShoot.Fire2 = shooting.Fire2;
            spawnClone.GetComponent<PlayerMovement>().moveSpeed = gameObject.GetComponent<PlayerMovement>().moveSpeed;

            cloneShoot.circleUpgraded = shooting.circleUpgraded;
            cloneShoot.numBullets = shooting.numBullets;
            cloneShoot.rockUpgraded = shooting.rockUpgraded;
            cloneShoot.windUpgraded = shooting.windUpgraded;
            cloneShoot.poisonUpgraded = shooting.poisonUpgraded;
            cloneShoot.lightningUpgraded = shooting.lightningUpgraded;

            /*Shields cloneShield = spawnClone.GetComponent<Shields>();
            Shields playerShield = gameObject.GetComponent<Shields>();
            cloneShield.shieldEnd = playerShield.shieldEnd;
            cloneShield.active = playerShield.active;

            if(cloneShield.active) 
            {
                foreach(Transform child in spawnClone.transform)
                {
                    if (child.name != "Shields")
                    {
                        continue;
                    }

                    if (!child.gameObject.activeInHierarchy)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }*/

            PlayerCatalyst clonePlayerCatalyst = spawnClone.GetComponent<PlayerCatalyst>();
            PlayerCatalyst playerCatalyst = gameObject.GetComponent<PlayerCatalyst>();
            clonePlayerCatalyst.catalystEnd = playerCatalyst.catalystEnd;
            clonePlayerCatalyst.catalyst = playerCatalyst.catalyst;

            PlayerSpeed clonePlayerSpeed = spawnClone.GetComponent<PlayerSpeed>();
            PlayerSpeed playerSpeed = gameObject.GetComponent<PlayerSpeed>();
            clonePlayerSpeed.speedEnd = playerSpeed.speedEnd;
            clonePlayerSpeed.speed = playerSpeed.speed;

            cloneActive = true;
            StartCoroutine(Clone(cloneDuration, spawnClone));
            //Debug.Log("Getting here");
            Destroy(spawnClone, cloneDuration);
        }

        if (ult == "beam" && charges > 0 && !beamActive)
        {
            beamCD = Time.time + beamCDI;
            if (charges == maxCharges)
                FindNextCharge();
            charges--;

            //audio
            audio.PlaySFX(audio.light);
            if (firstUlt)
            {
                FindNextCharge();
                firstUlt = false;
            }
            //beamCD = Time.time + beamCDI;

            if (!Beam.activeInHierarchy)
            {
                Beam.SetActive(true);
            }
            StartCoroutine(BeamOverTime(beamChargeTime));
        }
    }


    IEnumerator ScaleOverTime(float time)
    {
        Vector2 originalScale = new Vector2(aoeStart, aoeStart);
        Vector2 destinationScale = new Vector2(aoeSize, aoeSize);
        AoE.transform.localScale = originalScale;

        float currentTime = 0f;

        do
        {
            AoE.transform.localScale = Vector2.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime < time);

        AoE.transform.localScale = originalScale;
        AoEactive = false;
        AoE.SetActive(false);
    }

    IEnumerator BeamOverTime(float time)
    {

        Color beamColor = Beam.GetComponent<SpriteRenderer>().color;
        float beamAlpha = beamColor.a;
        //Color target = new Color(beamColor.r, beamColor.g, beamColor.b, 1f);
        float currentTime = 0f;


        do
        {
            Beam.transform.localScale = Vector2.Lerp(beamStart, beamSize, currentTime / time);
            Beam.transform.localPosition = Vector2.Lerp(beamStartPos, (new Vector2(0f, beamSize.y) * beamOffet + beamStartPos), currentTime / time);
            Beam.transform.localPosition = Beam.transform.localPosition + new Vector3(0, 0, -2);

            if (beamColor.a > 0.5f && !beamActive)
            {
                beamActive = true;
                beamEnd = Time.time + beamDuration;

                Debug.Log("Beam Active");
            }

            beamColor.a = Mathf.Lerp(beamAlpha, 1f, currentTime / time);
            Beam.GetComponent<SpriteRenderer>().color = beamColor;
            //Debug.Log(beamColor.a);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime < time);
    }

    IEnumerator BeamTickDamage(float time)
    {
        //float currentTime = 0f;


        yield return new WaitForSeconds(time);

        //Beam.GetComponent<BoxCollider2D>().isTrigger = false;
        Beam.GetComponent<Collider2D>().enabled = false;
    }

    void FindNextCharge()
    {
        if(ultimate == "aoe")
        {
            nextCharge = Time.time + aoeCDI;
        }
        else if (ultimate == "clone")
        {
            nextCharge = Time.time + cloneCDI;
        }
        else if (ultimate == "beam")
        {
            nextCharge = Time.time + beamCDI;
        }
    }

    IEnumerator Clone(float time, GameObject clone)
    {
        //Debug.Log("Clone start");
        yield return new WaitForSeconds(time);
        //Debug.Log("Clone Waited");
        //Destroy(clone);
        cloneActive = false;
    }
}
