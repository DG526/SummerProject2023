using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrakeBehavior : MonoBehaviour
{
    public GameObject player;
    public float speed = 1.4f;

    Rigidbody2D rb;

    public float attackDist = 1.5f;
    public bool canAttack;
    public bool attacking;

    public bool wind = false;
    public float windDuration;

    public bool lightningStunned = false;
    public float stunDuration = 0f;

    public bool poison = false;
    public float poisonDuration = 0f;
    public float poisonSlow = 0.5f;
    float speedSlowed;

    Collider2D[] hits;
    bool inMap;
    // Start is called before the first frame update
    void Start()
    {
        //Find player
        if (!player)
            player = GameObject.Find("Player");

        //Find rb
        rb = GetComponent<Rigidbody2D>();

        speedSlowed = speed * poisonSlow;
    }

    // Update is called once per frame
    void Update()
    {
        if(wind && windDuration < Time.time)
        {
            wind = false;
        }

        if (lightningStunned && stunDuration < Time.time)
        {
            lightningStunned = false;
        }

        if (poison && poisonDuration < Time.time)
        {
            poison = false;
        }
    }
    private void FixedUpdate()
    {
        if (player.GetComponent<PlayerHealth>().dead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        hits = Physics2D.OverlapCircleAll(transform.position, 0.1f);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.name.IndexOf("Map") != -1)
            {
                inMap = true;
                break;
            }
        }
        if (!inMap)
            transform.position = transform.position + transform.up * 2f;

        if (lightningStunned)
            return;
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if(distToPlayer < attackDist * transform.localScale.y)
        {
            canAttack = true;
        }
        if(canAttack && !attacking)
        {
            Attack();
        }
        else if(!attacking)
        {
            Walk();
        }
    }
    void Walk()
    {
        transform.up = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        transform.up.Normalize();
        if(!poison)
            rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
        else
            rb.MovePosition(rb.position + (Vector2)transform.up * speedSlowed * Time.fixedDeltaTime);
    }
    void Attack()
    {
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Animator>().SetTrigger("Attacking");
    }
    public void FinishAttack()
    {
        rb.constraints = 0;
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distToPlayer < attackDist * transform.localScale.y)
            return;
        //Debug.Log("Finishing gore attack");
        GetComponent<Animator>().SetTrigger("DoneAttacking");
        attacking = false;
        canAttack = false;
    }
    public void ActivateHitbox()
    {
        //Debug.Log("Activating hitbox");
        transform.Find("Hitbox").gameObject.SetActive(true);
    }
    public void DeactivateHitbox()
    {
        //Debug.Log("Deactivating hitbox");
        transform.Find("Hitbox").gameObject.SetActive(false);
    }
}
