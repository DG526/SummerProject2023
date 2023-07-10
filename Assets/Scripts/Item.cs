using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Transform player;

    private Rigidbody2D rb;

    [Header ("General")]
    public float magnetism = 3f;
    public float threshold = 5f;

    //distance from player
    private float distance = 0f;

    //direction to player
    private Vector2 dir = Vector2.zero;

    [Header ("Health Item")]
    public PlayerHealth playerHealth;
    public int healing = 1;

    [Header ("Player Scripts")]
    public PlayerSpeed playerSpeed;
    public PlayerCatalyst playerCatalyst;
    public PlayerPoints playerPoints;

    [Header("Gems")]
    public int gem1val = 10;
    public int gem2val = 50;

    bool moving = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;

        if (player == null)
        {
            Debug.Log("You didn't do it right brother");
        }
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(new Vector2(player.position.x, player.position.y), new Vector2(transform.position.x, transform.position.y));

        //moves object toward player if within threshold
        if (distance <= threshold)
        {
            dir = player.position - transform.position;
            dir = dir.normalized;

            rb.velocity = (dir * magnetism);
        }
        else if(distance > threshold && !moving) 
        { 
            rb.velocity = Vector2.zero;
        }
}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (gameObject.tag == "Healing Heart")
            {
                playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth.health < playerHealth.numOfHearts)
                {
                    playerHealth.health = playerHealth.health + 1;
                    Debug.Log(playerHealth.health);
                }
            }
            else if (gameObject.tag == "Speed Potion")
            {
                playerSpeed = collision.gameObject.GetComponent<PlayerSpeed>();
                playerSpeed.speed = true;
                playerSpeed.speedEnd = Time.time + playerSpeed.speedDuration;
                Debug.Log("You got a speed potion!");
            }
            else if(gameObject.tag == "Catalyst")
            {
                playerCatalyst = collision.gameObject.GetComponent<PlayerCatalyst>();
                playerCatalyst.catalyst = true;
                
                playerCatalyst.catalystEnd = Time.time + playerCatalyst.catalystDuration;
                Debug.Log("You picked up a magic catalyst!");
            }
            else if(gameObject.tag == "Shield Item")
            {
                foreach(Transform child in collision.gameObject.transform)
                {
                    //Debug.Log(child.name);

                    if(child.name != "Shields")
                    {
                        continue;
                    }

                    if(!child.gameObject.activeInHierarchy)
                    {
                        child.gameObject.SetActive(true);
                    }

                    Shields shields = child.GetComponent<Shields>();
                    shields.active = true;
                    shields.shieldEnd = Time.time + shields.shieldDuration;
                }
            }
            else if (gameObject.name.IndexOf("Gem1") != -1)
            {
                playerPoints = collision.gameObject.GetComponent<PlayerPoints>();
                playerPoints.AddPoints(gem1val);
            }
            else if (gameObject.name.IndexOf("Gem2") != -1)
            {
                playerPoints = collision.gameObject.GetComponent<PlayerPoints>();
                playerPoints.AddPoints(gem2val);
            }
            Destroy(gameObject);
        }
    }

    public IEnumerator StopMovement(GameObject item, float time)
    {
        moving = true;
        yield return new WaitForSeconds(time);
        item.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        moving = false;
    }
}
