using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Player Death Event Channel")]
public class PlayerDeathEvenChannelSO : ScriptableObject
{
    public UnityAction PlayerDeathEvent;

    public void RaiseEvent()
    {
        if(PlayerDeathEvent != null)
        {
            PlayerDeathEvent.Invoke();
        }
        else
        {
            Debug.Log($"{this.name} event was requested but no listeners picked up");
        }
    }
}
