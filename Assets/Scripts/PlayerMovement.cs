using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    private Vector2 moveDir;
    private Vector2 lookDir;

    public PlayerSpeed playerSpeed;
    void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        playerSpeed = gameObject.GetComponent<PlayerSpeed>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!playerHealth.dead)
        {
            GetInput();
        }
    }

    private void FixedUpdate()
    {
        if (!playerHealth.dead)
        {
            Move();
        }

        if(playerHealth.dead)
        {
            rb.velocity = Vector2.zero;
        }
    }

    void GetInput()
    {
        //movement
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(x, y).normalized;

        //looking
        if (Input.GetJoystickNames().Length > 0 && Input.GetJoystickNames()[0] == "Wireless Controller")
        {

            if (Input.GetAxisRaw("Mouse X") == 0f && Input.GetAxisRaw("Mouse Y") * -1f == 0f)
            {
                lookDir = transform.up;
            }
            else
            {
                lookDir = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y") * -1f);
            }
        }
        else
        {
            Vector3 mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            lookDir = new Vector2(mouse.x - transform.position.x, mouse.y - transform.position.y);
        }
    }


    void Move()
    {
        if(playerSpeed.speed)
        {
            //movement
            rb.velocity = new Vector2(moveDir.x * (moveSpeed * playerSpeed.speedUp), moveDir.y * (moveSpeed * playerSpeed.speedUp));
        }
        else
        {
            //movement
            rb.velocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
        }

        //looking
        transform.up = lookDir * Time.deltaTime;
    }
}
