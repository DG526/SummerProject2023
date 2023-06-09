using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    //public float lifeTime = 5f;
    void Start()
    {
        //if lifetime is zero, the bullet will go on forever.
        /*if(lifeTime != 0f)
        { 
            Destroy(gameObject, lifeTime);
        }*/
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {

        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!(collision.gameObject.layer == 3))
        {
            Destroy(gameObject);
        }
        
    }
}
