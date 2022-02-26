using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerBackup : PoolableObject, IDamageable
{
    [SerializeField]
    protected StatsSO _enemyStats;
    [SerializeField]
    protected HealthHandler _healthHandler;
    protected IEnemyAttackHandler _attackHandler;
    protected IEnemyMovementHandler _enemyMovementHandler;
    [SerializeField]
    private EnemyDeathEventSO _deathEventChannel;
    protected List<EnemyController> _enemiesInWave;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _takeDamageAudioClip;
    private Transform _target;
    [SerializeField]
    private ElectricalEffectsChannelSO _electricalEffectsChannelSO;

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
        if (!isAlive)
        {
            //_deathEventChannel.RaiseEvent(this);
            this.gameObject.SetActive(false);
        }
        else
        {
            _audioSource.PlayOneShot(_takeDamageAudioClip);
            _electricalEffectsChannelSO.RaiseEvent(this.transform);
        }
    }

    [ContextMenu("Debug Take Damage")]
    public void DebugTakeDamage()
    {
        Vector3 knockBack = transform.position - _target.position;
        knockBack = knockBack.normalized * 5f;
        TakeDamage(1, knockBack);
    }

    public void TakeDamage(float damage, Vector3 knockBack)
    {
        bool isAlive = _healthHandler.TakeDamage(damage);
        if (!isAlive)
        {
            //_deathEventChannel.RaiseEvent(this);
            this.gameObject.SetActive(false);
        }
        else
        {
            _audioSource.PlayOneShot(_takeDamageAudioClip);
            _electricalEffectsChannelSO.RaiseEvent(this.transform);
            _enemyMovementHandler.KnockBack(knockBack);
        }
    }

    public virtual void AttackHandler(Transform target, List<EnemyController> enemiesInWave)
    {
        _attackHandler.HandleAttack(target, _enemyMovementHandler, enemiesInWave);
        _target = target;
    }

    public virtual void MovementHandler(Transform target, List<EnemyController> enemiesInWave)
    {
        //_enemyMovementHandler.FlockingMovement(target, enemiesInWave);
    }

    public void AttackPlayer(Transform target, List<EnemyController> enemiesInWave)
    {
        _enemyMovementHandler.StopMovement();
        AttackHandler(target, enemiesInWave);
    }

    [ContextMenu("EMP Stun")]
    public void EmpStun()
    {
        _attackHandler.EMPStun();
        _enemyMovementHandler.StopMovement();
        Vector3 FlyBackDistance = (transform.position - _target.position).normalized * 2f;
        _enemyMovementHandler.KnockBack(FlyBackDistance);
        _electricalEffectsChannelSO.RaiseEvent(this.transform);
    }

    // for debugging death state
    [ContextMenu("Kill Unit")]
    private void KillUnit()
    {
        TakeDamage(_enemyStats.HealthStats.maxHealth);
    }

    bool IDamageable.TakeDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}
