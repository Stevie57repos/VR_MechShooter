using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class FlockController : MonoBehaviour
{
    [SerializeField]
    protected FlockSO _flockSO;
    [SerializeField]
    protected Rigidbody _rigidBody;
    protected Vector3 _targetDirection;

    [Header("Request Settings")]
    public Vector3 PreviousCellList;
    [SerializeField]
    //private bool _requesting = false;

    public bool isInitialized = false;
    public List<FlockController> _flockList;

    // jobs system
    private bool _isUsingJobs = false;
    private MovementJobSystem _movementJobSystem;
    public int _flockID;

    //private void Start()
    //{
    //    MakeRequest();
    //}

    //protected virtual void Update()
    //{
    //    MakeRequest();
    //}

    public FlockRequest MakeRequest()
    {
        FlockRequest request = new FlockRequest();
        request.AddUnitID(_flockID);
        return request;
    }
    public void HandleResult(FlockCallbackResult result)
    {
        //_requesting = false;
        SeekTarget(result.TargetPosition);

        if (result.InGridRange)
        {
            if (_isUsingJobs)
            {
                JobsSeperate(result.FlockList);
            }
            else
            {
                Seperate(result.FlockList);
            }
        }
    }

    public void QueueFull()
    {
        //_requesting = false;
    }

    public void Initialize(FlockSO flockSO, bool isUsingJobs, MovementJobSystem movementJobSystem, int flockID)
    {
        _flockSO = flockSO;
        _isUsingJobs = isUsingJobs;
        _movementJobSystem = movementJobSystem;
        _flockID = flockID;
    }

    public void SimpleFlockSystemUpdate(Vector3 targetPos, List<FlockController> flockList)
    {
        SeekTarget(targetPos);
        Seperate(flockList);
    }

    private void SeekTarget(Vector3 targetPos)
    {
        _targetDirection = targetPos - transform.position;

        if (_targetDirection.sqrMagnitude > _flockSO.DistanceSlowDown * _flockSO.DistanceSlowDown)
        {
            Vector3 desiredSpeed = _targetDirection.normalized * _flockSO.TopSpeed;
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            steer = Vector3.ClampMagnitude(steer, _flockSO.TopSpeed);
            _rigidBody.AddForce(steer);
        }
        else
        {
            float percentageValue = Mathf.InverseLerp(_flockSO.TargetDistanceSlowDown, _flockSO.DistanceSlowDown, _targetDirection.magnitude);
            Vector3 desiredSpeed = _targetDirection.normalized * (_flockSO.TopSpeed * percentageValue);
            Vector3 adjustmentSpeed = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(adjustmentSpeed);
        }
    }
    private void Seperate(List<FlockController> flockList)
    {
        if (flockList == null) return;
        Vector3 directionSum = Vector3.zero;
        foreach (FlockController flock in flockList)
        {
            float distance = Vector3.Distance(transform.position, flock.transform.position);
            if ((distance > 0) && (distance < _flockSO.DesiredSeperationDistance) && flock != this)
            {
                Vector3 oppositeDireciton = (transform.position - flock.transform.position).normalized;
                directionSum += oppositeDireciton;
            }

            directionSum *= (_flockSO.TopSpeed * Time.fixedDeltaTime);
            _rigidBody.velocity += directionSum;
            _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _flockSO.TopSpeed);
        }
        // for debugging lines
        _flockList = flockList;
    }

    private void JobsSeperate(List<FlockController> flockList) => _movementJobSystem.SeperateMovementJob(flockList, this, _flockSO);

    public void JobSeperateResult(Vector3 jobResultDireciton)
    {
        _rigidBody.velocity += jobResultDireciton;
        _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _flockSO.TopSpeed);        
    }

    private void OnDrawGizmosSelected()
    {
        if (_flockList == null || _flockList.Count == 0) return;

        for (int i = 0; i < _flockList.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _flockList[i].transform.position);
        }
    }
}
