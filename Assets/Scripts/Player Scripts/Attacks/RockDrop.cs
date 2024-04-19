using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDrop : MonoBehaviour
{
    public Shooting shooting;
    float timeToLive;

    //public PlayerCatalyst playerCatalyst;

    Vector2 destinationScale = new Vector2(1f, 1f);
    // Start is called before the first frame update
    void Start()
    {
        timeToLive = shooting.rockTime;
        //Debug.Log(timeToLive);

        /*if(playerCatalyst == null )
        {
            Debug.Log("RockDrop no cat");
        }
        else*/
        if (shooting.playerCatalyst.catalyst)
        {
            destinationScale *= shooting.playerCatalyst.catalystFactor;
        }

        StartCoroutine(ScaleOverTime(timeToLive));
        Destroy(gameObject, timeToLive + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ScaleOverTime(float time)
    {
        Vector2 originalScale = gameObject.transform.localScale;
        Debug.Log(destinationScale);

        float currentTime = 0f;

        do
        {
            gameObject.transform.localScale = Vector2.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            //Debug.Log(gameObject.transform.localScale);
            yield return null;
        } while (currentTime < time);
    }
}
