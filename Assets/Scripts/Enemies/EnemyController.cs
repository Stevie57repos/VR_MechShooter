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
    
    private Coroutine _currentState;
    protected List<EnemyController> _enemiesInWave;


    protected override void OnEnable()
    {
        base.OnEnable(); 
        _health = _enemyStats.maxHealth;
        _maxHealth = _enemyStats.maxHealth;

        SetState(State_Flocking());  
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

    public virtual void AttackTarget(Vector3 targetPos, List<EnemyController> enemiesInWave)
    {
        _enemiesInWave = enemiesInWave; 
    }

    public virtual void MoveTowardsTarget(Vector3 targetPos)
    {

    }

    public virtual void Seperate(List<EnemyController> flockList)
    {
      
    }

    protected void SetState(IEnumerator newState)
    {
        if (_currentState != null)
        {
            StopCoroutine(_currentState);
        }

        _currentState = StartCoroutine(newState);
    }

    protected IEnumerator State_Flocking()
    {
        yield return null;
    }
    protected virtual IEnumerator State_Attack()
    {
        yield return null;
    }

    // for debugging death state
    [ContextMenu("Kill Unit")]
    private void KillUnit()
    {
        TakeDamage(_maxHealth);
    }
}
