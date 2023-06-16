using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int health;
    public bool dead = false;
    public bool grace = false;
    public float graceDuration = 1f;
    private float graceTime = 0f;

    void Start()
    {
        health = maxHealth;
    }
    void Update()
    {
        if (grace && graceTime < Time.time)
        {
            grace = false;
        }

        if (!dead && health <= 0)
        {
            dead = true;
            Debug.Log("You Died!");
        }
    }

    public void Damage(int damage)
    {
        health -= damage;
        Debug.Log("You took " + damage + " damage!");

        grace = true;
        graceTime = Time.time + graceDuration;
    }

    public void resetPlayer()
    {
        health = maxHealth;
        dead = false;
    }
}
