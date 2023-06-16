using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private Rigidbody2D rb;
    public float lifetime = 5f;
    //determines how fast poison does damage
    public float tickSpeed = 0.3f;
    public float tickDuration = 0.1f;

    float tick = 0f;
    CircleCollider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if(Time.time > tick)
        {
            tick = Time.time + tickSpeed;
            col.enabled = true;
            StartCoroutine(TickDamage(tickDuration));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 0)
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 0)
        {
            rb.velocity = Vector3.zero;
        }

        if(collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
        {
            Debug.Log("Hit an enemy");
        }
    }

    IEnumerator TickDamage(float time)
    {
        yield return new WaitForSeconds(time);
        
        col.enabled = false;
    }
}