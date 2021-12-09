using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobFlockController : FlockController
{

    protected override void HandleResult(FlockCallbackResult result)
    {
        
    }

    public override void SeekTarget(Vector3 targetPos)
    {
        // if target direction squared is greater than the slowdown squared
        if (targetPos.sqrMagnitude > _flockSO.DistanceSlowDown * _flockSO.DistanceSlowDown)
        {
            Vector3 desiredSpeed = targetPos.normalized * _flockSO.TopSpeed;
            Vector3 steer = desiredSpeed - _rigidBody.velocity;
            steer = Vector3.ClampMagnitude(steer, _flockSO.TopSpeed);
            _rigidBody.AddForce(steer);
        }
        else
        {
            // Slow down when reaching the center
            float percentageValue = Mathf.InverseLerp(_flockSO.TargetDistanceSlowDown, _flockSO.DistanceSlowDown, targetPos.magnitude);
            Vector3 desiredSpeed = targetPos.normalized * (_flockSO.TopSpeed * percentageValue);
            Vector3 adjustmentSpeed = desiredSpeed - _rigidBody.velocity;
            _rigidBody.AddForce(adjustmentSpeed);
        }
    } 

    //public void Seperate(Vector3 directionSum)
    //{
    //    //Vector3 directionSum = Vector3.zero;
    //    //foreach (FlockController flock in flockList)
    //    //{
    //    //    float distance = Vector3.Distance(transform.position, flock.transform.position);
    //    //    if ((distance > 0) && (distance < _flockSO.DesiredSeperationDistance) && flock != this)
    //    //    {
    //    //        Vector3 oppositeDireciton = (transform.position - flock.transform.position).normalized;
    //    //        directionSum += oppositeDireciton;
    //    //    }

    //    //    directionSum *= (_flockSO.TopSpeed * Time.fixedDeltaTime);
    //        _rigidBody.velocity += directionSum;
    //        _rigidBody.velocity = Vector3.ClampMagnitude(_rigidBody.velocity, _flockSO.TopSpeed);
        
    //}

}
