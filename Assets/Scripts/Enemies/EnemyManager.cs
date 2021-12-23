using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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

    private void OnEnable()
    {
        _deathEventChannel.EnemyDeathEvent += EnemyDeathEvent;
    }

    private void OnDisable()
    {
        _deathEventChannel.EnemyDeathEvent -= EnemyDeathEvent;
    }
    private void Start()
    {
        SpawnEnemies(_spawnAmount, _enemy, _spawnPositionList);
    }

    public void SpawnEnemies(int spawnAmount, EnemyController enemyPrefab, List<Transform> spawnLocation)
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            EnemyController enemy = Instantiate(enemyPrefab, spawnLocation[i].position, Quaternion.identity);
            _currentEnemylist.Add(enemy);
        }
    }

    public void SpawnEnemies(int spawnAmount, EnemyController enemyPrefab, Vector3 spawnLocation)
    {
        EnemyController enemy = Instantiate(enemyPrefab, spawnLocation, Quaternion.identity);
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
        SpawnEnemies(1, _enemy, spawnPosition);
    }
}
