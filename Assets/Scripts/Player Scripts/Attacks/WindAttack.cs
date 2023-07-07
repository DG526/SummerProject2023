using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttack : MonoBehaviour
{
    public Shooting shooting;
    float timeToLive;
    float force;
    public int damage;

    public float wyrmGracePeriod = 0.1f;
    public float drakeGracePeriod = 0.2f;
    public float wyvernGracePeriod = 0.2f;

    public float pushForce = 500000f;
    
    enum Enemies
    {
        Wyrm,
        Drake,
        Wyvern
    }

    public Vector2 destinationScale = new Vector2(1.5f, 0.75f);

    int boostedDamage;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timeToLive = shooting.windTTL;
        force = shooting.windForce;

        boostedDamage = (int)(damage * shooting.playerCatalyst.catalystFactor);

        if (shooting.playerCatalyst.catalyst)
        {
            destinationScale = destinationScale * shooting.playerCatalyst.catalystFactor;
        }

        if (shooting.windUpgraded)
        {
            destinationScale = destinationScale * 1.5f;
        }
        StartCoroutine(ScaleOverTime(timeToLive));

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Rigidbody2D colRB = collision.gameObject.GetComponent<Rigidbody2D>();
            #region wyrm collision
            if (collision.gameObject.name.IndexOf("Wyrm") != -1)
            {
                WyrmHeadBehavior wyrmHeadBehavior;

                if (collision.gameObject.name.IndexOf("Tail") == -1 && collision.gameObject.name.IndexOf("Segment") == -1)
                {
                    wyrmHeadBehavior = collision.gameObject.GetComponent<WyrmHeadBehavior>();

                    if (!wyrmHeadBehavior.wind)
                    {
                        wyrmHeadBehavior.windDuration = Time.time + wyrmGracePeriod;
                        wyrmHeadBehavior.wind = true;
                        
                        if (Random.Range(-1f, 1f) > 0)
                            wyrmHeadBehavior.windSkew = 0.5f;
                        else
                            wyrmHeadBehavior.windSkew = -0.5f;
                        Damage(collision.gameObject);
                    }
                }
                else
                {
                    wyrmHeadBehavior = collision.gameObject.GetComponent<WyrmSegmentBehavior>().hBehav;

                    if (!wyrmHeadBehavior.wind)
                    {
                        wyrmHeadBehavior.windDuration = Time.time + wyrmGracePeriod;
                        wyrmHeadBehavior.wind = true;

                        if (Random.Range(-1f, 1f) > 0)
                            wyrmHeadBehavior.windSkew = 0.5f;
                        else
                            wyrmHeadBehavior.windSkew = -0.5f;
                        Damage(collision.gameObject);
                    }
                }
            }
            #endregion

            if(collision.gameObject.name.IndexOf("Drake") != -1)
            {
                DrakeBehavior drakeBehavior = collision.gameObject.GetComponent<DrakeBehavior>();
                if (!drakeBehavior.wind)
                {
                    drakeBehavior.windDuration = Time.time + drakeGracePeriod;
                    drakeBehavior.wind = true;
                    if (shooting.windUpgraded)
                        colRB.AddForce(rb.velocity * pushForce * shooting.windPush);
                    else
                        colRB.AddForce(rb.velocity * pushForce);
                }
                Damage(collision.gameObject);
            }
        }
        else if(collision.gameObject.layer == 10)
        {
            Rigidbody2D colRB = collision.gameObject.GetComponent<Rigidbody2D>();
            WyvernBehavior wyvernBehavior = collision.gameObject.GetComponent<WyvernBehavior>();

            if (!wyvernBehavior.wind)
            {
                Damage(collision.gameObject);
                wyvernBehavior.windDuration = Time.time + wyvernGracePeriod;
                wyvernBehavior.wind = true;

                float push = 5f;
                if (shooting.windUpgraded)
                    push *= shooting.windPush;

                collision.gameObject.transform.position = collision.gameObject.transform.position + new Vector3(rb.velocity.normalized.x,rb.velocity.normalized.y,0f) * push;
            }
        }
    }

    void Damage(GameObject enemy)
    {
        if(shooting.playerCatalyst.catalyst)
        {
            enemy.GetComponent<EnemyHealth>().Damage(boostedDamage);
        }
        else
        {
            enemy.GetComponent<EnemyHealth>().Damage(damage);
        }

        Debug.Log(enemy.GetComponent<EnemyHealth>().health);
    }
    IEnumerator ScaleOverTime(float time)
    {
        Vector2 originalScale = gameObject.transform.localScale;
        //Vector2 destinationScale = new Vector2(1.5f, 0.75f);

        float currentTime = 0f;

        do
        {
            gameObject.transform.localScale = Vector2.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime < time);
    }
}
