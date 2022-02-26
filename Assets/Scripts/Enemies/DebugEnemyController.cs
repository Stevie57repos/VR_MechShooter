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
    private List<EnemyController> list = new List<EnemyController>();

    [SerializeField]
    private bool isDebug;
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        if (isDebug)
        {
            //_enemyController.AttackHandler(_target, list);
            _enemyController.BeginPlayerAttack(_target, list);
        }
    }

    private void FixedUpdate()
    {

    }
}
