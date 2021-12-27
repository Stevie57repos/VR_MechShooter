using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosTest : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 2f);
    }
}
