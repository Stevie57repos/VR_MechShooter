using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingMovementController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _rigidBody;
    protected Transform _target = null;
    private StatsSO _enemyStats;
    private Vector3 _targetDirection;
    [SerializeField]
    private float _rotationSpeed;

    public void SetUpMovementController(StatsSO stats)
    {        
        _enemyStats = stats; 
    }

    public void FlyTowardsTarget(Vector3 targetPos)
    {
        _targetDirection = targetPos - transform.position;
        if (_targetDirection.magnitude > _enemyStats.TargetDistanceSlowDown)
        {
            Vector3 desiredSpeed = _targetDirection.normalized * _enemyStats.TopSpeed;
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            steer = Vector3.ClampMagnitude(steer, _enemyStats.TopSpeed);
            _rigidBody.AddForce(steer);
        }
        else if (_targetDirection.magnitude >_enemyStats.TargetDistanceLimit)
        {
            float percentageValueSpeed = Mathf.InverseLerp(_enemyStats.TargetDistanceLimit, _enemyStats.TargetDistanceSlowDown, _targetDirection.magnitude);
            Vector3 desiredSpeed = _targetDirection.normalized * (_enemyStats.TopSpeed * percentageValueSpeed);
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(steer);
        }
        else if(_targetDirection.magnitude < _enemyStats.TargetDistanceLimit)
        {
            Vector3 reverseDirection = transform.position - targetPos;
            float percentageValueSpeed = Mathf.InverseLerp(0f, _enemyStats.TargetDistanceLimit, _targetDirection.magnitude);
            Vector3 desiredSpeed = reverseDirection.normalized * (_enemyStats.TopSpeed * percentageValueSpeed);
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
            if ((distance > 0) && (distance < _enemyStats.DesiredSeperationDistance) && flock != this)
            {
                Vector3 oppositeDireciton = (transform.position - flock.transform.position).normalized;
                directionSum += oppositeDireciton;
            }

            directionSum *= (_enemyStats.TopSpeed * Time.fixedDeltaTime);
            _rigidBody.velocity += directionSum;
            _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _enemyStats.TopSpeed);
        }
    }
}
