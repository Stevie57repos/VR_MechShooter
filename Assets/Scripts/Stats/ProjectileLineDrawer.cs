using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLineDrawer : MonoBehaviour
{
    public Vector3 direction;
    public float force;
    public float gravity = 9.8f;
    public float timeToSimulate;
    public float timeStep;
    private LineRenderer lineRenderer;
    private bool isFired = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFired) return;

        Vector3 position = transform.position; 
        List<Vector3> linePosition = new List<Vector3>();
        Vector3 velocity = direction.normalized * force;
        linePosition.Add(position);
        for (float i = 0; i < timeToSimulate; i += timeStep)
        {
            position += velocity * timeStep;
            linePosition.Add(position);
            velocity.y = velocity.y - (timeStep * gravity);          
        }
        lineRenderer.positionCount = linePosition.Count;
        lineRenderer.SetPositions(linePosition.ToArray());

        if (Input.GetMouseButton(0))
        {
            FireBall();
        }
    }

    private void FireBall()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Physics.gravity = new Vector3(0 , -gravity, 0);
        rb.velocity = direction.normalized * force;
        isFired = true;
    }
}
