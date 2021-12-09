using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    TwoBoneIKConstraint _LeftArmIK;

    [SerializeField]
    private float _leftArmIKValue;

    private void Update()
    {
        _LeftArmIK.weight = _leftArmIKValue;
    }
}
