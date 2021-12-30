using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyController : PoolableObject, IDamageable
{
    [SerializeField]
    private float _health;
    private float _minHealth = 0f;
    private float _maxHealth;
    [SerializeField]
    protected StatsSO _enemyStats;
    [SerializeField]
    private EnemyDeathEventSO _death;
    
    public EnemyState CurrentState;


    protected override void OnEnable()
    {
        base.OnEnable(); 
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
            this.gameObject.SetActive(false);
        }
    }

    public virtual void AttackTarget(Vector3 targetPos)
    {

    }

    public virtual void MoveTowardsTarget(Vector3 targetPos)
    {

    }

    public virtual void Seperate(List<EnemyController> flockList)
    {
      
    }

    // for debugging death state
    [ContextMenu("Kill Unit")]
    private void KillUnit()
    {
        TakeDamage(_maxHealth);
    }
}
