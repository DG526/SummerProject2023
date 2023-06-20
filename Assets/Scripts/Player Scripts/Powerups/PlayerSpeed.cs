using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour
{
    //speed buff active?
    public bool speed = false;

    //movement speed buff increase
    public float speedUp = 2.5f;

    //fire rate increase
    public float fireSpeedUp = 0.5f;

    //speed buff duration
    public float speedDuration = 10f;

    //stop speed buff
    public float speedEnd = 0f;

    // Update is called once per frame
    void Update()
    {
        if(speed && speedEnd < Time.time)
        {
            speed = false;
        }
    }
}
