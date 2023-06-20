using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoETouch : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 8 || collision.gameObject.layer == 10)
        {
            collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);
        }
    }
}
