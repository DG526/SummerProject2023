using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            Debug.Log("Hit an Enemy!");
        }
    }
}
