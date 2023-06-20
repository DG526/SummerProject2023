using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    //was maxHealth
    public int numOfHearts;
    public int health;
    public bool dead = false;
    public bool grace = false;
    public float graceDuration = 1f;
    private float graceTime = 0f;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    void Start()
    {
        health = numOfHearts;
    }
    void Update()
    {
        if (gameObject.name.Equals("Player Clone(Clone)"))
            return;
        if (grace && graceTime < Time.time)
        {
            grace = false;
        }

        if (!dead && health <= 0)
        {
            dead = true;
            Debug.Log("You Died!");
            GameObject.Find("GameOver").GetComponent<GameOver>().Lose();
        }

        #region Health
        if (health > numOfHearts)
        {
            health = numOfHearts;
        }

        //Health
        for (int i = 0; i < hearts.Length; i++)
        {
            if(i <health)
            {
                hearts[i].sprite = fullHeart;
            }

            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }

            else
            {
                hearts[i].enabled = false;
            }
        }
        #endregion
    }

    public void Damage(int damage)
    {
        if (gameObject.name.Equals("Player Clone(Clone)"))
            return;
        if (!dead && !grace)
        {
            health -= damage;
            Debug.Log("You took " + damage + " damage!");
            grace = true;
            graceTime = Time.time + graceDuration;
        }
        else
        {
            Debug.Log("Grace period active");
        }
    }

    public void resetPlayer()
    {
        health = numOfHearts;
        dead = false;
    }
}
