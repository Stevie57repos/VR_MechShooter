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
        while(_target != null)
        {
            Debug.Log($" the distance is {Vector3.Distance(transform.position, _target.position)} " +
                $"and target is {_stats.AttackDistance}");

            if(Vector3.Distance(transform.position,_target.position) > _stats.AttackDistance)
            {
                _movementHandler.HandleMovement(_target, _enemiesInWave);
                yield return null;
            }
            else
            {
                _attackTimer = _stats.AttackChargeTime;
                SetState(State_Attack());
            }
        }
    }

    private IEnumerator State_Attack()
    {
        Debug.Log($"in Attack State");
        if (Vector3.Distance(transform.position, _target.position) > _stats.AttackDistance) SetState(State_MoveTowardsTarget());
        if(_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime / _stats.AttackChargeTime;
            yield return null;
        }
        else
        {
            Debug.Log($"Player takes damage !");
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
