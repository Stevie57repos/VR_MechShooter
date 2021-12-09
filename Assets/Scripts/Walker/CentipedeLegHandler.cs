using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeLegHandler : MonoBehaviour
{
    [SerializeField]
    private List<IKFootSolver> _legList = new List<IKFootSolver>();
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _stepDistance;
    [SerializeField]
    private float _stepLength;
    [SerializeField]
    private float _stepHeight;
    [SerializeField]
    private Vector3 _footOffset;

    private void Awake()
    {
        SetValues();
    }

    private void Update()
    {
        SetValues();
    }

    private void SetValues()
    {
        foreach(IKFootSolver leg in _legList)
        {
            leg.SetValues(_speed, _stepDistance, _stepLength, _stepHeight, _footOffset);
        }
    }

}
