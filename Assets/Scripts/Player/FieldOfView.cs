using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    private float _horizontalFieldOfView;
    [SerializeField]
    private float _verticalFieldOfView;

    [SerializeField]
    private GameObject _target;
    private Vector3 PositivePositionXlimit;
    private Vector3 NegativePositionXLimit;

    private void OnDrawGizmos()
    {
        float positiveHorizontalAngle = 90 - (_horizontalFieldOfView / 2);
        Vector3 rightHorizontalBorder = HorizontalVisionVector(positiveHorizontalAngle);

        float negativeHorizontalLimit = 90 + (_horizontalFieldOfView / 2);
        Vector3 leftHorizontalBorder = HorizontalVisionVector(negativeHorizontalLimit);

        Vector3 currentPosition = _target.transform.position - transform.position;

        Vector3 xPositiveLimitPosition = rightHorizontalBorder * currentPosition.z * 2;
        xPositiveLimitPosition.y = transform.position.y;

        // draw right FOV limit
        Gizmos.color = _target.transform.position.x < xPositiveLimitPosition.x ? Color.green : Color.red;
        Gizmos.DrawSphere(xPositiveLimitPosition, 0.25f);
        Gizmos.DrawLine(transform.position, xPositiveLimitPosition);
        Gizmos.DrawCube(xPositiveLimitPosition, new Vector3(0.5f, 0.5f, 0.5f));

        // draw left FOV limit

        Vector3 xNegativeLimitPosition = leftHorizontalBorder * currentPosition.z * 2;
        xNegativeLimitPosition.y = transform.position.y;

        Gizmos.color = _target.transform.position.x > xNegativeLimitPosition.x ? Color.green : Color.red;
        Gizmos.DrawSphere(xNegativeLimitPosition, 0.25f);
        Gizmos.DrawLine(transform.position, xNegativeLimitPosition);
        Gizmos.DrawCube(xNegativeLimitPosition, new Vector3(0.5f, 0.5f, 0.5f));

        float positiveVerticallLimit = (_horizontalFieldOfView / 2);
        Vector3 TopVerticalBorder = VerticalVisionVector(positiveVerticallLimit);
        Gizmos.color = Color.green;

        Vector3 topLimitPosition = TopVerticalBorder.normalized * currentPosition.z * 2;
        if (_target.transform.position.y < topLimitPosition.y && _target.transform.position.y > 0)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawSphere(topLimitPosition, 0.25f);
        Gizmos.DrawLine(transform.position, topLimitPosition);
        Gizmos.DrawCube(topLimitPosition, new Vector3(0.5f, 0.5f, 0.5f));
    }

    public Vector3 HorizontalVisionVector(float angle)
    {
        float xRadians = (float)Mathf.Deg2Rad * angle;
        float yRadians = (float)Mathf.Deg2Rad * angle;
        Vector3 result = new Vector3(Mathf.Cos(xRadians), 0, Mathf.Sin(yRadians)).normalized;
        result.y = transform.position.y;
        return result;
    }
    public Vector3 VerticalVisionVector(float angle)
    {
        float xRadians = (float)Mathf.Deg2Rad * angle;
        float yRadians = (float)Mathf.Deg2Rad * angle;
        Vector3 result = new Vector3(0, Mathf.Sin(yRadians), Mathf.Cos(xRadians)).normalized;
        return result;
    }

    private void Awake()
    {
        CalculateFOVLimits();
    }

    private void CalculateFOVLimits()
    {
        float positiveHorizontalAngle = 90 - (_horizontalFieldOfView / 2);
        PositivePositionXlimit = CalculateXLimit(positiveHorizontalAngle);

        float negativeHorizontalAngle = 90 + (_horizontalFieldOfView / 2);
        NegativePositionXLimit = CalculateXLimit(negativeHorizontalAngle);
    }

    public bool CheckPosition(Vector3 position)
    {
        bool isInPOV = false;

        Vector3 targetPosition = position - transform.position;

        Vector3 xPositiveLimit = PositivePositionXlimit * targetPosition.z * 2 ;
        Vector3 xNegativeLimit = NegativePositionXLimit * position.z * 2;
        if( position.x < xPositiveLimit.x && position.x > xNegativeLimit.x)
            isInPOV = true;
        else
            isInPOV = false;

        return isInPOV;
    }

    private Vector3 CalculateXLimit( float Angle)
    {
        Vector3 HorizontalPositionLimit = HorizontalVisionVector(Angle);
        return HorizontalPositionLimit;
    }
}
