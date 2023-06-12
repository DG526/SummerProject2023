using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour
{
    public PlayerHealth playerHealth;

    //AoE cooldown
    public float aoeCDI = 20;
    public float aoeCD = 0f;

    //End radius of AoE
    public float aoeSize = 10f;

    //Beginning radius of AoE
    public float aoeStart = 0.5f;

    //AoE explosion speed (lower the faster)
    public float aoeTime = 1f;

    GameObject AoE;
    //bool aoeActive = false;

    public string ultimate = "aoe";
    void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();

        foreach(Transform child in gameObject.transform)
        {
            if(child.gameObject.name != "AoE Ult")
            {
                continue;
            }

            AoE = child.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerHealth.dead)
        {
            if (Input.GetButtonDown("Jump"))
            {
                FireUltimate(ultimate);
            }
        }
    }

    void FireUltimate (string ult)
    {
        if(ult == "aoe" && Time.time > aoeCD)
        {
            aoeCD = Time.time + aoeCDI;

            if(!AoE.activeInHierarchy)
            {
                AoE.SetActive(true);
            }
            StartCoroutine(ScaleOverTime(aoeTime));
        }
    }


    IEnumerator ScaleOverTime(float time)
    {
        Vector2 originalScale = new Vector2(aoeStart, aoeStart);
        Vector2 destinationScale = new Vector2 (aoeSize,aoeSize);
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
}
