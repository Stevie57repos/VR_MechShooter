using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    [SerializeField]
    private float _health;

    [SerializeField]
    private List<Timer> _buffs = new List<Timer>();

    public void AddBuff(float duration, int ticks, Action onTick, Action onEnd)
    {
        _buffs.Add(new Timer(duration, ticks, onTick, onEnd));
    }

    virtual
    public void OnHit(float damage)
    {
        _health -= damage;
        _health = Mathf.Clamp(_health, 0f, 100f);
        if (_health == 0f)
        {
            OnDie();
        }
    }

    virtual
    public void OnDie()
    {
        // death stuff ehre
        Debug.Log($"Im dead {gameObject.name}", this);
    }

    protected void Update()
    {
        for (int i = _buffs.Count - 1; i >= 0; i--)
        {
            if (_buffs[i].Tick() == false)
            {
                _buffs.RemoveAt(i);
            }
        }
    }
}
