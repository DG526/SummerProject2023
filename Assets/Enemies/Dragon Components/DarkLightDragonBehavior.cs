using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkLightDragonBehavior : MonoBehaviour
{
    public GameObject player;
    //public int maxHealth, health;
    public float maxMoveSpeed, rotationSpeed;
    float moveSpeed;
    int restCountdown = 2;
    public DragonAction action, plan;
    DragonAction lastPlan;
    public DragonSecondaryAction secondaryAction;
    public GameObject lightProjectile, darkProjectile;
    public int rage; // 0 = normal, 1 (HP < 40%) = faster movement, 2 (HP < 15%) = can spam breath. 
    float moveTimer;
    Rigidbody2D rb;
    bool usingBreathAttack;
    float breathTime; // For breath patterns based on position in time (Water)
    float breathTimer; // For breath patterns in bursts (Wind)
    int breathSwitch; // For breath patterns with variable amounts of bullets/frame (Lightning)
    bool aggro = false;
    public AudioClip sndClaw, sndBite, sndFootstep, sndInhale, sndBreath, sndBreath2;
    AudioSource[] sources;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        sources = GetComponents<AudioSource>();
        if (sndBite)
            sndBite.LoadAudioData();
        if (sndClaw)
            sndClaw.LoadAudioData();
        if (sndFootstep)
            sndFootstep.LoadAudioData();
        if (sndInhale)
            sndInhale.LoadAudioData();
        if (sndBreath)
            sndBreath.LoadAudioData();
        if (sndBreath2)
            sndBreath2.LoadAudioData();
    }
    private void OnDestroy()
    {
        Spawner.canSpawn = false;
        GameObject.Find("GameOver").GetComponent<GameOver>().WinWaitStart(5);
        player.GetComponent<PlayerHealth>().graceTime = Time.time + 5.2f;
        player.GetComponent<PlayerHealth>().grace = true;
    }
    private void FixedUpdate()
    {
        DestroyImmediate(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        GetComponent<PolygonCollider2D>().isTrigger = true;

        if (action == DragonAction.IDLE || action == DragonAction.IDLE_TRACKING || action == DragonAction.WALKING_F || action == DragonAction.WALKING_B || action == DragonAction.THINKING)
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
        Debug.Log("Planning");
        int choice = UnityEngine.Random.Range(0, 10);
        while ((lastPlan == DragonAction.ATK_BREATH_L && choice > 5 && choice < 8) || (lastPlan == DragonAction.ATK_BREATH_R && choice > 7))
        {
            choice = UnityEngine.Random.Range(0, 10);
        }
        switch (choice)
        {
            case 0:
            case 1:
            case 2:
                plan = DragonAction.ATK_CLAW_L;
                break;
            case 3:
            case 4:
            case 5:
                plan = DragonAction.ATK_CLAW_R;
                break;
            case 6:
            case 7:
                plan = DragonAction.ATK_BREATH_L;
                moveTimer = 1.5f;
                break;
            case 8:
            case 9:
                plan = DragonAction.ATK_BREATH_R;
                moveTimer = 1.5f;
                break;
        }
        lastPlan = plan;
        action = DragonAction.THINKING;
        Debug.Log("Done planning.");
    }
    public void StartRotation_Left()
    {
        secondaryAction = DragonSecondaryAction.ROTATION_LEFT;
    }
    public void StartRotation_Right()
    {
        secondaryAction = DragonSecondaryAction.ROTATION_RIGHT;
    }
    public void ResetSecondary()
    {
        secondaryAction = DragonSecondaryAction.NONE;
    }
    public void Rest()
    {
        Debug.Log("Preparing to rest.");
        DragonAction newAction = action;
        switch (action)
        {
            case DragonAction.IDLE:
                newAction = DragonAction.IDLE_TRACKING;
                break;
            case DragonAction.ATK_CLAW_L:
            case DragonAction.ATK_CLAW_R:
                restCountdown = UnityEngine.Random.Range(0, 2);
                if (restCountdown > 0)
                    newAction = DragonAction.IDLE;
                break;
            case DragonAction.ATK_BREATH_L:
            case DragonAction.ATK_BREATH_R:
                restCountdown = UnityEngine.Random.Range(1, 3);
                newAction = DragonAction.IDLE;
                break;
        }
        action = newAction;
        Debug.Log("Resting for " + restCountdown + " cycles.");
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
        rb.MoveRotation(Quaternion.RotateTowards(Quaternion.Euler(0, 0, rb.rotation), Quaternion.Euler(0, 0, Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg - 90), rotationSpeed * Time.fixedDeltaTime));
    }
    void RotateLeft()
    {
        rb.MoveRotation(Quaternion.Euler(0, 0, rb.rotation + rotationSpeed * 5 * Time.fixedDeltaTime));
    }
    void RotateRight()
    {
        rb.MoveRotation(Quaternion.Euler(0, 0, rb.rotation - rotationSpeed * 5 * Time.fixedDeltaTime));
    }
    void BackUp()
    {
        if (moveTimer <= 0)
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
            case DragonAction.ATK_BREATH_L:
            case DragonAction.ATK_BREATH_R:
                if (moveTimer <= 0)
                    action = plan;
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
        switch (action)
        {
            case DragonAction.ATK_BREATH_L: // Light Breath
                for (int i = 0; i < 5; i++)
                {
                    GameObject bullet;
                    bullet = Instantiate(lightProjectile, transform.Find("Maw Mark").position, transform.Find("Maw Mark").rotation);
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

                    Destroy(bullet, 4);
                }
                break;
            case DragonAction.ATK_BREATH_R: // Dark Breath
                for (int i = 0; i < 5; i++)
                {
                    GameObject bullet;
                    bullet = Instantiate(darkProjectile, transform.Find("Maw Mark").position, transform.Find("Maw Mark").rotation);
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

                    Destroy(bullet, 4);
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
            GameObject.FindGameObjectWithTag("BGM Player").GetComponent<BGMLooper>().PlayTrack(Track.FinalBoss);
        }
    }
    public void PlaySound(DragonSound sound)
    {
        AudioSource source = sources[0];
        AudioClip clip = sndFootstep;
        switch (sound)
        {
            case DragonSound.FOOTSTEP:
                source = sources[0];
                clip = sndFootstep;
                break;
            case DragonSound.CLAW:
                source = sources[1];
                clip = sndClaw;
                break;
            case DragonSound.BITE:
                source = sources[1];
                clip = sndBite;
                break;
            case DragonSound.INHALE:
                source = sources[1];
                clip = sndInhale;
                break;
            case DragonSound.BREATH:
                source = sources[1];
                clip = sndBreath;
                break;
            case DragonSound.BREATH2:
                source = sources[1];
                clip = sndBreath2;
                break;
        }
        source.clip = clip;
        source.Play();
    }
}
