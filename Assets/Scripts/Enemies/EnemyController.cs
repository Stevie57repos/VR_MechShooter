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

    private bool reachedDestination;

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

    public void BeginAttack(Transform target, List<EnemyController> enemyList)
    {
        _target = target;
        _enemiesInWave = enemyList;
        reachedDestination = false;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {                
        while (!reachedDestination)
        {
            _enemyMovementHandler.FlyTowards(_target.position, _enemiesInWave, out reachedDestination);
            yield return null;
        }
        AttackPlayer();
    }

    private void AttackPlayer()
    {
        StopCoroutine(AttackRoutine());
        _enemyMovementHandler.StopMovement();
        print($"reached target");
    }

    public void FlockingMovement(Vector3 targetPosition, List<EnemyController> enemyList)
    {
        _enemyMovementHandler.FlyTowards(targetPosition, enemyList);
    }

    public void TakeDamage(float damage)
    {
        bool isAlive = _healthHandler.TakeDamage(damage);
        if (!isAlive)
        {
            _deathEventChannel.RaiseEvent(this);
            this.gameObject.SetActive(false);
        }
        else
        {
            _audioSource.PlayOneShot(_takeDamageAudioClip);
            _electricalEffectsChannelSO.RaiseEvent(this.transform);
        }
    }

    // overload for knockback option
    public void TakeDamage(float damage, Vector3 knockBack)
    {
        TakeDamage(damage);
        _enemyMovementHandler.KnockBack(knockBack);
    }
}
