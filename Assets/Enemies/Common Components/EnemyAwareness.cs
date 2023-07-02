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
    MonoBehaviour behaviour;
    // Start is called before the first frame update
    void Start()
    {
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
        
    }
    private void OnBecameVisible()
    {
        behaviour.enabled = true;
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().enabled = true;
    }
}
