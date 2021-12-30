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

    private void Awake()
    {
        PoolSystem.CreatePool(_scoutDrone, 50);
        PoolSystem.CreatePool(_sentinel, 50);
    }

    public void SpawnFlock(int ScoutDroneAmountToSpawn, int SentinelAmountToSpawn)
    {
        Vector3 spawnPos = _flockSpawn.position;
        for (int i = 0; i < ScoutDroneAmountToSpawn; i++)
        {
            EnemyController scoutDroneEnemy = PoolSystem.GetNext(_scoutDrone) as EnemyController;
            scoutDroneEnemy.transform.position = spawnPos;
            _scoutFlockList.Add(scoutDroneEnemy);
            scoutDroneEnemy.CurrentState = EnemyState.Flocking;
            _flockQueue.Enqueue(scoutDroneEnemy);
            scoutDroneEnemy.gameObject.SetActive(true);
            spawnPos.z++;
        }

        for (int i = 0; i < SentinelAmountToSpawn; i++)
        {
            EnemyController sentinel = PoolSystem.GetNext(_sentinel) as EnemyController;
            sentinel.transform.position = spawnPos;
            _sentinelFlockList.Add(sentinel);
            sentinel.CurrentState = EnemyState.Flocking;
            _flockQueue.Enqueue(sentinel);
            sentinel.gameObject.SetActive(true);
            spawnPos.z++;
        }
    }

    private void Update()
    {
        if (_flockQueue.Count == 0) return;
        int i = 0;
        while (i < 10)
        {
            EnemyController enemy = _flockQueue.Dequeue();
            enemy.MoveTowardsTarget(_flockTarget.position);
            enemy.Seperate(_scoutFlockList);
            enemy.Seperate(_sentinelFlockList);
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
