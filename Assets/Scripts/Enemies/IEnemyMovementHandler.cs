using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMovementHandler 
{
    public void Setup(MovementStats stats);
    public void HandleMovement(Transform target, List<EnemyController> enemiesInWave);
}
