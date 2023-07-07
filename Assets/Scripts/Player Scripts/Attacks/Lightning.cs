using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public Shooting shooting;

    private bool struck = false;
    private GameObject[] enemies;
    private List<GameObject> validEnemies = new List<GameObject>();

    //distance with which lightning will seek enemies
    public float threshold = 10f;

    public int damage = 50;

    public float stunTime = 1f;

    //number of times lightning can strike other enemies
    public int maxStrikes = 1;

    private int boostedDamage;
    private int strikes = 0;
    //distance from this object to other enemy
    private float distance;

    private bool foundTarget;
    private int targetNum;
    private GameObject target;

    private Vector2 dir;
    private float speed; 
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = shooting.lightningSpeed * 1.5f;
        boostedDamage = (int)(damage * shooting.playerCatalyst.catalystFactor);
    }

    // Update is called once per frame
    private void Update()
    {
        if (boostedDamage != (int)(damage * shooting.playerCatalyst.catalystFactor))
        {
            boostedDamage = (int)(damage * shooting.playerCatalyst.catalystFactor);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 0 || collision.gameObject.layer == 14)
        {
            Destroy(gameObject);
        }

        #region stun
        if (shooting.lightningUpgraded)
        {
            if (collision.gameObject.name.IndexOf("Wyrm") != -1)
            {
                WyrmHeadBehavior wyrmHeadBehavior;

                //find what part of wyrm we're dealing with
                if (collision.gameObject.name.IndexOf("Tail") == -1 && collision.gameObject.name.IndexOf("Segment") == -1)
                {
                    wyrmHeadBehavior = collision.gameObject.GetComponent<WyrmHeadBehavior>();

                    if (!wyrmHeadBehavior.lightningStunned)
                    {
                        wyrmHeadBehavior.stunDuration = Time.time + stunTime;
                        wyrmHeadBehavior.lightningStunned = true;
                    }
                }
                else
                {
                    wyrmHeadBehavior = collision.gameObject.GetComponent<WyrmSegmentBehavior>().hBehav;

                    if (!wyrmHeadBehavior.lightningStunned)
                    {
                        wyrmHeadBehavior.stunDuration = Time.time + stunTime;
                        wyrmHeadBehavior.lightningStunned = true;
                    }
                }
            }

            if (collision.gameObject.name.IndexOf("Drake") != -1)
            {
                DrakeBehavior drakeBehavior = collision.gameObject.GetComponent<DrakeBehavior>();
                if (!drakeBehavior.lightningStunned)
                {
                    drakeBehavior.stunDuration = Time.time + stunTime;
                    drakeBehavior.lightningStunned = true;
                }
            }

            if (collision.gameObject.layer == 10)
            {
                WyvernBehavior wyvernBehavior = collision.gameObject.GetComponent<WyvernBehavior>();

                if (!wyvernBehavior.wind)
                {
                    wyvernBehavior.stunDuration = Time.time + stunTime;
                    wyvernBehavior.lightningStunned = true;
                }
            }
        }
        #endregion

        if (struck)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                Damage(collision.gameObject);
            }
                Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Enemy" && !struck)
        {
            strikes++;
            Damage(collision.gameObject);
            if(strikes >= maxStrikes)
            {
                struck = true;
            }

            //list of all enemies
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            for (int i = 0; i < enemies.Length; i++)
            {
                distance = Vector2.Distance(gameObject.transform.position, enemies[i].transform.position);

                //if enemy is close enough, it is a valid target
                if (distance <= threshold)
                {
                    if ((enemies[i] == collision.gameObject))
                    {

                        //Debug.Log("Ignore same object");
                    }
                    else
                    {
                        //Debug.Log("Found a possible target");
                        validEnemies.Add(enemies[i]);
                        //Debug.Log(enemies[i].name);
                        foundTarget = true;
                    }
                }
            }

            if (foundTarget)
            {
                targetNum = Random.Range(0, validEnemies.Count);
                target = validEnemies[targetNum];

                //dir = new Vector2(gameObject.transform.position.x + target.transform.position.x, gameObject.transform.position.y + target.transform.position.y).normalized;
                dir = (target.transform.position - gameObject.transform.position).normalized;
                //Debug.Log(target.transform.position);
                //rb.velocity = Vector2.zero;

                //Debug.Log(dir.x + ", " + dir.y);
                gameObject.transform.up = dir;
                rb.velocity = dir * speed;
                foundTarget = false;

                enemies = null;
                validEnemies.Clear();
            }
            else
            {
                //if it didn't find another target, it has no reason to live
                //Debug.Log("Didn't find another target");
                Destroy(gameObject);
            }
        }
    }

    void Damage(GameObject enemy)
    {
        if (shooting.playerCatalyst.catalyst)
        {
            enemy.GetComponent<EnemyHealth>().Damage(boostedDamage);
        }
        else
        {
            enemy.GetComponent<EnemyHealth>().Damage(damage);
        }

        //Debug.Log(enemy.GetComponent<EnemyHealth>().health);
    }
}
