using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyController _trainingDummy;
    [SerializeField]
    private EnemyController _sentinel;
    [SerializeField]
    List<EnemyController> _enemylist = new List<EnemyController>();
    private Transform _target;

    public void Awake()
    {
        PoolSystem.CreatePool(_trainingDummy, 10);
        PoolSystem.CreatePool(_sentinel, 10);
    }

    private void Update()
    {
        foreach (EnemyController enemy in _enemylist)
        {
            enemy.MoveTowardsTarget(_target.position);
            enemy.Seperate(_enemylist);
        }       
    }

    public void SetUpEnemyManager(Transform target)
    {
        _target = target;
    }

    public void SpawnEnemies(EnemyWave waveData)
    {
        StartCoroutine(SpawnEnemyWave(waveData));
    }

    private IEnumerator SpawnEnemyWave(EnemyWave waveData)
    {
        int LocationTracker = 0;
        for(int i = 0; i < waveData.enemyAmount; i++)
        {
            EnemyController enemy = PoolSystem.GetNext(waveData.enemyPrefab) as EnemyController;
            enemy.transform.position = waveData.enemySpawnPosition[LocationTracker];
            enemy.gameObject.SetActive(true);
            _enemylist.Add(enemy);
            yield return new WaitForSeconds(waveData.delayBetweenSpawn);
            LocationTracker++;
            if(LocationTracker >= waveData.enemySpawnPosition.Count) LocationTracker = 0;
        }     
    }
}
