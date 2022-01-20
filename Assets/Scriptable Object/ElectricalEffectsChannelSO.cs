using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Electrical Effects Event Channel")]
public class ElectricalEffectsChannelSO : ScriptableObject
{
    public UnityAction<Transform> ElectricalEffectsEvent;
    public void RaiseEvent(Transform effectsSpawnPosition)
    {
        if (ElectricalEffectsEvent != null)
        {
            ElectricalEffectsEvent.Invoke(effectsSpawnPosition);
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
