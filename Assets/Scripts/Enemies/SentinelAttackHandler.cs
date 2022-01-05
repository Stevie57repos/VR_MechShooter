using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelAttackHandler : MonoBehaviour, IEnemyAttackHandler
{
    private AttackStats _stats;
    private Transform _target;
    private IEnemyMovementHandler _movementHandler;
    private List<EnemyController> _enemiesInWave;

    public void HandleAttack(Transform target, IEnemyMovementHandler movementHandler, List<EnemyController> enemiesInWave)
    {
        _target = target;
        _movementHandler = movementHandler;
        _enemiesInWave = enemiesInWave;
        StartCoroutine(Attack());
    }

    public void Setup(AttackStats stats)
    {
        _stats = stats;
    }

    private IEnumerator Attack()
    {
        // move towards target
        while (_target != null)
        {
            _movementHandler.HandleMovement(_target, _enemiesInWave);
            yield return null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, _target.position);
        Gizmos.DrawSphere(transform.position, 3f);
    }
}
