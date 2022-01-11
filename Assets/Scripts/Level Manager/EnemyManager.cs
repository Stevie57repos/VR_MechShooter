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

    [SerializeField]
    private EnemyDeathEventSO _enemyDeath;

    private void OnEnable()
    {
        _enemyDeath.EnemyDeathEvent += EnemyDeathRemoval;
    }

    private void OnDisable()
    {
        _enemyDeath.EnemyDeathEvent -= EnemyDeathRemoval;
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
        for(int i = 0; i < waveData.enemyAmount; i++)
        {
            EnemyController enemy = _enemyFlockManager.GetEnemy(waveData.enemyPrefab);
            _enemylist.Add(enemy);            
            yield return new WaitForSeconds(waveData.delayBetweenSpawn);
        }     

        foreach(EnemyController enemy in _enemylist)
        {
            enemy.AttackHandler(_playerTarget, _enemylist);
        }
    }

    private void EnemyDeathRemoval(EnemyController enemy)
    {
        _enemylist.Remove(enemy);
        if(_enemylist.Count == 0)
        {
            DebugEditorScreen.Instance.DisplayValue($"All enemies in wave cleared");
        }
    }
}
