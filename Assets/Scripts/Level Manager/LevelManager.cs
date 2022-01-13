using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoboRyanTron.SceneReference;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private PlayerDeathEvenChannelSO _playerDeathEventChannel;
    [SerializeField]
    private SceneReference _lossScene;

    [SerializeField]
    private EnemyWaveClearedEventChannelSO _enemyWaveClearedEventChannel;
    [SerializeField]
    private SceneReference _WinScene;


    private void Awake()
    {
        StartLevel();
    }
    private void OnEnable()
    {
        _spawnTimerEventChannel.WaveSpawnEvent += SpawnWaveTimerCallback;
        _playerDeathEventChannel.PlayerDeathEvent += PlayerLoss;
        _enemyWaveClearedEventChannel.EnemyWaveClearedEvent += PlayerWin; 
    }

    private void OnDisable()
    {
        _spawnTimerEventChannel.WaveSpawnEvent -= SpawnWaveTimerCallback;
        _playerDeathEventChannel.PlayerDeathEvent -= PlayerLoss;
        _enemyWaveClearedEventChannel.EnemyWaveClearedEvent -= PlayerWin;
    }

    private void StartLevel()
    {
        int levelScoutDroneTotal = _levelData.GetEnemyPoolAmount(_scoutDrone);
        int levelSentinelTotal = _levelData.GetEnemyPoolAmount(_sentinel);
        Debug.Log($" total scout drone for level is {levelScoutDroneTotal} || " +
            $"total sentinal for level is {levelSentinelTotal}");
        int scoutPoolAmount = levelScoutDroneTotal > _minimumPoolAmount ? levelScoutDroneTotal: _minimumPoolAmount;
        int sentinelPoolAmount = levelSentinelTotal > _minimumPoolAmount ? levelSentinelTotal: _minimumPoolAmount;
        _enemyManager.CreateEnemyFlock(_target, scoutPoolAmount, sentinelPoolAmount );

        _levelTimer.StartLevelTimer(_levelData);
    }

    private void SpawnWave(int enemyWaveNumber)
    {
        _currentWave = enemyWaveNumber;
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

    private void PlayerWin()
    {
        _currentWave++;

        if( _currentWave == _levelData._waveDataList.Count)
        {
            // you won the game
            StopGameManagers();
            _WinScene.LoadSceneAsync();
        }
        else
        {
            SpawnWave(_currentWave);
        }
    }

    public void PlayerLoss()
    {
        StopGameManagers();
        _lossScene.LoadSceneAsync();
    }

    private void StopGameManagers()
    {
        _levelTimer.Stop();
        _enemyManager.Stop();
    }
}