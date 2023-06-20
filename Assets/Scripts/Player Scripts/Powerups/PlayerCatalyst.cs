using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatalyst : MonoBehaviour
{
    public bool catalyst = false;

    public float catalystFactor = 1.5f;

    public float catalystDuration = 10f;

    public float catalystEnd = 0f;

    private void Start()
    {
        catalyst = false;

    }
    // Update is called once per frame
    void Update()
    {
        if(catalyst && Time.time > catalystEnd)
        {
            catalyst = false;
        }
    }

}
