using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyWave
{
    public EnemyController enemyPrefab;
    public float spawnWaveTime;
    public int enemyAmount;  
    public List<Vector3> enemySpawnPosition;
    public float delayBetweenSpawn;
}
