using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyController : PoolableObject, IDamageable
{
    [SerializeField]
    protected StatsSO _enemyStats;
    [SerializeField]
    protected HealthHandler _healthHandler;
    [SerializeField]
    protected IEnemyAttackHandler _attackHandler;
    [SerializeField]
    protected IEnemyMovementHandler _enemyMovementHandler;
    [SerializeField]
    private EnemyDeathEventSO _death;
    
    private Coroutine _currentState;
    protected List<EnemyController> _enemiesInWave;
    [SerializeField]
    protected EnemyState _enemyState;

    protected override void OnEnable()
    {
        base.OnEnable();
        SetupEnemyController();
    }

    protected void SetupEnemyController()
    {
        _healthHandler = GetComponent<HealthHandler>();
        _healthHandler.Setup(_enemyStats);
        _enemyMovementHandler = GetComponent<IEnemyMovementHandler>();
        _enemyMovementHandler.Setup(_enemyStats.MovementStats);
        _attackHandler = GetComponent<IEnemyAttackHandler>();
        _attackHandler.Setup(_enemyStats.AttackStats);
    }

    public void TakeDamage(float damage)
    {
        bool isAlive = _healthHandler.TakeDamage(damage);
        if(!isAlive)
        {
            _death.RaiseEvent(this);
            this.gameObject.SetActive(false);
        }
    }

    public virtual void AttackHandler(Transform target, List<EnemyController> enemiesInWave)
    {
        _attackHandler.HandleAttack(target, _enemyMovementHandler, enemiesInWave);
    }

    public virtual void MovementHandler(Transform target, List<EnemyController> enemiesList)
    {
        _enemyMovementHandler.HandleMovement(target, enemiesList);
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
}
