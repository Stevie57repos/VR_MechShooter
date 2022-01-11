using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Wave Spawn Event Channel")]
public class SpawnTimerEventChannelSO : ScriptableObject
{
    public UnityAction<int> WaveSpawnEvent;
    public void RaiseEvent(int currentEnemyWave)
    {
        if (WaveSpawnEvent != null)
        {
            WaveSpawnEvent.Invoke(currentEnemyWave);
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
