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
    [SerializeField]
    private int _scoutFlockSpawnAmount;
    [SerializeField]
    private int _sentinelFlockSpawnAmount;

    private void Start()
    {
        _enemyFlockManager.SpawnFlock(_scoutFlockSpawnAmount, _sentinelFlockSpawnAmount); 
    }
    private void Update()
    {
        if (_enemylist.Count > 0)
        {
            foreach (EnemyController enemy in _enemylist)
            {
                enemy.MoveTowardsTarget(_playerTarget.position);
            }
        }
    }

    public void SetUpEnemyManager(Transform target)
    {
        _playerTarget = target;
    }

    public void SpawnEnemies(EnemyWave waveData)
    {
        Debug.Log($"spawn wave has been called");
        StartCoroutine(SpawnEnemyWave(waveData));
    }
    // need to retreive from flock list and split them into enemy wave
    private IEnumerator SpawnEnemyWave(EnemyWave waveData)
    {
        int LocationTracker = 0;
        for(int i = 0; i < waveData.enemyAmount; i++)
        {
            //EnemyController enemy = PoolSystem.GetNext(waveData.enemyPrefab) as EnemyController;
            //enemy.transform.position = waveData.enemySpawnPosition[LocationTracker];
            //enemy.gameObject.SetActive(true);
            //_enemylist.Add(enemy);
            //yield return new WaitForSeconds(waveData.delayBetweenSpawn);
            //LocationTracker++;
            //if(LocationTracker >= waveData.enemySpawnPosition.Count) LocationTracker = 0;

            EnemyController enemy = _enemyFlockManager.GetEnemy(waveData.enemyPrefab);
            _enemylist.Add(enemy);
            yield return new WaitForSeconds(waveData.delayBetweenSpawn);
        }     
    }


}
