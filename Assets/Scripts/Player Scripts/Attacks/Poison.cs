using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison : MonoBehaviour
{
    private Rigidbody2D rb;
    public Shooting shooting;
    public int damage;
    int boostedDamage;
    public float lifetime = 5f;
    //determines how fast poison does damage
    public float tickSpeed = 0.3f;
    public float tickDuration = 0.1f;

    float tick = 0f;

    //how long enemies are slowed after coming into contact with poison
    public float slowDuration = 0.3f;
    CircleCollider2D col;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        boostedDamage = (int)(damage * shooting.playerCatalyst.catalystFactor);
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
        if (collision.gameObject.layer == 0 || collision.gameObject.layer == 14)
        {
            rb.velocity = Vector3.zero;
        }

        if(collision.gameObject.tag == "Enemy")
        {
            if(shooting.poisonUpgraded)
            {
                string name = collision.gameObject.name;
                if(name.IndexOf("Wyrm") != -1)
                {
                    WyrmHeadBehavior wyrmHeadBehavior = collision.gameObject.GetComponent<WyrmHeadBehavior>();

                    wyrmHeadBehavior.poisonDuration = Time.time + slowDuration;
                    wyrmHeadBehavior.poison = true;
                }
                else if(name.IndexOf("Drake") != -1)
                {
                    DrakeBehavior drakeBehavior = collision.gameObject.GetComponent<DrakeBehavior>();

                    drakeBehavior.poisonDuration = Time.time + slowDuration;
                    drakeBehavior.poison = true;
                }
                else if(name.IndexOf("Wyvern") != -1)
                {
                    WyvernBehavior wyvernBehavior = collision.gameObject.GetComponent<WyvernBehavior>();

                    wyvernBehavior.poisonDuration = Time.time + slowDuration;
                    wyvernBehavior.poison = true;
                }
            }

            if(shooting.playerCatalyst.catalyst)
            {
                collision.gameObject.GetComponent<EnemyHealth>().Damage(boostedDamage);
            }
            else
            {
                collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);
            }
            //Debug.Log("Hit an enemy");
        }
    }

    IEnumerator TickDamage(float time)
    {
        yield return new WaitForSeconds(time);
        
        col.enabled = false;
    }
}