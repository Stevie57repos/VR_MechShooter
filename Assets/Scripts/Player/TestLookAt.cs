using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLookAt : MonoBehaviour
{
    [SerializeField]
    Transform DebugTarget;

    [SerializeField]
    Transform DebugObject;

    [SerializeField]
    float xOffset;

    [SerializeField]
    float yOffset;
    
    [SerializeField]
    float zOffset;

    [SerializeField]
    Vector3 targetDirection;
    Vector3 targetPosition;

    private void Update()
    {
        targetPosition = DebugTarget.position;
        targetDirection = targetPosition - new Vector3(DebugObject.position.x + xOffset, DebugObject.position.y +yOffset, DebugObject.position.z);         
        //targetDirection = DebugObject.position;
        DebugObject.LookAt(targetDirection);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(DebugObject.transform.position, targetDirection);
        Color color = Color.green;
        color.a = 0.1f;
        Gizmos.color = color;
        Gizmos.DrawSphere(targetDirection, 1f);
    }
}
