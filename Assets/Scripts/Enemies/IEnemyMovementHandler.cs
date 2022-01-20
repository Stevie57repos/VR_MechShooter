using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovementHandler 
{
    public void Setup(MovementStats stats);
    public void FlockingMovement(Transform target, List<EnemyController> enemiesInWave);
    public void AttackMovement(Transform target, List<EnemyController> enemiesInWave);
    public void StopMovement();
    public void KnockBack(Vector3 force);
}
