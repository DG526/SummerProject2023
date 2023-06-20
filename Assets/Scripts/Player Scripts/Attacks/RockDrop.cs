using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockDrop : MonoBehaviour
{
    public Shooting shooting;
    float timeToLive;

    //public PlayerCatalyst playerCatalyst;

    public Vector2 destinationScale = new Vector2(0.5f, 0.5f);
    // Start is called before the first frame update
    void Start()
    {
        timeToLive = shooting.rockTime;

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


        float currentTime = 0f;

        do
        {
            gameObject.transform.localScale = Vector2.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime < time);
    }
}
