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
    // Start is called before the first frame update
    void Start()
    {
        //Find player
        if (!player)
            player = GameObject.Find("Player");

        //Find rb
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
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
        rb.MovePosition(rb.position + (Vector2)transform.up * speed * Time.fixedDeltaTime);
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
