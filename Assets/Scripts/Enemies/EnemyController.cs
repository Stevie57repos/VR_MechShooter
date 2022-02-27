using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class EnemyController : PoolableObject, IDamageable
{
    [SerializeField]
    protected StatsSO _enemyStats;
    protected HealthHandler _healthHandler;
    
    // player target
    private Transform _target;
    protected IEnemyAttackHandler _attackHandler;
    protected IEnemyMovementHandler _enemyMovementHandler;
    protected List<EnemyController> _enemiesInWave;

    private bool _reachedDestination;
    private Coroutine _currentState;

    [Header("Audio")]
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _takeDamageAudioClip;

    [Header("Event Channels")]
    [SerializeField]
    private EnemyDeathEventSO _deathEventChannel;
    [SerializeField]
    private ElectricalEffectsChannelSO _electricalEffectsChannelSO;

    private void Awake()
    {
        Setup();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StopAllCoroutines();
        _reachedDestination = false;
    }

    private void Setup()
    {
        _healthHandler = GetComponent<HealthHandler>();
        _healthHandler.Setup(_enemyStats);
        _enemyMovementHandler = GetComponent<IEnemyMovementHandler>();
        _enemyMovementHandler.Setup(_enemyStats.MovementStats);
        _attackHandler = GetComponent<IEnemyAttackHandler>();
        _attackHandler.Setup(_enemyStats.AttackStats);
    }

    public void BeginPlayerAttack(Transform target, List<EnemyController> enemyList)
    {
        _target = target;
        _enemiesInWave = enemyList;
        _reachedDestination = false;
        SetState(ApproachPlayerRoutine());
    }

    private IEnumerator ApproachPlayerRoutine()
    {                
        while (!_reachedDestination)
        {
            _enemyMovementHandler.FlyTowards(_target.position, _enemiesInWave, out _reachedDestination);
            yield return null;
        }
        _enemyMovementHandler.StopMovement();
        print($"reached target");
        SetState(AttackPlayerRoutine());
    }

    private IEnumerator AttackPlayerRoutine()
    {
        bool inAttackRange = Vector3.Distance(_target.position, transform.position) < _enemyStats.AttackStats.AttackDistance;
        if (!inAttackRange) 
        {
            _attackHandler.StopAttack();
            SetState(ApproachPlayerRoutine());
        }

        while (inAttackRange)
        {
            _attackHandler.AttackTarget(_target);
            yield return null;
        }        
    }

    public void FlockingMovement(Vector3 targetPosition, List<EnemyController> enemyList)
    {
        _enemyMovementHandler.FlyTowards(targetPosition, enemyList);
    }

    public bool TakeDamage(float damage)
    {
        bool isAlive = _healthHandler.TakeDamage(damage);
        if (!isAlive)
        {
            _deathEventChannel.RaiseEvent(this);
            StopAllCoroutines();
            _attackHandler.StopAttack();
            _enemyMovementHandler.StopMovement();
            this.gameObject.SetActive(false);
        }
        else
        {
            _audioSource.PlayOneShot(_takeDamageAudioClip);
            _electricalEffectsChannelSO.RaiseEvent(this.transform);
        }
        return isAlive;
    }

    // overload for knockback option
    public void TakeDamage(float damage, Vector3 knockBack)
    {
        bool isAlive = TakeDamage(damage);
        if (!isAlive) return;
        _enemyMovementHandler.KnockBack(knockBack);
        _attackHandler.StopAttack();
        Invoke("RestartAttack", 1f);
    }

    public void EMPStun(float damage, Vector3 knockBack, float stunDuration)
    {
        bool isAlive = TakeDamage(damage);
        if (!isAlive) return;
        _enemyMovementHandler.KnockBack(knockBack);
        _attackHandler.StopAttack();
        Invoke("RestartAttack", stunDuration);        
    }

    private void SetState(IEnumerator newState)
    {
        if (_currentState != null)
        {
            StopCoroutine(_currentState);
        }

        _currentState = StartCoroutine(newState);
    }

    private void RestartAttack()
    {
        if(!_healthHandler.IsAlive()) return;
        StopAllCoroutines();
        BeginPlayerAttack(_target, _enemiesInWave);
    }

    public void StopAttack()
    {
        _currentState = null;
        _attackHandler.StopAttack();
        _enemyMovementHandler.StopMovement();
        StopAllCoroutines();
    }
}
