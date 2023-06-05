using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private CapsuleCollider2D col;
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            rb.velocity = Vector2.zero;
            
            //col.enabled = false;
            

            PlayerHealth script = collision.gameObject.GetComponent<PlayerHealth>();
            if (!script.grace && !script.dead)
            {
                script.Damage(1);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            //col.enabled = true;
        }
    }
}
