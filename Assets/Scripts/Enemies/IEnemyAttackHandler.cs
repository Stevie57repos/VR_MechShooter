using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyAttackHandler 
{

    public void HandleAttack(Transform target, IEnemyMovementHandler movementHandler, List<EnemyController> enemiesInWave);

}
