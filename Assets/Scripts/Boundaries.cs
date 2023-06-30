using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public PolygonCollider2D map;
    public GameObject player;

    public float offset = 10f;
    //public bool check;
    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("Map").GetComponent<PolygonCollider2D>();
        player = GameObject.Find("Player");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject.name);
        int layer = collision.gameObject.layer;
        //player       monster       flying monster
        if (layer == 3 || layer == 8 || layer == 10)
        {

            if(layer == 8 || layer == 10)
            {
                collision.gameObject.transform.up = new Vector2(player.transform.position.x - collision.gameObject.transform.position.x, player.transform.position.y - collision.gameObject.transform.position.y);
            }

            Vector2 up = new Vector2(player.transform.position.x - collision.gameObject.transform.position.x, player.transform.position.y - collision.gameObject.transform.position.y);

            RaycastHit2D[] playerHit = Physics2D.RaycastAll(player.transform.position, up * -1, 100f, 3);

            foreach (RaycastHit2D i in playerHit) 
            {
                if (i.collider == map)
                    continue;
                //Debug.Log(i.transform.name);
                //Debug.Log(i.distance);
                RaycastHit2D hit = i;
                float distance = hit.distance - offset;
                collision.gameObject.transform.position = player.transform.position + new Vector3(-1f * up.x, -1f* up.y, 0f).normalized * distance;
                break;
            }
        }
    }
}
