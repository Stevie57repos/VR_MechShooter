using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private LevelData _levelData;
    [SerializeField]
    private int _currentWave;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private EnemyManager _enemyManager;
    [SerializeField]
    private LevelTimer _levelTimer;
    [SerializeField]
    private SpawnTimerEventChannelSO _spawnTimerEventChannel;


    private void Awake()
    {
        _levelTimer.StartLevelTimer(_levelData);
        _enemyManager.SetUpEnemyManager(_target);
    }

    private void Start()
    {
        //_enemyFlockManager.SpawnFlock(_scoutflockAmount, _sentinelFlockAmount);
        //SpawnWave(_currentWave);
    }

    private void OnEnable()
    {
        _spawnTimerEventChannel.WaveSpawnEvent += SpawnWaveTimerCallback;
    }

    private void OnDisable()
    {
        _spawnTimerEventChannel.WaveSpawnEvent -= SpawnWaveTimerCallback;
    }
    private void SpawnWave(int enemyWaveNumber)
    {
        _enemyManager.SpawnEnemies(_levelData._waveDataList[enemyWaveNumber]);
    }
    private void SpawnWaveTimerCallback(int timerEnemyWave)
    {
        Debug.Log($"call back triggered");
        if (_currentWave >= timerEnemyWave) return;

        _currentWave = timerEnemyWave;
        SpawnWave(_currentWave);
        Debug.Log($"spawning wave {timerEnemyWave}");
    }
}
