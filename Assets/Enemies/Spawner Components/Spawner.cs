using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header ("Spawn Point")]
    public Transform SpawnPoint;

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
            if(controller.enemyCheck(1))
            {
                controller.enemyAdd(1);
                float spawn = Random.Range(0f, wyvernRate);
                GameObject enemy;
                if(spawn > drakeRate)
                    enemy = Instantiate(wyvern, SpawnPoint.position, SpawnPoint.rotation);
                else if (spawn > wyrmRate)
                    enemy = Instantiate(drake, SpawnPoint.position, SpawnPoint.rotation);
                else
                    enemy = Instantiate(wyrm,SpawnPoint.position, SpawnPoint.rotation);

                Debug.Log("Spawned Enemy");
            }
            spawnTime = Time.time + Random.Range(minSpawnTime,maxSpawnTime);
        }
    }
}
