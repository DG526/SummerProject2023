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
    float timeLived = 0;
    Vector3 startScale;
    void Start()
    {
        if(GameObject.Find("Map") != null)
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
        if(behavior == EnemyProjectileType.Poison)
        {
            if(timeLived > 3)
                Destroy(gameObject);
            else
            {
                Color baseCol = GetComponent<SpriteRenderer>().color;
                GetComponent<SpriteRenderer>().color = new Color(baseCol.r, baseCol.g, baseCol.b, Mathf.Clamp01(1-timeLived));
                transform.localScale = startScale * (1 + timeLived);
            }
        }

        timeLived += Time.deltaTime;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12 || collision.gameObject.layer == 14 || collision.gameObject.layer == 0)
        {
            if(behavior != EnemyProjectileType.Poison)
                Destroy(gameObject);
            else
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        if (collision.gameObject.layer == 3 && GetComponent<SpriteRenderer>().color.a >  0.1f)
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(1);
            if(behavior == EnemyProjectileType.Standard)
                Destroy(gameObject);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && behavior == EnemyProjectileType.Poison && GetComponent<SpriteRenderer>().color.a > 0.1f)
        {
            collision.gameObject.GetComponent<PlayerHealth>().Damage(1);
        }
    }
}
