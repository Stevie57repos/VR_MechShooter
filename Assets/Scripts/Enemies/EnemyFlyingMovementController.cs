using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovementController : MonoBehaviour, IEnemyMovementHandler
{
    [SerializeField]
    private Rigidbody _rigidBody;
    protected Transform _target = null;
    private MovementStats _movementStats;
    [SerializeField]
    private Vector3 _targetDirection;
    [SerializeField]
    private Vector3 _newTargetPos;
    private bool _targetSet = false;
    [SerializeField]
    private float _rotationSpeed;
    private List<EnemyController> _enemiesInWave;

    public void Setup(MovementStats stats)
    {        
        _movementStats = stats; 
    }
    public void FlockingMovement(Transform target, List<EnemyController> enemiesInWave)
    {
        _target = target;
        _enemiesInWave = enemiesInWave;
        FlyTowardsTarget(target.position);
        Seperate(enemiesInWave);
    }

    public void AttackMovement(Transform target, List<EnemyController> enemiesInWave)
    {
        _target = target;
        _enemiesInWave = enemiesInWave;
        AttackMovement(target.position);
        Seperate(enemiesInWave);
    }

    public void FlyTowardsTarget(Vector3 targetPos)
    {
        _targetDirection = targetPos - transform.position;

        if (_targetDirection.magnitude > _movementStats.TargetDistanceSlowDown)
        {
            Vector3 desiredSpeed = _targetDirection.normalized * _movementStats.TopSpeed;
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            steer = Vector3.ClampMagnitude(steer, _movementStats.TopSpeed);
            _rigidBody.AddForce(steer);
        }
        else if (_targetDirection.magnitude >_movementStats.TargetDistanceLimit)
        {
            float percentageValueSpeed = Mathf.InverseLerp(_movementStats.TargetDistanceLimit, _movementStats.TargetDistanceSlowDown, _targetDirection.magnitude);
            Vector3 desiredSpeed = _targetDirection.normalized * (_movementStats.TopSpeed * percentageValueSpeed);
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(steer);
        }
        else if(_targetDirection.magnitude < _movementStats.TargetDistanceLimit)
        {
            Vector3 reverseDirection = transform.position - targetPos;
            float percentageValueSpeed = Mathf.InverseLerp(0f, _movementStats.TargetDistanceLimit, _targetDirection.magnitude);
            Vector3 desiredSpeed = reverseDirection.normalized * (_movementStats.TopSpeed * percentageValueSpeed);
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(steer);
        }

        Vector3 _targetRotation = targetPos - transform.position;
        Quaternion rotTarget = Quaternion.LookRotation(_targetRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Time.deltaTime * _rotationSpeed);
    }

    public void AttackMovement(Vector3 targetPos)
    {
        // get within attack range
        // attack range is at target transform y + 2 and at least z + 2

        if (!_targetSet)
        {
            _newTargetPos = targetPos;
            _newTargetPos.y += 10;
            _newTargetPos.z += 10;
            _targetSet = true;
        }
        _targetDirection = _newTargetPos - transform.position;

        // if outside of slow distance move at normal speed
        if (_targetDirection.magnitude > _movementStats.TargetDistanceSlowDown)
        {
            Vector3 desiredSpeed = _targetDirection.normalized * _movementStats.TopSpeed;
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            steer = Vector3.ClampMagnitude(steer, _movementStats.TopSpeed);
            _rigidBody.AddForce(steer);
        }
        // start slowing down
        else if (_targetDirection.magnitude > _movementStats.TargetDistanceLimit)
        {
            float percentageValueSpeed = Mathf.InverseLerp(_movementStats.TargetDistanceLimit, _movementStats.TargetDistanceSlowDown, _targetDirection.magnitude);
            Vector3 desiredSpeed = _targetDirection.normalized * (_movementStats.TopSpeed * percentageValueSpeed);
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(steer);
        }
        // if too close, move backwards to get to the ideal distance
        else if (_targetDirection.magnitude < _movementStats.TargetDistanceLimit)
        {
            Vector3 reverseDirection = transform.position - targetPos;
            float percentageValueSpeed = Mathf.InverseLerp(0f, _movementStats.TargetDistanceLimit, _targetDirection.magnitude);
            Vector3 desiredSpeed = reverseDirection.normalized * (_movementStats.TopSpeed * percentageValueSpeed);
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(steer);
        }

        Vector3 _targetRotation = targetPos - transform.position;
        Quaternion rotTarget = Quaternion.LookRotation(_targetRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Time.deltaTime * _rotationSpeed);
    }

    public void Seperate(List<EnemyController> flockList)
    {
        if (flockList.Count == 0) return;
        Vector3 directionSum = Vector3.zero;
        foreach (EnemyController flock in flockList)
        {
            float distance = Vector3.Distance(transform.position, flock.transform.position);
            if ((distance > 0) && (distance < _movementStats.DesiredSeperationDistance) && flock != this)
            {
                Vector3 oppositeDireciton = (transform.position - flock.transform.position).normalized;
                directionSum += oppositeDireciton;
            }

            directionSum *= (_movementStats.TopSpeed * Time.fixedDeltaTime);
            _rigidBody.velocity += directionSum;
            _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _movementStats.TopSpeed);
        }
    }

    public void StopMovement()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_targetDirection, 1f);
    }
}
