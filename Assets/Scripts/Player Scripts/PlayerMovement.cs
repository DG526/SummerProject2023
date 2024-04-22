using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    private Vector2 moveDir;
    private Vector2 lookDir;

    public PlayerSpeed playerSpeed;

    public InputActionAsset inputs;
    InputAction IAMove, IAMouseLook, IAControllerLook;

    GameObject loadout;

    private void Awake()
    {
        IAMove = inputs.FindAction("Level Actions/Movement");
        IAControllerLook = inputs.FindAction("Level Actions/Look1");
        IAMouseLook = inputs.FindAction("Level Actions/Look2");
    }
    void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        playerSpeed = gameObject.GetComponent<PlayerSpeed>();
        loadout = GameObject.Find("LoadOut");
    }

    private void OnEnable()
    {
        IAMove.Enable();
        IAMouseLook.Enable();
        IAControllerLook.Enable();
    }
    private void OnDisable()
    {
        if (tag == "Player Clone")
            return;
        IAMove.Disable();
        IAMouseLook.Disable();
        IAControllerLook.Disable();
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
        if (playerHealth.dead)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (loadout != null && loadout.activeInHierarchy)
            return;

        Move();
    }

    void GetInput()
    {
        //movement
        moveDir = Vector2.zero;
        if (IAMove.ReadValue<Vector2>() != Vector2.zero)
        {
            moveDir = IAMove.ReadValue<Vector2>();
        }

        //looking
        if (IAControllerLook.triggered)
            lookDir = IAControllerLook.ReadValue<Vector2>();
        else if (IAMouseLook.triggered)
        {
            Vector3 mouse = Input.mousePosition;
            mouse = Camera.main.ScreenToWorldPoint(mouse);
            lookDir = new Vector2(mouse.x - transform.position.x, mouse.y - transform.position.y);
        }
    }
    /* // OLD INPUT METHOD
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
    */

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
