using System;
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

    [Header("Enemy Pooling settings")]
    [SerializeField]
    private EnemyController _scoutDrone;
    [SerializeField]
    private EnemyController _sentinel;
    [SerializeField]
    private int _minimumPoolAmount;

    private void Awake()
    {
        _levelTimer.StartLevelTimer(_levelData);
        SetupEnemyManager();     
     }

    private void SetupEnemyManager()
    {
        int levelScoutDroneTotal = _levelData.GetEnemyPoolAmount(_scoutDrone);
        int levelSentinelTotal = _levelData.GetEnemyPoolAmount(_sentinel);
        Debug.Log($" total scout drone for level is {levelScoutDroneTotal} || " +
            $"total sentinal for level is {levelSentinelTotal}");
        int scoutPoolAmount = levelScoutDroneTotal > _minimumPoolAmount ? levelScoutDroneTotal: _minimumPoolAmount;
        int sentinelPoolAmount = levelSentinelTotal > _minimumPoolAmount ? levelSentinelTotal: _minimumPoolAmount;
        _enemyManager.SetUpEnemyManager(_target, scoutPoolAmount, sentinelPoolAmount );
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
