using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : MonoBehaviour
{

    private Transform player;

    private Rigidbody2D rb;
    //Force with which item is pulled to the player
    public float magnetism = 3f;

    //Min distance within which items are attracted to the player
    public float threshold = 5f;

    //distance from player
    private float distance = 0f;

    //direction to player
    private Vector2 dir = Vector2.zero;

    public int maxItems;

    public int minItems;

    //death stuff

    public PlayerHealth playerHealth;

    public GameObject speedPotion;

    public float speedDropRate = 0.2f;

    public GameObject shields;

    public float shieldDropRate = 0.4f;

    public GameObject catalyst;

    public float catalystDropRate = 0.6f;

    public GameObject health;

    public float healthDropRate = 0.7f;

    public float healthIncreasedRate = 1f;

    public float maxItemSpeed;

    public float minItemSpeed;

    public float itemStopTime = 1f;
    private bool increased;

    private float gracePeriod = 0f;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (player == null)
        {
            Debug.Log("You didn't do it right brother");
        }
        rb = GetComponent<Rigidbody2D>();

        playerHealth = player.gameObject.GetComponent<PlayerHealth>();

    }

    private void Update()
    {
        if (playerHealth.health <= playerHealth.numOfHearts / 2 && !increased)
        {
            increased = true;
        }
        else if (playerHealth.health > playerHealth.numOfHearts / 2 && increased)
        {
            increased = false;
        }
    }
    void FixedUpdate()
    {
        distance = Vector2.Distance(new Vector2(player.position.x, player.position.y), new Vector2(transform.position.x, transform.position.y));

        //moves object toward player if within threshold
        if (distance <= threshold)
        {
            dir = player.position - transform.position;
            dir = dir.normalized;

            rb.AddForce(dir * magnetism);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time > gracePeriod)
        {
            Death();
            gracePeriod= Time.time + 5f;
        }
    }*/


    void Death()
    {
        Debug.Log("Doin\' the dyin\' mate");

        int items = Random.Range(minItems, maxItems);

        Debug.Log("Item num: " + items);

        for (int i = 0; i < items; i++)
        {
            float itemRange;

            GameObject item;

            float itemSpeed = Random.Range(minItemSpeed, maxItemSpeed);

            //rotation randomizer
            Vector2 dir = gameObject.transform.up;
            Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-1f,1f);

            #region Item Select
            if (increased)
            {
                itemRange = Random.Range(0f, healthIncreasedRate);
            }
            else
            {
                itemRange = Random.Range(0f, healthDropRate);
            }

            if (itemRange <= shieldDropRate)
            {
                if (itemRange > speedDropRate)
                {
                    item = Instantiate(shields, gameObject.transform.position, gameObject.transform.rotation);
                    item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                    StartCoroutine(StopMovement(item));
                }
                else
                {
                    item = Instantiate(speedPotion, gameObject.transform.position, gameObject.transform.rotation);
                    item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                    StartCoroutine(StopMovement(item));
                }
            }
            else
            {
                if (itemRange > catalystDropRate)
                {
                    item = Instantiate(health, gameObject.transform.position, gameObject.transform.rotation);
                    item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                    StartCoroutine(StopMovement(item));
                }
                else
                {
                    item = Instantiate(catalyst, gameObject.transform.position, gameObject.transform.rotation);
                    item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                    StartCoroutine(StopMovement(item));
                }
            }
            #endregion
        }
    }

    IEnumerator StopMovement(GameObject item) 
    {
        yield return new WaitForSeconds(itemStopTime);
        if(item)
        item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
