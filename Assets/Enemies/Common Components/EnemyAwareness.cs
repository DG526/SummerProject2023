using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum AwarenessType
{
    HuntAtStart,
    HuntWhenOnScreen
}
public class EnemyAwareness : MonoBehaviour
{
    public AwarenessType awarenessType;
    public HUD hud;
    bool sighted = false;
    float killtime = 10f;
    MonoBehaviour behaviour;
    // Start is called before the first frame update
    void Start()
    {
        hud = GameObject.Find("HUD").GetComponent<HUD>();

        if(GetComponent<WyrmHeadBehavior>() != null)
        {
            behaviour = GetComponent<WyrmHeadBehavior>();
        }
        else if(GetComponent<DrakeBehavior>() != null)
        {
            behaviour = GetComponent<DrakeBehavior>();
        }
        else if(GetComponent<WyvernBehavior>() != null)
        {
            behaviour = GetComponent<WyvernBehavior>();
        }
        else if(GetComponent<DragonBehavior>() != null)
        {
            behaviour = GetComponent<DragonBehavior>();
        }
        else if(GetComponent<DarkLightDragonBehavior>() != null)
        {
            behaviour = GetComponent<DarkLightDragonBehavior>();
        }
        if(awarenessType != AwarenessType.HuntAtStart)
        {
            behaviour.enabled = false;
            if (GetComponent<Animator>() != null)
                GetComponent<Animator>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!sighted && awarenessType == AwarenessType.HuntAtStart)
        {
            killtime -= Time.deltaTime;
            if(killtime < 0)
            {
                foreach(GameObject p in GetComponent<EnemyHealth>().partsToDestroy)
                {
                    Destroy(p);
                }
                Destroy(gameObject);
            }
        }
    }
    private void OnBecameVisible()
    {
        sighted = true;
        //if the gameobject is a dragon
        if (behaviour.gameObject.name.IndexOf("Dragon") != -1)
        {
            hud.isHealthBar = true;
        }

        behaviour.enabled = true;
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().enabled = true;
    }
}
