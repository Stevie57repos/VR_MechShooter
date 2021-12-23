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
    private HealthStatsSO _enemyStats;
    [SerializeField]
    private EnemyDeathEventSO _death;

    private Coroutine _currentState = null;

    [SerializeField]
    protected Transform _target = null;

    protected virtual void Start()
    {
        SetState(State_Idle());
    }

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

    protected void SetState(IEnumerator newState)
    {
        if(_currentState != null)
        {
            StopCoroutine(_currentState);
        }

        _currentState = StartCoroutine(newState);
    }

    protected virtual IEnumerator State_Spawning()
    {
        yield return null;
    }

    protected virtual IEnumerator State_Idle()
    {
        while(_target == null)
            yield return null;

        SetState(PursueTarget());
    }
    protected virtual IEnumerator PursueTarget()
    {
        yield return null;
    }

}
