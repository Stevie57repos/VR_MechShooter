using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{   
    [SerializeField]
    private float secondsLevelTimeElapsed;
    private int _curentWaveSpawnTracker = 0;
    private List<float> _spawnWaveTimeTriggers = new List<float>();
    private float _maxLevelDuration;
    [SerializeField]
    private SpawnTimerEventChannelSO _spawnWaveEvent;

    void Update()
    {
        secondsLevelTimeElapsed += Time.deltaTime;
    }

    public void StartLevelTimer(LevelData levelData)
    {
        _spawnWaveTimeTriggers = levelData.GetSpawnWaveTimesList();
        _maxLevelDuration = levelData.maxLevelDuration;
        StartCoroutine(CheckTimer());
    }

    private IEnumerator CheckTimer()
    {
        while(secondsLevelTimeElapsed < _maxLevelDuration && _spawnWaveTimeTriggers.Count != 0)
        {
            if (secondsLevelTimeElapsed < _spawnWaveTimeTriggers[_curentWaveSpawnTracker])
                yield return null;
            else
            {
                _spawnWaveEvent.RaiseEvent(_curentWaveSpawnTracker);
                _curentWaveSpawnTracker++;

                if (_curentWaveSpawnTracker >= _spawnWaveTimeTriggers.Count)
                {
                    Debug.Log($" no more spawn timers and stopping coroutine");
                    yield break;
                }
            }                  
        }
    }
}
