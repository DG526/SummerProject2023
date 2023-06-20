using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyvernBehavior : MonoBehaviour
{
    public GameObject player;
    public float fSpeed = 1.6f, bSpeed = 1f;

    Rigidbody2D rb;
    public GameObject projectile;
    public float projectileSpeed = 3;

    public float attackDist = 7f, tooClose = 5f;
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
        if (distToPlayer > tooClose * transform.localScale.y && distToPlayer < attackDist * transform.localScale.y)
        {
            canAttack = true;
        }
        if (canAttack && !attacking)
        {
            Attack();
        }
        else if (!attacking && distToPlayer < tooClose * transform.localScale.y)
        {
            Backwards();
        }
        else if (!attacking)
        {
            Forwards();
        }
    }
    void Forwards()
    {
        transform.up = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        transform.up.Normalize();
        rb.MovePosition(rb.position + (Vector2)transform.up * fSpeed * Time.fixedDeltaTime);
    }
    void Backwards()
    {
        transform.up = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y);
        transform.up.Normalize();
        rb.MovePosition(rb.position + (Vector2)transform.up * -bSpeed * Time.fixedDeltaTime);
    }
    void Attack()
    {
        rb.freezeRotation = true;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Animator>().SetTrigger("Attacking");
    }
    void Spit()
    {
        GameObject bullet = null;
        bullet = Instantiate(projectile, transform.Find("Spitpos").position, transform.rotation);
        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

        //add spread
        Vector2 dir = transform.Find("Spitpos").up;
        //Vector2 pdir = Vector2.Perpendicular(dir);

        //pdir = Vector2.Perpendicular(dir) * Random.Range(-circleSpread, circleSpread);

        bulletRB.velocity = (dir).normalized * projectileSpeed;
        //bulletRB.velocity = firePoint.up * circleBulletForce;
        
        Destroy(bullet,4);
    }
    public void FinishAttack()
    {
        rb.constraints = 0;
        float distToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distToPlayer > tooClose * transform.localScale.y && distToPlayer < attackDist * transform.localScale.y)
            return;
        //Debug.Log("Finishing spit attack");
        GetComponent<Animator>().SetTrigger("DoneAttacking");
        attacking = false;
        canAttack = false;
    }
}
