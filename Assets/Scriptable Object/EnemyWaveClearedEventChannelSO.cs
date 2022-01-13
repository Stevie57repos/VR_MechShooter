using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Enemy Wave Event Channel")]
public class EnemyWaveClearedEventChannelSO : ScriptableObject
{
    public UnityAction EnemyWaveClearedEvent;

    public void RaiseEvent()
    {
        if (EnemyWaveClearedEvent != null)
        {
            EnemyWaveClearedEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
