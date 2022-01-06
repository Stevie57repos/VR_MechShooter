using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelAttackHandler : MonoBehaviour, IEnemyAttackHandler
{
    private AttackStats _stats;
    private Transform _target;
    private IEnemyMovementHandler _movementHandler;
    private List<EnemyController> _enemiesInWave;
    private Coroutine _currentState = null;
    private float _attackTimerStart = float.MinValue;
    private float _attackTimerEnd;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    List<Transform> _tentacleList;
    [SerializeField]
    ParticleSystem _attackParticles;
    public void HandleAttack(Transform target, IEnemyMovementHandler movementHandler, List<EnemyController> enemiesInWave)
    {
        _target = target;
        _movementHandler = movementHandler;
        _enemiesInWave = enemiesInWave;
        SetState(State_MoveTowardsTarget());
    }

    public void Setup(AttackStats stats)
    {
        _stats = stats;
    }

    private IEnumerator State_MoveTowardsTarget()
    {
        _attackParticles.Stop();
        Debug.Log($"in moving state");
        // move towards target
        _animator.SetTrigger("SetIdle");
        while (Vector3.Distance(transform.position, _target.position) > _stats.AttackDistance)
        {
            _movementHandler.HandleMovement(_target, _enemiesInWave);
            yield return null;
        }
        _attackTimerStart = Time.time;
        _attackTimerEnd = Time.time + _stats.AttackChargeTime;
        _attackParticles.Play();
        SetState(State_Attack());
    }

    private IEnumerator State_Attack()
    {
        Debug.Log($"in Attack State");
        _animator.SetTrigger("SetAttack");
        float currentDistance = Vector3.Distance(transform.position, _target.position);
        if (currentDistance > _stats.AttackDistance) SetState(State_MoveTowardsTarget());
        while (currentDistance < _stats.AttackDistance)
        {
            transform.LookAt(_target.position);
            _movementHandler.StopMovement();
            _attackTimerStart = Time.time;
            if (_attackTimerStart <= _attackTimerEnd)
            {
                yield return null;
            }
            else
            {
                Debug.Log($"timer started at {_attackTimerStart} and ended at {_attackTimerEnd}");
                Debug.Log($"Player takes damage !");
            }
            yield return null;
        }
    }
    private void SetState(IEnumerator newState)
    {
        if (_currentState != null)
        {
            StopCoroutine(_currentState);
        }

        _currentState = StartCoroutine(newState);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _target.position);
    }

    private void OnDisable()
    {
        
    }
}
