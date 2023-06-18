using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public float scriptStop = 0.01f;
    public Shooting shooting;
    public int damage;
    int boostedDamage;
    void Start()
    {
        boostedDamage = (int)(damage * shooting.playerCatalyst.catalystFactor);
        StartCoroutine(Stop(scriptStop));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
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
    }

    IEnumerator Stop(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("Goodbye");
        Destroy(this);
    }
}
