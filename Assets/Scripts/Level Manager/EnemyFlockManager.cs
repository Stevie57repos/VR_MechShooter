using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlockManager : MonoBehaviour
{
    [SerializeField]
    private EnemyController _scoutDrone;
    [SerializeField]
    private EnemyController _sentinel;
    [SerializeField]
    List<EnemyController> _scoutFlockList = new List<EnemyController>();
    [SerializeField]
    List<EnemyController> _sentinelFlockList = new List<EnemyController>();
    [SerializeField]
    private Transform _flockSpawn;
    [SerializeField]
    private Transform _flockTarget;
    private Queue<EnemyController> _flockQueue = new Queue<EnemyController>();

    public void SpawnFlock(int ScoutDroneAmountToSpawn, int SentinelAmountToSpawn)
    {
        CreateEnemyPools(ScoutDroneAmountToSpawn, SentinelAmountToSpawn);

        Vector3 spawnPos = _flockSpawn.position;
        for (int i = 0; i < ScoutDroneAmountToSpawn; i++)
        {
            EnemyController scoutDroneEnemy = PoolSystem.GetNext(_scoutDrone) as EnemyController;
            scoutDroneEnemy.transform.position = spawnPos;
            _scoutFlockList.Add(scoutDroneEnemy);
            _flockQueue.Enqueue(scoutDroneEnemy);
            scoutDroneEnemy.gameObject.SetActive(true);
            spawnPos.z++;
        }

        for (int i = 0; i < SentinelAmountToSpawn; i++)
        {
            EnemyController sentinel = PoolSystem.GetNext(_sentinel) as EnemyController;
            sentinel.transform.position = spawnPos;
            _sentinelFlockList.Add(sentinel);
            _flockQueue.Enqueue(sentinel);
            sentinel.gameObject.SetActive(true);
            spawnPos.z++;
        }
    }

    public void CreateEnemyPools(int ScoutDroneAmountToPool, int SentinelAmountToPool)
    {
        PoolSystem.CreatePool(_scoutDrone, ScoutDroneAmountToPool);
        PoolSystem.CreatePool(_sentinel, SentinelAmountToPool);
    }

    private void Update()
    {
        if (_flockQueue.Count == 0) return;
        int i = 0;
        while (i < 10)
        {
            EnemyController enemy = _flockQueue.Dequeue();
            enemy.MovementHandler(_flockTarget, _scoutFlockList);
            enemy.MovementHandler(_flockTarget, _sentinelFlockList);
            _flockQueue.Enqueue(enemy);
            i++;
        }
    }

    public EnemyController GetEnemy(EnemyController enemyPrefab)
    {
        EnemyController enemy = null;
        if(enemyPrefab == _scoutDrone)
        {
            int lastListElement = _scoutFlockList.Count - 1;
            enemy = _scoutFlockList[lastListElement];
            _scoutFlockList.RemoveAt(lastListElement);            
        }
        else if (enemyPrefab == _sentinel)
        {
            int lastListElement = _scoutFlockList.Count - 1;
            enemy = _sentinelFlockList[lastListElement];
            _sentinelFlockList.RemoveAt(lastListElement);
        }
        return enemy;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_flockTarget.position, 2f);
    }
}
