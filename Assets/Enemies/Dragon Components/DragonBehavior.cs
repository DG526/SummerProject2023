using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum DragonColor
{
    RED,
    BLUE,
    GREEN,
    YELLOW,
    PURPLE
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
    ATK_BREATH_F,
    ATK_BREATH_L, ATK_BREATH_R,
    THINKING
}
public enum DragonSecondaryAction
{
    NONE,
    ROTATION_LEFT,
    ROTATION_RIGHT
}

public class DragonBehavior : MonoBehaviour
{
    public GameObject player;
    public DragonColor color;
    //public int maxHealth, health;
    public float maxMoveSpeed, rotationSpeed;
    float moveSpeed;
    int restCountdown = 2;
    public DragonAction action, plan;
    DragonAction lastPlan;
    public DragonSecondaryAction secondaryAction;
    public GameObject projectile;
    public int rage; // 0 = normal, 1 (HP < 40%) = faster movement, 2 (HP < 15%) = can spam breath. 
    float moveTimer;
    Rigidbody2D rb;
    bool usingBreathAttack;
    float breathTime; // For breath patterns based on position in time (Water)
    float breathTimer; // For breath patterns in bursts (Wind)
    int breathSwitch; // For breath patterns with variable amounts of bullets/frame (Lightning)
    bool aggro = false;

