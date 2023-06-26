using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DragonColor
{
    RED
}

public enum DragonAction
{
    IDLE,
    IDLE_TRACKING,
    WALKING_F,
    WALKING_B,
    ATK_CLAW_L,
    ATK_CLAW_R,
    ATK_BITE_L,
    ATK_BITE_R,
    ATK_BREATH_F
}

public class DragonBehavior : MonoBehaviour
{
    public DragonColor color;
    public int maxHealth, health;
    public float moveSpeed, rotationSpeed;
    public DragonAction action;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            action = DragonAction.IDLE;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            action = DragonAction.WALKING_F;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            action = DragonAction.ATK_CLAW_L;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            action = DragonAction.ATK_CLAW_R;
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            action = DragonAction.ATK_BITE_L;
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            action = DragonAction.ATK_BITE_R;
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            action = DragonAction.ATK_BREATH_F;

        GetComponent<Animator>().SetInteger("Action", (int)action);
    }
    private void FixedUpdate()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        GetComponent<PolygonCollider2D>().isTrigger = true;

    }
}
