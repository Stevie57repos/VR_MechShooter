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
    private Vector3 _startAttackPos;
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
        Quaternion rotTarget = Quaternion.LookRotation(_targetRotation, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Time.deltaTime * _rotationSpeed);
        transform.LookAt(targetPos);
    
    }

    public void AttackMovement(Vector3 targetPos)
    {
        if (!_targetSet)
        {
            _newTargetPos = targetPos;
            _newTargetPos.y += 5;
            _newTargetPos.z += 7;
            _targetSet = true;
            _startAttackPos = transform.position;
        }
        _targetDirection = _newTargetPos - transform.position;

        // snake movement based on percent distance travelled
        float percentTravelled = InverseLerp(_startAttackPos, _newTargetPos, transform.position);
        if(percentTravelled < 0.25f)
        {
            _rigidBody.AddForce(Vector3.left * _movementStats.TopSpeed * 0.5f);
            _target.gameObject.GetComponent<FieldOfView>().ChecKWithinPlayerFOV(transform.position, out Vector3 adjustment );
            _rigidBody.AddForce(adjustment);

        }
        else if(percentTravelled >0.3f && percentTravelled < 0.5f)
        {
            _rigidBody.AddForce(Vector3.right * _movementStats.TopSpeed * 0.5f);
            _target.gameObject.GetComponent<FieldOfView>().ChecKWithinPlayerFOV(transform.position, out Vector3 adjustment);
            _rigidBody.AddForce(adjustment);
        }
        else if(percentTravelled > 0.5f && percentTravelled < 0.8f)
        {
            _target.gameObject.GetComponent<FieldOfView>().ChecKWithinPlayerFOV(transform.position, out Vector3 adjustment);
            _rigidBody.AddForce(adjustment * 2);

        }

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

    public void KnockBack(Vector3 force)
    {
        _rigidBody.velocity += force;
    }

    float InverseLerp(Vector3 pointA, Vector3 pointB, Vector3 currentPos)
    {
        Vector3 AB = pointB - pointA;
        Vector3 AV = currentPos - pointA;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
