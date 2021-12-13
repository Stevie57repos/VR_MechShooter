using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTesting : MonoBehaviour
{
    [SerializeField]
    FlockSO _flockSO;

    [SerializeField]
    Transform _target;

    [SerializeField]
    Rigidbody _rigidBody;

    private Vector3 _targetDirection;
    private void Update()
    {
        SeekTarget(_target.position);
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
            // Slow down when reaching the center
            float percentageValue = Mathf.InverseLerp(_flockSO.TargetDistanceSlowDown, _flockSO.DistanceSlowDown, _targetDirection.magnitude);
            Vector3 desiredSpeed = _targetDirection.normalized * (_flockSO.TopSpeed * percentageValue);
            Vector3 adjustmentSpeed = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(adjustmentSpeed);
        }
    }
}
