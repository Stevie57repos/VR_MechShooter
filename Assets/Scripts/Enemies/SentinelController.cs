using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyFlyingMovementController))]
public class SentinelController : EnemyController
{
    [SerializeField]
    private EnemyFlyingMovementController _flyingMovementController;

    [Header("Tentacle Target")]
    [SerializeField]
    private List<Transform> tentacleList = new List<Transform>();

    [SerializeField]
    private Animator _animator;

    private Coroutine _currentState = null;

    private void Awake()
    {
        _flyingMovementController.SetUpMovementController(_enemyStats);
    }

    private void Start()
    {
        _animator.SetTrigger("SetIdle");
    }

    public override void MoveTowardsTarget(Vector3 targetPos)
    {
        _flyingMovementController.FlyTowardsTarget(targetPos);
    }
    public override void Seperate(List<EnemyController> flockList)
    {
        _flyingMovementController.Seperate(flockList);
    }

    #region State Coroutines

    private void SetState(IEnumerator newState)
    {
        if (_currentState != null)
        {
            StopCoroutine(_currentState);
        }

        _currentState = StartCoroutine(newState);
    }

    private IEnumerator State_Idle()
    {
        _animator.SetTrigger("SetIdle");
        yield return null;
    }

    private IEnumerator State_Spawning()
    {
        yield return null;
    }

    private IEnumerator PursueTarget()
    {
        yield return null;
    }

    #endregion


}
