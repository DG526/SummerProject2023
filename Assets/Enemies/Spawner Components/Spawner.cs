using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header ("Spawn Point")]
    public Transform SpawnPoint;
    public float spawnAngleOffset = 0.3f;
    public float spawnDistOffset = 3f;

    public SpawnController controller;

    [Header ("Enemies")]
    public GameObject wyrm;
    public GameObject drake;
    public GameObject wyvern;

    [Header ("Spawn Interval")]
    public float minSpawnTime = 1f;
    public float maxSpawnTime = 5f;

    public float wyrmRate = 0.3f;
    public float drakeRate = 0.7f;
    public float wyvernRate = 1f;

    float spawnTime;
    public int localLimit = 10;
    int local = 0;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("SpawnController").GetComponent<SpawnController>();

        spawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnTime < Time.time)
        {
            if (controller.enemyCheck(1) && Check(1))
                Spawn();
            else
                Debug.Log("Spawn Failed");

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
        controller.enemyAdd(1);
        local++;
        float spawn = Random.Range(0f, wyvernRate);
        GameObject enemy;

        Vector3 pdir = Vector2.Perpendicular(SpawnPoint.up) * Random.Range(-spawnAngleOffset, spawnAngleOffset);
        float dist = Random.Range(0, spawnDistOffset);
        Vector3 mod = (SpawnPoint.up + pdir).normalized * dist;
        if (spawn > drakeRate)
        {
            enemy = Instantiate(wyvern, SpawnPoint.position + mod, SpawnPoint.rotation);
            enemy.GetComponent<EnemyHealth>().spawner = gameObject;
        }
        else if (spawn > wyrmRate)
        {
            enemy = Instantiate(drake, SpawnPoint.position + mod, SpawnPoint.rotation);
            enemy.GetComponent<EnemyHealth>().spawner = gameObject;
        }
        else
        {
            enemy = Instantiate(wyrm, SpawnPoint.position + mod, SpawnPoint.rotation);
            enemy.GetComponent<EnemyHealth>().spawner = gameObject;
        }
    }
}
