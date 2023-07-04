using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyProjectileType
{
    Standard,
    Wind,
    Poison
}
public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public PolygonCollider2D map;
    public EnemyProjectileType behavior = EnemyProjectileType.Standard;
    Vector3 startScale;
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<PolygonCollider2D>();
        startScale= transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(behavior == EnemyProjectileType.Wind)
        {
            transform.localScale *= 1 + Time.deltaTime * 2;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 || collision.gameObject.layer == 14 || collision.gameObject.layer == 0)
            Destroy(gameObject);
        if (collision.gameObject.layer == 3)
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(1);
            if(behavior == EnemyProjectileType.Standard)
                Destroy(gameObject);
        }

    }
}
