using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindAttack : MonoBehaviour
{
    public Shooting shooting;
    float timeToLive;
    float force;

    public Vector2 destinationScale = new Vector2(1.5f, 0.75f);
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        timeToLive = shooting.windTTL;
        force = shooting.windForce;

        StartCoroutine(ScaleOverTime(timeToLive));
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            Rigidbody2D colRB = collision.gameObject.GetComponent<Rigidbody2D>();
            colRB.AddForce(rb.velocity);
        }
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