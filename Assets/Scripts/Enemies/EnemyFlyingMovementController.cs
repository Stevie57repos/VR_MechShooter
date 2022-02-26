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
    private float _rotationSpeed;
    private List<EnemyController> _enemiesInWave;

    [SerializeField]
    private Vector3 _steer;
    [SerializeField]
    private Vector3 _seperationVector;
    [SerializeField]
    private float _seperateScaler;

    public void Setup(MovementStats stats)
    {        
        _movementStats = stats; 
    }

    public void StopMovement()
    {
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }

    public void KnockBack(Vector3 force)
    {
        _rigidBody.velocity += force;
    }

    public Vector3 Seperate(List<EnemyController> flockList)
    {
        if (flockList.Count == 0) return Vector3.zero;
        Vector3 directionSum = Vector3.zero;
        foreach (EnemyController flock in flockList)
        {
            float distance = Vector3.Distance(transform.position, flock.transform.position);
            if ((distance > 0) && (distance < _movementStats.DesiredSeperationDistance) && flock != this)
            {
                Vector3 oppositeDireciton = (transform.position - flock.transform.position).normalized;
                float distanceScaler = InverseLerp(flock.transform.position, (flock.transform.position + oppositeDireciton * _movementStats.DesiredSeperationDistance), transform.position);
                oppositeDireciton *= distanceScaler;
                directionSum += oppositeDireciton;
            }
        }
        directionSum = directionSum * _movementStats.TopSpeed;
        //directionSum += _rigidBody.velocity;
        return directionSum;
    }

    public void FlyTowards(Vector3 targetPosition, List<EnemyController> enemiesInWave)
    {      
        _targetDirection = targetPosition - transform.position;
        _enemiesInWave = enemiesInWave; ;

        // reset values 
        _steer = Vector3.zero;
        _seperationVector = Vector3.zero;

        if (_targetDirection.magnitude > _movementStats.TargetDistanceSlowDown)
        {
            // move at full speed
            Vector3 desiredSpeed = _targetDirection.normalized * _movementStats.TopSpeed;
            _steer = desiredSpeed - _rigidBody.velocity;
        }
        else if (_targetDirection.magnitude > _movementStats.TargetDistanceLimit)
        {
            // slow down
            float percentageValueSpeed = Mathf.InverseLerp(_movementStats.TargetDistanceLimit, _movementStats.TargetDistanceSlowDown, _targetDirection.magnitude);
            Vector3 desiredSpeed = _targetDirection.normalized * (_movementStats.TopSpeed * percentageValueSpeed);
            _steer = desiredSpeed - _rigidBody.velocity;
        }
        else if (_targetDirection.magnitude < _movementStats.TargetDistanceLimit)
        {
            // reverse direction if past target

            Vector3 reverseDirection = transform.position - targetPosition;
            float percentageValueSpeed = Mathf.InverseLerp(0f, _movementStats.TargetDistanceLimit, _targetDirection.magnitude);
            Vector3 desiredSpeed = reverseDirection.normalized * (_movementStats.TopSpeed * percentageValueSpeed);
            _steer = desiredSpeed - _rigidBody.velocity;
        }

        _seperationVector = Seperate(enemiesInWave);
        _seperationVector = _seperationVector * _seperateScaler;

        _rigidBody.AddForce(_steer);
        _rigidBody.AddForce(_seperationVector);
        _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _movementStats.TopSpeed);

        RotateTowardsMovementDirection(_targetDirection);
    }

    public bool FlyTowards(Vector3 targetPosition, List<EnemyController> enemiesInWave, out bool reachedPosition)
    {
        FlyTowards(targetPosition, enemiesInWave);
        reachedPosition = Vector3.Magnitude(targetPosition - transform.position) <= _movementStats.TargetDistanceLimit;
        if (reachedPosition)
        {
            _steer = Vector3.zero;
            _seperationVector = Vector3.zero;
            StopMovement();
        }
        return reachedPosition;
    }

    private void RotateTowardsMovementDirection(Vector3 targetDirection)
    {
        Quaternion rotTarget = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Time.deltaTime * _rotationSpeed);
    }

    float InverseLerp(Vector3 pointA, Vector3 pointB, Vector3 currentPos)
    {
        Vector3 AB = pointB - pointA;
        Vector3 AV = currentPos - pointA;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
