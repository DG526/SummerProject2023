using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public PolygonCollider2D map;
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 || collision.gameObject.layer == 14 || collision.gameObject.layer == 0)
            Destroy(gameObject);
        if (collision.gameObject.layer == 3)
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(1);
            Destroy(gameObject);
        }

    }
}
