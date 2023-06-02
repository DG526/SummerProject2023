using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
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

            rb.AddForce(dir * magnetism);
        }
    }

    //should make it ignore collisions with anything other than players
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
