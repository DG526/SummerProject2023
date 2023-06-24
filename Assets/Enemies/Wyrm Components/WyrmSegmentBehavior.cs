using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyrmSegmentBehavior : MonoBehaviour
{
    public GameObject head, following, nextSeg;
    Rigidbody2D rb;
    public WyrmHeadBehavior hBehav;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hBehav = head.GetComponent<WyrmHeadBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slither()
    {
        transform.up = new Vector2(following.transform.position.x - transform.position.x, following.transform.position.y - transform.position.y);
        transform.up.Normalize();
        rb.MovePosition(this.following.transform.position - transform.up * transform.localScale.y * 0.4f);

        if (nextSeg)
            nextSeg.GetComponent<WyrmSegmentBehavior>().Slither();
    }
}
