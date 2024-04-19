using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

public class AoETouch : MonoBehaviour
{
    public int damage;
    public float reset = 2.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.layer == 8 || obj.layer == 10)
        {
            if (obj.name.IndexOf("DarkLight") != -1)
            {
                if (!obj.GetComponent<DarkLightDragonBehavior>().AoEHit)
                {
                    obj.GetComponent<DarkLightDragonBehavior>().AoEHit = true;
                    obj.GetComponent<DarkLightDragonBehavior>().AoEReset = Time.time + reset;
                    collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);
                    Debug.Log("Doing damage to Dark Light");
                }
            }
            else if (obj.name.IndexOf("Dragon") != -1)
            {
                if (!obj.GetComponent<DragonBehavior>().AoEHit)
                {
                    obj.GetComponent<DragonBehavior>().AoEHit = true;
                    obj.GetComponent<DragonBehavior>().AoEReset = Time.time + reset;
                    collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);
                    Debug.Log("Doing damage to Dragon");
                }
            }
            else
            collision.gameObject.GetComponent<EnemyHealth>().Damage(damage);
        }
    }
}
