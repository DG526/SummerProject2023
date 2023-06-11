using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerSpeed playerSpeed;
    public PlayerCatalyst playerCatalyst;

    public Transform firePoint;
    public GameObject triangleBullet;
    public GameObject circleBullet;
    public GameObject poisonBullet;
    public GameObject rockDrop;
    public GameObject rock;
    public GameObject wind;
    public GameObject lightning;


    public int numBullets = 5;
    public float triangleSpreadInterval = 0.4f;
    private float triangleSpread;

    //bullet speed
    public float circleBulletForce = 20f;
    public float triangleBulletForce = 20f;
    public float poisonForce = 10f;
    public float lightningSpeed = 80f;

    //speed of wind
    public float windSpeed = 15f;
    //force with which wind pushes objects
    public float windForce = 30f;

    //Circle Cooldown Interval
    public float circleCDI = 0.1f;

    //Circle Cooldown
    private float circleCD = 0f;

    //Circle Spread Range
    public float circleSpread = 3f;

    //Circle Time to Live
    public float circleTTL = 1f;

    //Triangle Cooldown Interval
    public float triangleCDI = 0.5f;

    //Triangle Cooldown
    private float triangleCD = 0f;

    //Triangle Time to Live
    public float triangleTTL = 0.5f;

    //Poison Cooldown Interval
    public float poisonCDI = 2f;

    //Poison Cooldown
    private float poisonCD = 0f;

    //Rock Cooldown Interval
    public float rockCDI = 2f;

    //Rock Cooldown
    private float rockCD = 0f;

    //Rock Spawn Time
    public float rockTime = 0.5f;

    //Rock Time to Live
    public float rockTTL = 5f;

    //Rock distance from player
    public float rockDist = 5f;

    //Wind Cooldown to Interval
    public float windCDI = 1f;

    //Wind Cooldown
    private float windCD = 0f;

    //Wind Time to Live
    public float windTTL = 1.5f;

    //Lightning Cooldown Interval
    public float lightningCDI = 0.75f;

    //Lightning Cooldown
    private float lightningCD = 0f;

    //Lightning Time to Live
    public float lightningTTL = 1.5f;

    public string Fire1 = "circle";
    public string Fire2 = "triangle";

    void Start()
    {
        //playerCatalyst = gameObject.GetComponent<PlayerCatalyst>();
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        playerSpeed = gameObject.GetComponent<PlayerSpeed>();
        playerCatalyst = GetComponent<PlayerCatalyst>();

    }
    // Update is called once per frame
    void Update()
    {
        //you should not be able to hold down both mouse buttons and fire both weapons at the same time

        //can't shoot if you're dead
        
        
        if (!playerHealth.dead)
        {
            //left click
            if (Input.GetButton("Fire1") && !Input.GetButton("Fire2"))
            {
                Fire(Fire1);
            }

            //right click
            if (Input.GetButton("Fire2") && !Input.GetButton("Fire1"))
            {
                Fire(Fire2);
            }

            /*if (Input.GetButton("Fire3"))
            {
                Fire("poison");
            }

            //wind and rock drop
            if (Input.GetButtonDown("Jump"))
            {
                //Fire("rockDrop");
                Fire("wind");
            }*/
        }
    }


    void Fire(string type)
    {
        //Debug.Log("X:" + Input.GetAxisRaw("Mouse X"));
        //Debug.Log("Y:" + Input.GetAxisRaw("Mouse Y"));
        GameObject bullet = null;

        if (type == "circle" && Time.time > circleCD)
        {
            bullet = Instantiate(circleBullet, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            
            if(playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            //add spread
            Vector2 dir = firePoint.up;
            Vector2 pdir = Vector2.Perpendicular(dir);

            pdir = Vector2.Perpendicular(dir) * Random.Range(-circleSpread, circleSpread);

            bulletRB.velocity = (dir + pdir).normalized * circleBulletForce;
            //bulletRB.velocity = firePoint.up * circleBulletForce;
            circleCD = Time.time + circleCDI;
            if(playerSpeed.speed)
            {
                circleCD -= playerSpeed.fireSpeedUp;
            }

            Destroy(bullet, circleTTL);
        }

        if (type == "triangle" && Time.time > triangleCD)
        {
            FireTriangle(bullet);
            triangleCD = Time.time + triangleCDI;
            if (playerSpeed.speed)
            {
                triangleCD -= playerSpeed.fireSpeedUp;
            }
        }

        if (type == "poison" && Time.time > poisonCD)
        {
            bullet = Instantiate(poisonBullet, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();

            if (playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            bulletRB.velocity = firePoint.up * poisonForce;
            poisonCD = Time.time + poisonCDI;
            if (playerSpeed.speed)
            {
                poisonCD -= playerSpeed.fireSpeedUp;
            }
        }

        if (type == "rockDrop" && Time.time > rockCD)
        {
            //firePoint.rotation changed to 0
            bullet = Instantiate(rockDrop, firePoint.position + firePoint.up * rockDist, new Quaternion(0,0,0,0));

            if (playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            RockDrop script = bullet.GetComponent<RockDrop>();
            script.shooting = gameObject.GetComponent<Shooting>();
            

            StartCoroutine(Rock(bullet));
            rockCD = Time.time + rockCDI;
            
            if (playerSpeed.speed)
            {
                rockCD -= playerSpeed.fireSpeedUp;
            }

            
        }

        if (type == "wind" && Time.time > windCD)
        {
            bullet = Instantiate(wind, firePoint.position, firePoint.rotation);

            if (playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            Rigidbody2D windRB = bullet.GetComponent<Rigidbody2D>();
            windRB.AddForce(firePoint.up * windSpeed, ForceMode2D.Impulse);
            windCD = Time.time + windCDI;
            if (playerSpeed.speed)
            {
                windCD -= playerSpeed.fireSpeedUp;
            }

            WindAttack windAttack = bullet.GetComponent<WindAttack>();
            windAttack.shooting = gameObject.GetComponent<Shooting>();
            Destroy(bullet, windTTL);
        }

        if (type == "lightning" && Time.time > lightningCD)
        {
            bullet = Instantiate(lightning, firePoint.position, firePoint.rotation);
            Rigidbody2D lightningRB = bullet.GetComponent<Rigidbody2D>();

            if (playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            lightningRB.velocity = firePoint.up * lightningSpeed;
            lightningCD = Time.time + lightningCDI;
            if (playerSpeed.speed)
            {
                lightningCD -= playerSpeed.fireSpeedUp;
            }

            Lightning script = bullet.GetComponent<Lightning>();
            script.shooting = gameObject.GetComponent<Shooting>();
            Destroy(bullet, lightningTTL);
        }
    }

    void FireTriangle(GameObject bullet)
    {
        triangleSpread = triangleSpreadInterval;
        //Debug.Log("Original spread: " + triangleSpread);
        for (int i = 0; i < numBullets; i++)
        {

            bullet = Instantiate(triangleBullet, firePoint.position, firePoint.rotation);

            if(playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }
            

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            if (i == 0)
            {
                bulletRB.velocity = (firePoint.up * triangleBulletForce);
                Destroy(bullet, triangleTTL);
                continue;
            }

            Vector2 dir = firePoint.up;
            Vector2 pdir = Vector2.Perpendicular(dir);

            if (i % 2 == 1)
            {
                pdir = Vector2.Perpendicular(dir) * -triangleSpread;
            }

            if (i % 2 == 0)
            {
                pdir = Vector2.Perpendicular(dir) * triangleSpread;
                triangleSpread += triangleSpreadInterval;
            }

            //Debug.Log("Current spread: " + triangleSpread);
            bulletRB.velocity = (dir + pdir).normalized * triangleBulletForce;
            Destroy(bullet, triangleTTL);
        }
    }

    IEnumerator Rock(GameObject bullet)
    {
        yield return new WaitForSeconds(rockTime);
        GameObject Rock = Instantiate(rock, bullet.transform.position, bullet.transform.rotation);

        if(playerCatalyst.catalyst)
        {
            Rock.transform.localScale *= playerCatalyst.catalystFactor;
        }

        Destroy(Rock, rockTTL);
    }
}
