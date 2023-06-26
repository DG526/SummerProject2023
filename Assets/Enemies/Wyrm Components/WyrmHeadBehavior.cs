using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class WyrmHeadBehavior : MonoBehaviour
{
    public GameObject segTemplate, endTemplate;
    public int tailLength;
    public GameObject[] segments;

    public GameObject player;
    public float maxSegAngle = 40;//Max angle between segments
    public float speed = 1.4f;

    Rigidbody2D rb;

    public bool wind;
    public float windDuration;
    public float windSkew;
    // Start is called before the first frame update
    void Start()
    {
        //Find player
        if (!player)
            player = GameObject.Find("Player");

        //Find rb
        rb = GetComponent<Rigidbody2D>();

        //Create segments
        segments = new GameObject[tailLength];
        float dist = 0.4f;
        segments[0] = Instantiate(segTemplate, transform.position + new Vector3(0, -dist, 0.001f), new Quaternion());
        for (int i = 1; i < tailLength-1; i++)
        {
            segments[i] = Instantiate(segTemplate, transform.position + new Vector3(0, -dist, 0.001f * (i + 1)), new Quaternion());
            dist += 0.4f * transform.localScale.y;
        }
        segments[tailLength-1] = Instantiate(endTemplate, transform.position + new Vector3(0, -dist, 0.001f * tailLength), new Quaternion());

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), segments[0].GetComponent<Collider2D>());
        //Initialize segments
        for (int i = 0; i < tailLength;i++)
        {
            segments[i].GetComponent<WyrmSegmentBehavior>().head = gameObject;
            if (i == 0)
                segments[i].GetComponent<WyrmSegmentBehavior>().following = gameObject;
            else
            {
                segments[i].GetComponent<WyrmSegmentBehavior>().following = segments[i - 1];
                Physics2D.IgnoreCollision(segments[i - 1].GetComponent<Collider2D>(), segments[i].GetComponent<Collider2D>());
            }
            if (i < tailLength - 1)
                segments[i].GetComponent<WyrmSegmentBehavior>().nextSeg = segments[i + 1];
            segments[i].GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            segments[i].transform.localScale = transform.localScale;
            segments[i].GetComponent<EnemyHealth>().SetRedirection(gameObject);
        }
        GetComponent<EnemyHealth>().SetParts(segments);
    }

    // Update is called once per frame
    void Update()
    {
        //wind must be turned off here because the wind gameobject is destoryed before the grace period is supposed to end
        if(wind && windDuration < Time.time)
        {
            wind = false;
        }
    }
    private void FixedUpdate()
    {
        
        if(Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y)) > 0.5f)
            Slither();
    }

    void Slither()
    {
        
        transform.up = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        transform.up.Normalize();
        if (wind)
        {
            //reverse direction of movement
            transform.up *= -1;
            //slightly skew direction to avoid other segments
            transform.up = new Vector3(transform.up.x + windSkew, transform.up.y, transform.up.z);
            //increase speed of movement to avoid other segments as well as make the wind feel more effective
            rb.MovePosition(rb.position + (Vector2)transform.up * speed * 3f * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
        }
        segments[0].GetComponent<WyrmSegmentBehavior>().Slither();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") && !collision.gameObject.GetComponent<PlayerHealth>().grace)
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(1);
        }
    }private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player") && !collision.gameObject.GetComponent<PlayerHealth>().grace)
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(1);
        }
    }
}
