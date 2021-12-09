using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLegHandler : MonoBehaviour
{
    [SerializeField]
    private List<SpiderIKFootSolver> _spiderLegsList = new List<SpiderIKFootSolver>();

    [SerializeField]
    private Transform _body;
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
    [SerializeField]
    private Vector3 _targetOffset;

    private void Awake()
    {
        foreach(SpiderIKFootSolver spiderLeg in _spiderLegsList)
        {
            spiderLeg.SetValues(_body, _speed, _stepDistance, _stepLength, _stepLength, _footOffset, _targetOffset);
        }
    }
}
