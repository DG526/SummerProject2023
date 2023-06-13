using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    GameObject redirection;
    bool hasParts;
    GameObject[] partsToDestroy;
    public int maxHealth;
    public float multiplier = 1;
    int health;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = Mathf.Max(1,(int)(maxHealth * multiplier));
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(int damage)
    {
        if (redirection)
        {
            redirection.GetComponent<EnemyHealth>().Damage(damage);
            return;
        }
        health -= damage;
        Debug.Log("An enemy took damage!");
        if (health <= 0)
        {
            if(hasParts)
            {
                foreach(var part in partsToDestroy)
                {
                    Destroy(part);
                }
            }
            Destroy(gameObject);
        }
    }
    public void SetParts(GameObject[] parts)
    {
        partsToDestroy = parts;
        hasParts = true;
    }
    public void SetRedirection(GameObject parent)
    {
        redirection = parent;
    }
}
