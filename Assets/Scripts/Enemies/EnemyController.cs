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
    protected IEnemyAttackHandler _attackHandler;
    protected IEnemyMovementHandler _enemyMovementHandler; 
    [SerializeField]
    private EnemyDeathEventSO _death;
    protected List<EnemyController> _enemiesInWave;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _takeDamageAudioClip;

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
        else
        {
            _audioSource.PlayOneShot(_takeDamageAudioClip);
        }        
    }

    public virtual void AttackHandler(Transform target, List<EnemyController> enemiesInWave)
    {
        _attackHandler.HandleAttack(target, _enemyMovementHandler, enemiesInWave);
    }

    public virtual void MovementHandler(Transform target, List<EnemyController> enemiesInWave)
    {
        _enemyMovementHandler.FlockingMovement(target, enemiesInWave);
    }
    
    public void AttackPlayer(Transform target, List<EnemyController> enemiesInWave)
    {
        _enemyMovementHandler.StopMovement();
        AttackHandler(target, enemiesInWave);
    }


    // for debugging death state
    [ContextMenu("Kill Unit")]
    private void KillUnit()
    {
        TakeDamage(_enemyStats.HealthStats.maxHealth);
    }
}
