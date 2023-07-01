using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour
{

    [Header ("Scripts and Prefabs")]
    public Shooting shooting;
    public GameObject clone;
    public PlayerHealth playerHealth;

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

    [Header ("Ultimate Charges")]
    public int maxCharges = 1;
    public int charges = 1;

    [Header("Selected Ultimate")]
    public string ultimate = "aoe";

    //script specific
    GameObject AoE;

    GameObject Beam;
    bool beamActive;
    float beamEnd = 0f;
    

    Color beamColor;
    float beamAlpha;
    Vector2 beamStartPos;
    float beamTick = 0f;

    float nextCharge = 0f;
    bool firstUlt = true;

    GameObject loadout;
    //BoxCollider2D beamCollider;
    void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();

        shooting = GetComponent<Shooting>();

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
            if (Input.GetButtonDown("Jump"))
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
                Beam.GetComponent<BoxCollider2D>().enabled = true;
                //Beam.GetComponent<BoxCollider2D>().isTrigger = true;

                beamTick = Time.time + beamTickSpeed;

                StartCoroutine(BeamTickDamage(beamTickDuration));
            }
        }
    }

    void FireUltimate(string ult)
    {
        if (ult == "aoe" && charges > 0)
        {
            charges--;
            if(firstUlt)
            {
                FindNextCharge();
                firstUlt = false;
            }

            //aoeCD = Time.time + aoeCDI;

            if (!AoE.activeInHierarchy)
            {
                AoE.SetActive(true);
            }
            StartCoroutine(ScaleOverTime(aoeTime));
        }

        if (ult == "clone" && charges > 0)
        {
            charges--;
            if (firstUlt)
            {
                FindNextCharge();
                firstUlt = false;
            }
            //cloneCD = Time.time + cloneCDI;

            GameObject spawnClone = Instantiate(clone, gameObject.transform.position + cloneSpawnPos, gameObject.transform.rotation);

            //change clone loadout to player loadout
            spawnClone.GetComponent<Shooting>().Fire1 = shooting.Fire1;
            spawnClone.GetComponent <Shooting>().Fire2 = shooting.Fire2;

            Destroy(spawnClone, cloneDuration);
        }

        if (ult == "beam" && charges > 0)
        {
            charges--;
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
            Beam.transform.localPosition = Vector2.Lerp(beamStartPos, (new Vector2(0f, beamSize.y) + beamStartPos) / 2, currentTime / time);

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
        Beam.GetComponent<BoxCollider2D>().enabled = false;
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
}
