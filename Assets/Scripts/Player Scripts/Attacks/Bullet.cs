using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public Shooting shooting;
    public int damage;
    int boostedDamage;
    void Start()
    {
        boostedDamage = (int)(damage * shooting.playerCatalyst.catalystFactor);
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
        {

        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
        {
            if (shooting.playerCatalyst.catalyst)
            {
                collision.gameObject.GetComponent<EnemyHealth>().Damage(boostedDamage);
            }
            else
            {
            collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);
            }
        }
        Destroy(gameObject); 
    }
}
