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
    private float _attackTimer;

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
        // move towards target
        while(Vector3.Distance(transform.position, _target.position) > _stats.AttackDistance)
        {
            _movementHandler.HandleMovement(_target, _enemiesInWave);
            yield return null;    
        }     
        _attackTimer = _stats.AttackChargeTime;
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
            if (_attackTimer > 0)
            {
                _attackTimer -= Time.deltaTime / _stats.AttackChargeTime;
                yield return null;
            }
            else
            {
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

}
