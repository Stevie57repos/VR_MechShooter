using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackHandler : MonoBehaviour, IEnemyAttackHandler
{
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
    private IEnumerator Attack()
    {
        // move towards target
        while(_target != null)
        {
            _movementHandler.HandleMovement(_target, _enemiesInWave);
            yield return null;
        }
    }
}
