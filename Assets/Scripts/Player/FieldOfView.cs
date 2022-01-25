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
    private float _projectedRange;
    private Vector3 PositivePositionXlimit;
    private Vector3 NegativePositionXLimit;
    private Vector3 PositivePositionYLimit;
    private Vector3 NegativePositionYlimit;
    
    private bool isInPositiveXLimits = true;
    private bool isInNegativeXLimits = true;
    private bool isInPositiveYLimits = true;
    private bool isInNegativeYLimits = true;

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

        float positiveVerticalAngle = _verticalFieldOfView / 2;
        PositivePositionYLimit = CalculateYLimit(positiveVerticalAngle);

        NegativePositionYlimit = transform.position;
    }

    public Vector3 CalculateXLimit(float angle)
    {
        float xRadians = (float)Mathf.Deg2Rad * angle;
        float yRadians = (float)Mathf.Deg2Rad * angle;
        Vector3 result = new Vector3(Mathf.Cos(xRadians), 0, Mathf.Sin(yRadians)).normalized;
        return result;
    }
    public Vector3 CalculateYLimit(float angle)
    {
        float xRadians = (float)Mathf.Deg2Rad * angle;
        float yRadians = (float)Mathf.Deg2Rad * angle;
        Vector3 result = new Vector3(0, Mathf.Sin(yRadians), Mathf.Cos(xRadians)).normalized;
        return result;
    }

    public bool ChecKWithinPlayerFOV(Vector3 position, out Vector3 adjustment)
    {
        bool isInPOV = true;
        adjustment = new Vector3(0, 0, 0);

        Vector3 targetPosition = position - transform.position;

        // calculate limits based on Z position
        Vector3 xPositiveLimit = PositivePositionXlimit * targetPosition.z * 1.42f;
        Vector3 xNegativeLimit = NegativePositionXLimit * targetPosition.z * 1.42f;
        Vector3 yPositiveLimit = PositivePositionYLimit * targetPosition.z * 1.42f;

        // check if the target is beyond Position Limits
        if( position.x > xPositiveLimit.x)
        {
            isInPOV = false;
            isInPositiveXLimits = false;
            adjustment += new Vector3(xPositiveLimit.x - position.x , 0, 0);
        }
        else
        {
            isInPositiveXLimits = true;
        }
        
        if (position.x < xNegativeLimit.x)
        {
            isInPOV = false;
            isInNegativeXLimits = false;
            adjustment += new Vector3(xNegativeLimit.x - position.x, 0, 0);
        }
        else
        {
            isInNegativeXLimits = true;
        }

        if(position.y > yPositiveLimit.y )
        {
            isInPOV = false;
            isInPositiveYLimits = false;
            adjustment += new Vector3(0, yPositiveLimit.y - position.y, 0);
            Debug.Log($"too high up");
        }
        else
        {
            isInPositiveYLimits = true;
        }

        if(position.y < NegativePositionYlimit.y)
        {
            isInPOV = false;
            isInNegativeYLimits = false;
            Debug.Log($"isInNegativeYLimits is {isInNegativeYLimits}");
            adjustment += new Vector3(0, NegativePositionYlimit.y - position.y , 0);
        }
        else
        {
            isInNegativeYLimits = true;
        }

        
 
        return isInPOV;
    }

    private void OnDrawGizmos()
    {
        float positiveHorizontalAngle = 90 - (_horizontalFieldOfView / 2);
        Vector3 rightHorizontalBorder = CalculateXLimit(positiveHorizontalAngle);

        float negativeHorizontalLimit = 90 + (_horizontalFieldOfView / 2);
        Vector3 leftHorizontalBorder = CalculateXLimit(negativeHorizontalLimit);

        Vector3 xPositiveLimitPosition = rightHorizontalBorder * 1.42f * _projectedRange;
        xPositiveLimitPosition.y = transform.position.y; 

        // draw right FOV limit
        Gizmos.color = isInPositiveXLimits ? Color.green : Color.red;
        Gizmos.DrawSphere(xPositiveLimitPosition, 0.25f);
        Gizmos.DrawLine(transform.position, xPositiveLimitPosition);
        Gizmos.DrawCube(xPositiveLimitPosition, new Vector3(0.5f, 0.5f, 0.5f));

        // draw left FOV limit
        Vector3 xNegativeLimitPosition = leftHorizontalBorder * 1.42f * _projectedRange;
        xNegativeLimitPosition.y = transform.position.y;

        Gizmos.color = isInNegativeXLimits ? Color.green : Color.red;
        Gizmos.DrawSphere(xNegativeLimitPosition, 0.25f);
        Gizmos.DrawLine(transform.position, xNegativeLimitPosition);
        Gizmos.DrawCube(xNegativeLimitPosition, new Vector3(0.5f, 0.5f, 0.5f));


        // draw top FOV Limit
        float positiveVerticallLimit = (_verticalFieldOfView / 2);
        Vector3 TopVerticalBorder = CalculateYLimit(positiveVerticallLimit);
        Gizmos.color = Color.green;

        Vector3 topLimitPosition = TopVerticalBorder * 1.42f * _projectedRange;
        if (isInNegativeYLimits && isInPositiveYLimits)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(topLimitPosition, 0.25f);
        Gizmos.DrawLine(transform.position, topLimitPosition);
        Gizmos.DrawCube(topLimitPosition, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
