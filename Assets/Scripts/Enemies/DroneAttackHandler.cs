using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackHandler : MonoBehaviour, IEnemyAttackHandler
{
    private AttackStats _stats;
    private Transform _target;
    private IEnemyMovementHandler _movementHandler;
    private List<EnemyController> _enemiesInWave;
    private Coroutine _currentState = null;
    private float _attackTimerStart = float.MinValue;
    private float _attackTimerEnd;
    [SerializeField]
    private ParticleSystem _attackParticles;

    private void OnDisable()
    {
        _attackParticles.Stop();
    }

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
        Debug.Log($"in moving state");
        _attackParticles.Stop();
        // move towards target
        while(Vector3.Distance(transform.position, _target.position) > _stats.AttackDistance)
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
        float currentDistance = Vector3.Distance(transform.position, _target.position);
        if ( currentDistance > _stats.AttackDistance) SetState(State_MoveTowardsTarget());
        while(currentDistance < _stats.AttackDistance)
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
                yield return new WaitForSeconds(1f);
                Debug.Log($"timer started at {_attackTimerStart} and ended at {_attackTimerEnd}");
                Debug.Log($"Player takes damage !");
                // Reset attack
                _attackTimerStart = Time.time;
                _attackTimerEnd = Time.time + _stats.AttackChargeTime;
                _attackParticles.Play();
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
}
