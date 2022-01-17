using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunRangeEnemySpawnType { Basic, Flock};
public class GunRangeEnemyManager : MonoBehaviour
{
    [SerializeField]
    private GunRangeEnemySpawnType _spawnType;
    [SerializeField]
    private int _spawnAmount;
    [SerializeField]
    EnemyController _enemy;
    [SerializeField]
    List<Transform> _spawnPositionList = new List<Transform>();
    [SerializeField]
    List<EnemyController> _currentEnemylist = new List<EnemyController>();
    [SerializeField]
    EnemyDeathEventSO _deathEventChannel;
    [SerializeField]
    private Transform _flockTarget;

    private void OnEnable()
    {
        _deathEventChannel.EnemyDeathEvent += EnemyDeathEvent;
    }

    private void OnDisable()
    {
        _deathEventChannel.EnemyDeathEvent -= EnemyDeathEvent;
    }

    private void Awake()
    {
        PoolSystem.CreatePool(_enemy, 20);
    }

    private void Start()
    {
        SpawnEnemies(_spawnAmount, _enemy, _spawnPositionList);       
    }

    private void Update()
    {
        if (_spawnType == GunRangeEnemySpawnType.Flock)
        {
            foreach (EnemyController enemy in _currentEnemylist)
            {
                //SentinelController sentinel = (SentinelController)enemy;
                //sentinel.MoveTowardsTarget(_flockTarget.position);
                //sentinel.Seperate(_currentEnemylist);

                enemy.MovementHandler(_flockTarget, _currentEnemylist);
            }
        }
    }

    public void SpawnEnemies(int spawnAmount, EnemyController enemyPrefab, List<Transform> spawnLocationList)
    {
        int locationCount = 0;
        for(int i = 0; i < spawnAmount; i++)
        {           
            Vector3 spawnLocation = new Vector3(spawnLocationList[locationCount].position.x, spawnLocationList[locationCount].position.y + 5, spawnLocationList[locationCount].position.z);
            EnemyController enemy = PoolSystem.GetNext(_enemy) as EnemyController;
            Quaternion lookDirection = Quaternion.LookRotation(_flockTarget.position - transform.position);
            enemy.transform.SetPositionAndRotation(spawnLocation, lookDirection);                       
            enemy.gameObject.SetActive(true);
            _currentEnemylist.Add(enemy);

            locationCount++;
            if (locationCount > spawnLocationList.Count - 1) locationCount = 0;
        }
    }

    public void SpawnEnemy(int spawnAmount, EnemyController enemyPrefab, Vector3 spawnLocation)
    {
        EnemyController enemy = PoolSystem.GetNext(_enemy) as EnemyController;
        enemy.gameObject.transform.position = spawnLocation;
        enemy.gameObject.SetActive(true);
        _currentEnemylist.Add(enemy);
    }

    private void EnemyDeathEvent(EnemyController enemy)
    {
        if (!_currentEnemylist.Contains(enemy)) return;

        _currentEnemylist.Remove(enemy);
        Vector3 enemyPosition = enemy.transform.position;
        StartCoroutine(RespawnEnemy(enemyPosition));
    }

    private IEnumerator RespawnEnemy(Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(3f);
        SpawnEnemy(1, _enemy, spawnPosition);
    }
}
