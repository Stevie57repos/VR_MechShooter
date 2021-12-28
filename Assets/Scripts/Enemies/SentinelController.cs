using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelController : EnemyController
{
    [Header("Tentacle Target")]
    [SerializeField]
    private List<Transform> tentacleList = new List<Transform>();

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Rigidbody _rigidBody;

    private Coroutine _currentState = null;

    [SerializeField]
    protected Transform _target = null;
    private Vector3 _targetDirection;

    [SerializeField]
    Vector3 debugPosition;

    float rotationSpeed = 100f;

    private void Start()
    {
        _animator.SetTrigger("SetIdle");
    }

    private void Update()
    {

    }

    public void SeekTarget(Vector3 targetPos)
    {
        _targetDirection = targetPos - transform.position;
        if(_targetDirection.magnitude > _enemyStats.TargetDistanceSlowDown)
        {
            Vector3 desiredSpeed = _targetDirection.normalized * _enemyStats.TopSpeed;
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            steer = Vector3.ClampMagnitude(steer, _enemyStats.TopSpeed);
            _rigidBody.AddForce(steer);
        }
        else
        {
            float percentageValueSpeed = Mathf.InverseLerp(_enemyStats.TargetDistanceLimit, _enemyStats.TargetDistanceSlowDown, _targetDirection.magnitude);
            Vector3 desiredSpeed = _targetDirection.normalized * (_enemyStats.TopSpeed * percentageValueSpeed);
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(steer);
        }

        Vector3 _targetRotation = targetPos - transform.position;
        Quaternion rotTarget = Quaternion.LookRotation(_targetRotation);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotTarget, Time.deltaTime * rotationSpeed);
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
        while(_target == null)
        {
            _animator.SetTrigger("SetIdle");
            yield return null;
        }
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
