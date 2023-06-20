using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : MonoBehaviour
{
    public bool active = false;

    public float shieldDuration = 5f;

    public float shieldEnd = 0f;

    public float rotationSpeed = 5f;
    // Update is called once per frame
    void Update()
    {
        if (active)
        {
                transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }

        if(active && Time.time > shieldEnd)
        {
            active = false;
            gameObject.SetActive(false);
        }
    }
}
