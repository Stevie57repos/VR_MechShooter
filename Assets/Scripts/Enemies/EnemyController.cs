using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float _health;
    private float _minHealth = 0f;
    private float _maxHealth;
    [SerializeField]
    protected StatsSO _enemyStats;
    [SerializeField]
    private EnemyDeathEventSO _death;

    private void OnEnable()
    {
        _health = _enemyStats.maxHealth;
        _maxHealth = _enemyStats.maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= _minHealth)
        {
            _health = _minHealth;
            _death.RaiseEvent(this);
            Destroy(gameObject);
        }
    }

    // for debugging death state
    [ContextMenu("Kill Unit")]
    private void KillUnit()
    {
        TakeDamage(_maxHealth);
    }
}
