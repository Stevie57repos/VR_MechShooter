using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    List<EnemyController> _enemylist = new List<EnemyController>();
    private Transform _playerTarget;

    [Header("Flock Settings")]
    [SerializeField]
    private EnemyFlockManager _enemyFlockManager;

    // get these amounts from the level data in the future
    private void Update()
    {
        if (_enemylist.Count > 0)
        {
            foreach (EnemyController enemy in _enemylist)
            {
                enemy.MoveTowardsTarget(_playerTarget.position);
                enemy.Seperate(_enemylist);
            }
        }
    }

    public void SetUpEnemyManager(Transform target, int scoutFlockSpawnAmount, int sentinelFlockSpawnAmount)
    {
        _playerTarget = target;
        _enemyFlockManager.SpawnFlock(scoutFlockSpawnAmount, sentinelFlockSpawnAmount);
    }

    public void SpawnEnemies(EnemyWave waveData)
    {
        Debug.Log($"spawn wave has been called");
        StartCoroutine(SpawnEnemyWave(waveData));
    }

    private IEnumerator SpawnEnemyWave(EnemyWave waveData)
    {
        int LocationTracker = 0;
        for(int i = 0; i < waveData.enemyAmount; i++)
        {
            EnemyController enemy = _enemyFlockManager.GetEnemy(waveData.enemyPrefab);
            _enemylist.Add(enemy);
            yield return new WaitForSeconds(waveData.delayBetweenSpawn);
        }     
    }
}
