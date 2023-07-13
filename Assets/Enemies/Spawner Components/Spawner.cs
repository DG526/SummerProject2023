using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Player and Active Range")]
    public GameObject player;
    public float threshold = 30f;

    [Header ("Spawn Point")]
    public Transform SpawnPoint;
    public float spawnAngleOffset = 0.3f;
    public float spawnDistOffset = 3f;
    public float spawnRadius = 2f;
    public int maxSpawnAttempts = 10;
    public SpawnController controller;
    public int startCount = 3;

    [Header ("Enemies")]
    public GameObject wyrm;
    public GameObject drake;
    public GameObject wyvern;

    [Header ("Spawn Interval")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 5f;

    [Header ("Spawn Rates")]
    public float wyrmRate = 0.3f;
    public float drakeRate = 0.7f;
    public float wyvernRate = 1f;

    float distance;
    Vector3 direction;
    float spawnTime;
    public int localLimit = 10;
    int local = 0;
    SetMap map;
    bool first = true;
    float firstThresh;

    public static bool canSpawn = true;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("SpawnController").GetComponent<SpawnController>();

        spawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);

        player = GameObject.Find("Player");

        map = GameObject.Find("Map").GetComponent<SetMap>();

        SpawnPoint = gameObject.transform;
        firstThresh = threshold * 2f;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y));
        direction = player.transform.position - SpawnPoint.position;
        direction.Normalize();
        direction.z = 0;

        if(distance <= firstThresh && first)
        {
            while (controller.enemyCheck(1) && local < startCount)
            {
                Spawn();
            }
            first = false;
        }
        if (distance <= threshold && spawnTime < Time.time)
        {
            if (controller.enemyCheck(1) && Check(1))
                Spawn();

            spawnTime = Time.time + Random.Range(minSpawnTime,maxSpawnTime);
        }
    }

    //return true if you can spawn that number of 
    public bool Check(int val)
    { return local + val <= localLimit; }

    public void Add(int val)
    { local += val; }

    public void Remove()
    { local--; }

    public void Spawn ()
    {
        if (!canSpawn) return;
        controller.enemyAdd(1);
        local++;
        float spawn = Random.Range(0f, wyvernRate);
        GameObject enemy;
        //Vector3 pdir = Vector2.Perpendicular(SpawnPoint.up) * Random.Range(-spawnAngleOffset, spawnAngleOffset);
        float dist = Random.Range(1.5f, spawnDistOffset);
        //Vector3 mod = (SpawnPoint.up + pdir).normalized * dist;
        Vector3 mod = direction * dist;

        mod = FindSpawnPoint(mod + SpawnPoint.position);
        if (spawn > drakeRate)
        {
            enemy = Instantiate(wyvern, SpawnPoint.position + mod + new Vector3(0,0,-0.5f), SpawnPoint.rotation);
            enemy.GetComponent<EnemyHealth>().spawner = gameObject;
            if(map != null)
                enemy.GetComponent<SpriteRenderer>().color = map.GetColor();
        }
        else if (spawn > wyrmRate)
        {
            enemy = Instantiate(drake, mod, SpawnPoint.rotation);
            enemy.GetComponent<EnemyHealth>().spawner = gameObject;
            if (map != null)
                enemy.GetComponent<SpriteRenderer>().color = map.GetColor();
        }
        else
        {
            enemy = Instantiate(wyrm, mod, SpawnPoint.rotation);
            enemy.GetComponent<EnemyHealth>().spawner = gameObject;
            if (map != null)
                enemy.GetComponent<SpriteRenderer>().color = map.GetColor();
        }
    }

    private Vector3 FindSpawnPoint(Vector3 input)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(input, spawnRadius);
        int attempts = 0;
        Vector3 og = input;
        bool inMap = false;
        
        while(attempts < maxSpawnAttempts)
        {
            foreach (Collider2D hit in hits)
            {
                //Debug.Log(hit.gameObject.name);
                if (hit.gameObject.name.IndexOf("Map") != -1)
                {
                    inMap = true;
                    //Debug.Log("Valid");
                    break;
                }
            }
            attempts++;
            if(hits.Length > 1)
            {
                input *= spawnRadius;
                hits = Physics2D.OverlapCircleAll(input, spawnRadius);
            }
            else if (hits.Length == 1 && inMap)
            { 
                return input;
            }
            inMap = false;
        }
        if(!inMap)
        {
            return SpawnPoint.position;
        }
        return og;
    }
}
