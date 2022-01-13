using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    [SerializeField]
    private float _health;
    private float _minHealth = 0f;
    private float _maxHealth;

    public void Setup(StatsSO stats)
    {
        _health = stats.HealthStats.maxHealth;
        _maxHealth = stats.HealthStats.maxHealth;
    }

    public bool TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= _minHealth)
        {
            _health = _minHealth;
            return false;
        }
        return true;
    }


    public bool HealthStatus()
    {
        return _health > 0;
    }
}
