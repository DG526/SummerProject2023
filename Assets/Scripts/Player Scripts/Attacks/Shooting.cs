using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [Header ("Other Scripts")]
    public PlayerHealth playerHealth;
    public PlayerSpeed playerSpeed;
    public PlayerCatalyst playerCatalyst;

    [Header("Prefabs and Transforms")]
    public Transform firePoint;
    public GameObject gem;
    public GameObject triangleBullet;
    public GameObject circleBullet;
    public GameObject poisonBullet;
    public GameObject rockDrop;
    public GameObject rock;
    public GameObject wind;
    public GameObject lightning;

    [Header ("Projectile Forces/Speeds")]
    //bullet speed
    public float circleBulletForce = 20f;
    public float triangleBulletForce = 20f;
    public float poisonForce = 10f;
    public float lightningSpeed = 80f;
    public float windSpeed = 15f;
    public float windForce = 30f;

    [Header ("Fireball")]
    public float circleCDI = 0.1f;
    private float circleCD = 0f;
    public float circleSpread = 3f;
    public float circleTTL = 1f;
    public bool circleUpgraded = false;

    [Header("Water Shotgun")]
    public float triangleCDI = 0.5f;
    private float triangleCD = 0f;
    public float triangleTTL = 0.5f;
    public int numBullets = 5;
    public float triangleSpreadInterval = 0.4f;
    private float triangleSpread;

    [Header ("Poison")]
    public float poisonCDI = 2f;
    private float poisonCD = 0f;
    public bool poisonUpgraded = false;

    [Header("Rock")]
    public float rockCDI = 2f;
    private float rockCD = 0f;
    public float rockTime = 0.5f;
    public float rockTTL = 5f;
    public float rockDist = 5f;
    public bool rockUpgraded = false;

    [Header ("Wind")]
    public float windCDI = 1f;
    private float windCD = 0f;
    public float windTTL = 1.5f;
    public float windPush = 1.5f;
    public bool windUpgraded = false;

    [Header ("Lightning")]
    public float lightningCDI = 0.75f;
    private float lightningCD = 0f;
    public float lightningTTL = 1.5f;
    public bool lightningUpgraded = false;

    [Header ("Selected Loadout")]
    public string Fire1 = "circle";
    public string Fire2 = "triangle";


    GameObject loadout;
    void Start()
    {
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        playerSpeed = gameObject.GetComponent<PlayerSpeed>();
        playerCatalyst = GetComponent<PlayerCatalyst>();
        loadout = GameObject.Find("LoadOut");
    }
    // Update is called once per frame
    void Update()
    {
        //you should not be able to hold down both mouse buttons and fire both weapons at the same time

        //can't shoot if you're dead
        if (playerHealth.dead)
            return;

        if (loadout != null && loadout.activeInHierarchy)
            return;

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
    }

    void Fire(string type)
    {
        //Debug.Log("X:" + Input.GetAxisRaw("Mouse X"));
        //Debug.Log("Y:" + Input.GetAxisRaw("Mouse Y"));
        GameObject bullet = null;

        #region fireball
        if (type == "circle" && Time.time > circleCD)
        {
            gem.GetComponent<SpriteRenderer>().color = new Color32(243,78,4,255);
            bullet = Instantiate(circleBullet, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bullet.GetComponent<Bullet>().shooting = this;

            if(circleUpgraded)
            {
                bullet.GetComponent<Bullet>().damage = (int)(bullet.GetComponent<Bullet>().damage * 1.5);
            }
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
                circleCD = Time.time + (circleCDI * playerSpeed.fireSpeedUp);
            }

            Destroy(bullet, circleTTL);
        }
        #endregion

        #region water
        if (type == "triangle" && Time.time > triangleCD)
        {
            gem.GetComponent<SpriteRenderer>().color = Color.blue;
            FireTriangle(bullet);
            triangleCD = Time.time + triangleCDI;
            if (playerSpeed.speed)
            {
                triangleCD = Time.time + (triangleCDI * playerSpeed.fireSpeedUp);
            }
        }
        #endregion

        #region poison
        if (type == "poison" && Time.time > poisonCD)
        {
            bullet = Instantiate(poisonBullet, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bullet.GetComponent<Poison>().shooting = this;
            gem.GetComponent<SpriteRenderer>().color = new Color32(237, 72, 241, 150);

            if(poisonUpgraded)
            {
                bullet.transform.localScale *= 1.5f;
                bullet.GetComponent<Poison>().tickSpeed = bullet.GetComponent<Poison>().tickSpeed - 0.1f;
            }

            if (playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            bulletRB.velocity = firePoint.up * poisonForce;
            poisonCD = Time.time + poisonCDI;
            if (playerSpeed.speed)
            {
                poisonCD = Time.time + (poisonCDI * playerSpeed.fireSpeedUp);
            }
        }
        #endregion

        #region rockDrop
        if (type.IndexOf("rock") != -1  && Time.time > rockCD)
        {
            //firePoint.rotation changed to 0
            gem.GetComponent<SpriteRenderer>().color = new Color32(188, 127, 100, 255);
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
                rockCD = Time.time + (rockCDI * playerSpeed.fireSpeedUp);
            }
        }
        #endregion

        #region wind
        if (type == "wind" && Time.time > windCD)
        {
            bullet = Instantiate(wind, firePoint.position, firePoint.rotation);
            gem.GetComponent<SpriteRenderer>().color = new Color32(42, 255, 216, 255);
            if (playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            if(windUpgraded)
            {
                bullet.transform.localScale *= 1.5f;
            }

            Rigidbody2D windRB = bullet.GetComponent<Rigidbody2D>();
            windRB.AddForce(firePoint.up * windSpeed, ForceMode2D.Impulse);
            windCD = Time.time + windCDI;
            if (playerSpeed.speed)
            {
                windCD = Time.time + (windCDI * playerSpeed.fireSpeedUp);
            }

            WindAttack windAttack = bullet.GetComponent<WindAttack>();
            windAttack.shooting = gameObject.GetComponent<Shooting>();
            Destroy(bullet, windTTL);
        }
        #endregion

        #region lightning
        if (type == "lightning" && Time.time > lightningCD)
        {
            bullet = Instantiate(lightning, firePoint.position, firePoint.rotation);
            Rigidbody2D lightningRB = bullet.GetComponent<Rigidbody2D>();
            gem.GetComponent<SpriteRenderer>().color = new Color32(255, 251, 0, 255);

            if (playerCatalyst.catalyst)
            {
                bullet.transform.localScale *= playerCatalyst.catalystFactor;
            }

            lightningRB.velocity = firePoint.up * lightningSpeed;
            lightningCD = Time.time + lightningCDI;
            if (playerSpeed.speed)
            {
                lightningCD = Time.time + (lightningCDI * playerSpeed.fireSpeedUp);
            }

            Lightning script = bullet.GetComponent<Lightning>();
            script.shooting = gameObject.GetComponent<Shooting>();

            if(lightningUpgraded)
            {
                script.maxStrikes = script.maxStrikes + 2;
                script.damage = (int)(script.damage + 20);
            }
            Destroy(bullet, lightningTTL);
        }
        #endregion
    }

    void FireTriangle(GameObject bullet)
    {
        triangleSpread = triangleSpreadInterval;
        //Debug.Log("Original spread: " + triangleSpread);
        for (int i = 0; i < numBullets; i++)
        {

            bullet = Instantiate(triangleBullet, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Bullet>().shooting = this;

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
        Rock.GetComponent<Rock>().shooting = this;

        if(rockUpgraded)
        {
            Rock.GetComponent<Rock>().damage = Rock.GetComponent<Rock>().damage + 20;
        }

        if(playerCatalyst.catalyst)
        {
            Rock.transform.localScale *= playerCatalyst.catalystFactor;
        }
        Destroy(Rock, rockTTL);
    }
}
