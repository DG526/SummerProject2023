using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    GameObject redirection;
    bool hasParts;
    GameObject[] partsToDestroy;
    public int maxHealth;
    public float multiplier = 1;
    public int health;

    public GameOver gameOver;
    SpawnController spawnControl;
    public GameObject spawner;

    //for drops
    PlayerHealth playerHealth;

    //money drops
    [Header ("Gem Prefabs")]
    public GameObject gem1;
    public GameObject gem2;

    //how many gems(coins) drop on death
    [Header ("Number of Gems to Drop")]
    public int maxCoins = 4;
    public int minCoins = 1;

    //money drop rate
    [Header ("Gem Drop Rates")]
    public float gem1Drop = 0.75f;
    public float gem2Drop = 1f;

    //random.range from 0-1. If that is higher than this number, items will drop
    public float itemRate = 0.9f;

    //item drops
    [Header ("Item Prefabs")]
    public GameObject speed;
    public GameObject shield;
    public GameObject catalyst;
    public GameObject heart;
    public GameObject chest;

    [Header ("Number of Items to Drop")]
    public int maxItems = 2;
    public int minItems = 1;

    //item drop rates
    [Header ("Item Drop Rates")]
    public float speedDropRate = 0.2f;
    public float shieldDropRate = 0.4f;
    public float catalystDropRate = 0.6f;
    public float heartDropRate = 0.7f;
    public float heartIncreasedRate = 1f;

    [Header ("Item Move Speeds")]
    public float maxItemSpeed = 10f;
    public float minItemSpeed = 5f;
    public float itemStopTime = 1f;

    private bool increased = false;
    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Mathf.Max(1,(int)(maxHealth * multiplier));
        health = maxHealth;

        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

        spawnControl = GameObject.Find("SpawnController").GetComponent<SpawnController>();

        gameOver = GameObject.Find("GameOver").GetComponent<GameOver>();
    }

    // Update is called once per frame
    void Update()
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
    public void Damage(int damage)
    {
        if (redirection)
        {
            redirection.GetComponent<EnemyHealth>().Damage(damage);
            return;
        }
        health -= damage;
        //Debug.Log("An enemy took damage!");
        if (health <= 0)
        {
            if(!dead)
                Death();
            if(hasParts)
            {
                foreach(var part in partsToDestroy)
                {
                    Destroy(part);
                }
            }
            Destroy(gameObject);
        }
    }
    public void SetParts(GameObject[] parts)
    {
        partsToDestroy = parts;
        hasParts = true;
    }
    public void SetRedirection(GameObject parent)
    {
        redirection = parent;
    }

    public void Death()
    {
        gameOver.defeated += 1;

        dead = true;
        if(spawner != null)
        {
            spawner.GetComponent<Spawner>().Remove();
        }
        if(gameObject.name.IndexOf("chest") == -1 && gameObject.name.IndexOf("spawn") == -1)
        spawnControl.enemyRemove();

        if (gameObject.name.IndexOf("boss") != -1)
        {
            Debug.Log("Getting to chest");
            GameObject thing = Instantiate(chest, gameObject.transform.position, Quaternion.identity);
        }

        int coins = (int)(Random.Range(minCoins, maxCoins));
        //Debug.Log(gameObject.name);
        //Debug.Log("Coins: " + coins);
        for (int i = 0; i < coins; i++)
        {
            float coinRange = Random.Range(0f, gem2Drop);
            GameObject coin;
            float coinSpeed = Random.Range(minItemSpeed, maxItemSpeed);
            Vector2 dir = gameObject.transform.up;
            Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-1f, 1f);

            if (coinRange <= gem1Drop)
            {
                coin = Instantiate(gem1, gameObject.transform.position, Quaternion.identity);
                coin.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * coinSpeed;
                coin.GetComponent<Item>().StartCoroutine(coin.GetComponent<Item>().StopMovement(coin, itemStopTime));
            }
            else
            {
                coin = Instantiate(gem2, gameObject.transform.position, Quaternion.identity);
                coin.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * coinSpeed;
                coin.GetComponent<Item>().StartCoroutine(coin.GetComponent<Item>().StopMovement(coin, itemStopTime));
            }
        }

        if(Random.Range(0f,1f) >= itemRate)
        {
            int items = (int)(Random.Range(minItems, maxItems));

            for(int i = 0; i < items; i++)
            {
                float itemRange;
                GameObject item;
                float itemSpeed = Random.Range(minItemSpeed, maxItemSpeed);
                Vector2 dir = gameObject.transform.up;
                Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-1f, 1f);

                #region Item Select
                if (increased)
                {
                    itemRange = Random.Range(0f, heartIncreasedRate);
                }
                else
                {
                    itemRange = Random.Range(0f, heartDropRate);
                }

                if (itemRange <= shieldDropRate)
                {
                    if (itemRange > speedDropRate)
                    {
                        item = Instantiate(shield, gameObject.transform.position, Quaternion.identity);
                        item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                        item.GetComponent<Item>().StartCoroutine(item.GetComponent<Item>().StopMovement(item, itemStopTime));
                    }
                    else
                    {
                        item = Instantiate(speed, gameObject.transform.position, Quaternion.identity);
                        item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                        item.GetComponent<Item>().StartCoroutine(item.GetComponent<Item>().StopMovement(item, itemStopTime));
                    }
                }
                else
                {
                    if (itemRange > catalystDropRate)
                    {
                        item = Instantiate(heart, gameObject.transform.position, Quaternion.identity);
                        item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                        item.GetComponent<Item>().StartCoroutine(item.GetComponent<Item>().StopMovement(item, itemStopTime));
                    }
                    else
                    {
                        item = Instantiate(catalyst, gameObject.transform.position, Quaternion.identity);
                        item.GetComponent<Rigidbody2D>().velocity = (dir + pdir).normalized * itemSpeed;
                        item.GetComponent<Item>().StartCoroutine(item.GetComponent<Item>().StopMovement(item, itemStopTime));
                    }
                }
                #endregion
            }
        }
    }
}
