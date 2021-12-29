using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{   
    private float secondslevelTimeElapsed;
    private int _curentWaveSpawnTracker = 1;
    private List<float> _spawnWaveTimeTriggers = new List<float>();
    private float _maxLevelDuration;
    [SerializeField]
    private SpawnTimerEventChannelSO _spawnWaveEvent;

    void Update()
    {
        secondslevelTimeElapsed += Time.deltaTime;
    }

    public void StartLevelTimer(LevelData levelData)
    {
        _spawnWaveTimeTriggers = levelData.GetSpawnWaveTimesList();
        _maxLevelDuration = levelData.maxLevelDuration;
        StartCoroutine(CheckTimer());
    }

    private IEnumerator CheckTimer()
    {
        while(secondslevelTimeElapsed < _maxLevelDuration && _spawnWaveTimeTriggers.Count > 1)
        {
            if (secondslevelTimeElapsed < _spawnWaveTimeTriggers[_curentWaveSpawnTracker])
                yield return null;
            else
            {
                Debug.Log($"current time : {secondslevelTimeElapsed} is greater than {_spawnWaveTimeTriggers[_curentWaveSpawnTracker]} ");
                Debug.Log($"spawn event");
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
