using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public int enemyLimit = 100;
    int enemyCount = 0;
    
    //true if you can spawn enemy
    public bool enemyCheck (int numEnemies)
    {
        return enemyCount + numEnemies <= enemyLimit;
    }


    public void enemyAdd (int numEnemies)
    {
        enemyCount += numEnemies;
    }
}