    public SetMap map;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        map = GameObject.Find("Map").GetComponent<SetMap>();
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnDestroy()
    {
        map.levelTypes.RemoveAt(map.level);
        GameObject.Find("GameOver").GetComponent<GameOver>().WinWaitStart(5);
        player.GetComponent<PlayerHealth>().health = Mathf.Min(player.GetComponent<PlayerHealth>().health + 2, player.GetComponent<PlayerHealth>().numOfHearts);
        player.GetComponent<PlayerHealth>().graceTime = Time.time + 5f;
        player.GetComponent<PlayerHealth>().grace = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //    action = DragonAction.IDLE;
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //    action = DragonAction.WALKING_F;
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //    action = DragonAction.ATK_CLAW_L;
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //    action = DragonAction.ATK_CLAW_R;
        //else if (Input.GetKeyDown(KeyCode.Alpha5))
        //    action = DragonAction.ATK_BITE_L;
        //else if (Input.GetKeyDown(KeyCode.Alpha6))
        //    action = DragonAction.ATK_BITE_R;
        //else if (Input.GetKeyDown(KeyCode.Alpha7))
        //    action = DragonAction.ATK_BREATH_F;

        //GetComponent<Animator>().SetInteger("Action", (int)action);
    }
    private void FixedUpdate()
    {
        DestroyImmediate(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        GetComponent<PolygonCollider2D>().isTrigger = true;

        if(action == DragonAction.IDLE || action == DragonAction.IDLE_TRACKING || action == DragonAction.WALKING_F || action == DragonAction.WALKING_B || action == DragonAction.THINKING)
            ActionStart();

        switch (action)
        {
            case DragonAction.IDLE:
                break;
            case DragonAction.IDLE_TRACKING:
                RotateTowardPlayer();
                break;
            case DragonAction.WALKING_F:
                Advance();
                break;
            case DragonAction.WALKING_B:
                BackUp();
                break;
            default:
                break;
        }

        switch (secondaryAction)
        {
            case DragonSecondaryAction.NONE: break;
            case DragonSecondaryAction.ROTATION_LEFT: RotateLeft(); break;
            case DragonSecondaryAction.ROTATION_RIGHT: RotateRight(); break;
        }
        if (usingBreathAttack)
            BreathAttack();
    }


    void Plan()
    {
        //Debug.Log("Planning");
        int choice = UnityEngine.Random.Range(0, 6);
        while((choice > 3 && lastPlan == DragonAction.ATK_BREATH_F))
        {
            choice = UnityEngine.Random.Range(0, 6);
        }
        switch (choice)
        {
            case 0:
                plan = DragonAction.ATK_CLAW_L;
                break;
            case 1:
                plan = DragonAction.ATK_CLAW_R;
                break;
            case 2:
                plan = DragonAction.ATK_BITE_L;
                break;
            case 3:
                plan = DragonAction.ATK_BITE_R;
                break;
            case 4:
            case 5:
                plan = DragonAction.ATK_BREATH_F;
                //action = DragonAction.WALKING_B;
                moveTimer = 2.5f;
                break;
        }
        lastPlan = plan;
        action = DragonAction.THINKING;
        //Debug.Log("Done planning.");
    }
    public void StartRotation_Left()
    {
        secondaryAction = DragonSecondaryAction.ROTATION_LEFT;
    }
    public void StartRotation_Right()
    {
        secondaryAction= DragonSecondaryAction.ROTATION_RIGHT;
    }
    public void ResetSecondary()
    {
        secondaryAction= DragonSecondaryAction.NONE;
    }
    public void Rest()
    {
        //Debug.Log("Preparing to rest.");
        DragonAction newAction = action;
        switch (action)
        {
            case DragonAction.IDLE:
                newAction = DragonAction.IDLE_TRACKING;
                break;
            case DragonAction.ATK_CLAW_L:
            case DragonAction.ATK_CLAW_R:
                restCountdown = UnityEngine.Random.Range(0, 3);
                if(restCountdown > 0)
                    newAction = DragonAction.IDLE;
                break;
            case DragonAction.ATK_BITE_L:
            case DragonAction.ATK_BITE_R:
                restCountdown = UnityEngine.Random.Range(0, 2);
                if (restCountdown > 0)
                    newAction = DragonAction.IDLE_TRACKING;
                break;
            case DragonAction.ATK_BREATH_F:
                restCountdown = UnityEngine.Random.Range(1, 4);
                newAction = DragonAction.IDLE;
                break;
        }
        action = newAction;
        //Debug.Log("Resting for " + restCountdown + " cycles.");
        if (restCountdown <= 0)
            Plan();
        else
            plan = newAction;
    }
    public void TickRest()
    {
        restCountdown--;
        if (restCountdown <= 0)
            Plan();
        else
            Rest();
    }
    void RotateTowardPlayer()
    {
        rb.MoveRotation(Quaternion.RotateTowards(Quaternion.Euler(0,0,rb.rotation),Quaternion.Euler(0,0,Mathf.Atan2(player.transform.position.y - transform.position.y,player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90),rotationSpeed * Time.fixedDeltaTime));
    }
    void RotateLeft()
    {
        rb.MoveRotation(Quaternion.Euler(0, 0, rb.rotation + rotationSpeed * 5 * Time.fixedDeltaTime));
    }
    void RotateRight()
    {
        rb.MoveRotation(Quaternion.Euler(0,0,rb.rotation - rotationSpeed * 5 * Time.fixedDeltaTime));
    }
    void BackUp()
    {
        if(moveTimer <= 0)
        {
            action = plan;
            return;
        }
        RotateTowardPlayer();
        rb.MovePosition(transform.position - transform.up * Time.fixedDeltaTime * maxMoveSpeed / (rage > 0 ? 2 : 3.5f));
        GetComponent<Animator>().SetFloat("Speed", rage > 0 ? -1 : -2 / 3.5f);

        moveTimer -= Time.fixedDeltaTime;
    }
    void Advance()
    {
        RotateTowardPlayer();
        rb.MovePosition(transform.position + transform.up * Time.fixedDeltaTime * maxMoveSpeed / (rage > 0 ? 1 : 2));
        GetComponent<Animator>().SetFloat("Speed", rage > 0 ? 2 : 1);
    }
    void ActionStart()
    {
        switch (plan)
        {
            case DragonAction.IDLE:
            case DragonAction.IDLE_TRACKING:
                action = plan;
                break;
            case DragonAction.ATK_CLAW_L:
                if (Vector2.Distance(rb.position, player.transform.position) < 1.8f * transform.localScale.y && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90)) < 20)
                    action = DragonAction.ATK_CLAW_L;
                else if (action != DragonAction.ATK_CLAW_L)
                    action = DragonAction.WALKING_F;
                break;
            case DragonAction.ATK_CLAW_R:
                if (Vector2.Distance(rb.position, player.transform.position) < 1.8f * transform.localScale.y && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90)) < 20)
                    action = DragonAction.ATK_CLAW_R;
                else if (action != DragonAction.ATK_CLAW_R)
                    action = DragonAction.WALKING_F;
                break;
            case DragonAction.ATK_BITE_L:
                if (Vector2.Distance(rb.position, player.transform.position) < 1.8f * transform.localScale.y && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90)) < 10)
                    action = DragonAction.ATK_BITE_L;
                else if (action != DragonAction.ATK_BITE_L)
                    action = DragonAction.WALKING_F;
                break;
            case DragonAction.ATK_BITE_R:
                if (Vector2.Distance(rb.position, player.transform.position) < 1.8f * transform.localScale.y && Quaternion.Angle(transform.rotation, Quaternion.Euler(0, 0, Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90)) < 10)
                    action = DragonAction.ATK_BITE_R;
                else if (action != DragonAction.ATK_BITE_R)
                    action = DragonAction.WALKING_F;
                break;
            case DragonAction.ATK_BREATH_F:
                if (moveTimer <= 0)
                    action = DragonAction.ATK_BREATH_F;
                else
                    action = DragonAction.WALKING_B;
                break;
        }
        GetComponent<Animator>().SetInteger("Action", (int)action);
    }
    public void BreathStart()
    {
        usingBreathAttack = true;
        breathTime = 0;
        breathTimer = 0;
        breathSwitch = 0;
    }
    public void BreathEnd()
    {
        usingBreathAttack = false;
    }
    void BreathAttack()
    {
        switch(color)
        {
            case DragonColor.RED:
                for (int i = 0; i < 5; i++)
                {
                    GameObject bullet;
                    bullet = Instantiate(projectile, transform.Find("Maw Mark").position, transform.Find("Maw Mark").rotation);
                    bullet.transform.localScale *= 3;
                    Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

                    //add spread
                    Vector2 dir = transform.Find("Maw Mark").up;
                    Vector2 pdir = Vector2.Perpendicular(dir);

                    pdir = Vector2.Perpendicular(dir) * UnityEngine.Random.Range(-0.2f, 0.2f);

                    bulletRB.velocity = (dir + pdir).normalized * 15;
                    //bulletRB.velocity = firePoint.up * circleBulletForce;
                    //circleCD = Time.time + circleCDI;
                    //if (playerSpeed.speed)
                    //{
                    //    circleCD = Time.time + (circleCDI * playerSpeed.fireSpeedUp);
                    //}

                    Destroy(bullet, 20);
                }
                break;
            case DragonColor.BLUE:
                for (int i = 0; i < 2; i++)
                {
                    GameObject bullet;
                    bullet = Instantiate(projectile, transform.Find("Maw Mark").position, transform.Find("Maw Mark").rotation);
                    bullet.transform.localScale *= 3;
                    Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

                    //add spread
                    Vector2 dir = transform.Find("Maw Mark").up;
                    Vector2 pdir = Vector2.Perpendicular(dir);

                    pdir = Vector2.Perpendicular(dir) * Mathf.Max(breathTime - 0.5f, 0) * (i == 0 ? -0.5f : 0.5f);

                    bulletRB.velocity = (dir + pdir).normalized * 15;
                    //bulletRB.velocity = firePoint.up * circleBulletForce;
                    //circleCD = Time.time + circleCDI;
                    //if (playerSpeed.speed)
                    //{
                    //    circleCD = Time.time + (circleCDI * playerSpeed.fireSpeedUp);
                    //}

                    Destroy(bullet, 20);
                }
                breathTime += Time.fixedDeltaTime;
                break;
            case DragonColor.GREEN:
                if(breathTimer <= 0)
                {
                    GameObject bullet;
                    bullet = Instantiate(projectile, transform.Find("Maw Mark").position, transform.Find("Maw Mark").rotation);
                    bullet.transform.localScale *= 2;
                    Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

                    //no spread for wind direction
                    Vector2 dir = transform.Find("Maw Mark").up;

                    bulletRB.velocity = dir.normalized * 12;
                    breathTimer = 0.2f;
                }
                breathTimer -= Time.fixedDeltaTime;
                break;
            case DragonColor.YELLOW:
                if (breathTimer <= 0)
                {
                    int bolts = 0;
                    switch (breathSwitch)
                    {
                        case 0:
                            bolts = 3;
                            break;
                        case 1:
                        case 3:
                            bolts = 4;
                            break;
                        case 2:
                            bolts = 5;
                            break;
                    }
                    breathSwitch++;
                    breathSwitch %= 4;
                    for(int i = 0; i < bolts; i++)
                    {
                        GameObject bullet;
                        bullet = Instantiate(projectile, transform.Find("Maw Mark").position, transform.Find("Maw Mark").rotation);
                        bullet.transform.localScale *= 2;
                        Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

                        //variable spread for lightning direction
                        Vector2 dir = transform.Find("Maw Mark").up;
                        Vector2 pdir = Vector2.Perpendicular(dir);

                        pdir = Vector2.Perpendicular(dir) * ((float)bolts / -2.0f + i) * 0.1f;

                        bulletRB.velocity = (dir + pdir).normalized * 18;
                    }
                    breathTimer = 0.1f;
                }
                breathTimer -= Time.fixedDeltaTime;
                break;
            case DragonColor.PURPLE:
                for (int i = 0; i < 2; i++)
                {
                    GameObject bullet;
                    bullet = Instantiate(projectile, transform.Find("Maw Mark").position, transform.Find("Maw Mark").rotation);
                    bullet.transform.localScale *= 3;
                    Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

                    //add spread
                    Vector2 dir = transform.Find("Maw Mark").up;
                    Vector2 pdir = Vector2.Perpendicular(dir);

                    pdir = Vector2.Perpendicular(dir) * UnityEngine.Random.Range(-0.3f, 0.3f);

                    bulletRB.velocity = (dir + pdir).normalized * 15;
                    //bulletRB.velocity = firePoint.up * circleBulletForce;
                    //circleCD = Time.time + circleCDI;
                    //if (playerSpeed.speed)
                    //{
                    //    circleCD = Time.time + (circleCDI * playerSpeed.fireSpeedUp);
                    //}

                    ///Destroy(bullet, 20);
                }
                break;
        }
    }
    private void OnBecameVisible()
    {
        if (!aggro)
        {
            aggro = true;
            GameObject.FindGameObjectWithTag("BGM Player").GetComponent<BGMLooper>().StopTrack();
            GameObject.FindGameObjectWithTag("BGM Player").GetComponent<BGMLooper>().PlayTrack(Track.Boss);
        }
    }
}
