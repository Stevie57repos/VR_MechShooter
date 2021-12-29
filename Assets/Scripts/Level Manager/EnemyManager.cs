using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyController _trainingDummy;
    [SerializeField]
    private EnemyController _sentinel;

    public void Awake()
    {
        PoolSystem.CreatePool(_trainingDummy, 10);
        PoolSystem.CreatePool(_sentinel, 10);
    }

    public void SpawnEnemies(EnemyWave waveData)
    {
        StartCoroutine(SpawnEnemyWave(waveData));
    }

    private IEnumerator SpawnEnemyWave(EnemyWave waveData)
    {
        for(int i = 0; i < waveData.enemyAmount; i++)
        {
            EnemyController enemy = PoolSystem.GetNext(waveData.enemyPrefab) as EnemyController;
            enemy.transform.position = waveData.enemySpawnPosition[i];
            enemy.gameObject.SetActive(true);
            yield return new WaitForSeconds(waveData.delayBetweenSpawn);
        }     
    }
}
