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
    private Transform _target;

    [SerializeField]
    private Animator _animator;

    

    private void Awake()
    {
        _flyingMovementController.SetUpMovementController(_enemyStats);
    }

    private void Start()
    {
        _animator.SetTrigger("SetIdle");
    }

    public override void AttackTarget(Vector3 targetPos, List<EnemyController> enemiesInWave)
    {
        base.AttackTarget(targetPos, enemiesInWave);
        SetState(State_Attack());
    }

    public override void MoveTowardsTarget(Vector3 targetPos)
    {
        _flyingMovementController.FlyTowardsTarget(targetPos);
    }

    public override void Seperate(List<EnemyController> flockList)
    {
        _flyingMovementController.Seperate(flockList);
    }

    protected override IEnumerator State_Attack()
    {
        while(_target != null)
        {
            MoveTowardsTarget(_target.position);
            Seperate(_enemiesInWave);
            yield return null;
        }
    }
}
