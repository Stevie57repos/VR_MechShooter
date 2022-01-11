using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugEnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyController _enemyController;
    [SerializeField]
    Transform _target;

    [SerializeField]
    private bool isDebug;
    void Start()
    {
        if (isDebug)
        {
            List<EnemyController> list = new List<EnemyController>();
            _enemyController.AttackHandler(_target, list);
        }
    }
}
