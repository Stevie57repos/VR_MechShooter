using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttackHandler 
{
    public void Setup(AttackStats stats);
    public void HandleAttack(Transform target, IEnemyMovementHandler movementHandler, List<EnemyController> enemiesInWave);
    public void EMPStun();
    public void AttackTarget(Transform target);

    public void StopAttack();
}
