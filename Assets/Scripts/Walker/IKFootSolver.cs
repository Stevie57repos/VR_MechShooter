using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [SerializeField]
    private LayerMask _terrainLayer;
    [SerializeField]
    private List<IKFootSolver> _otherFootList = new List<IKFootSolver>();
    [SerializeField]
    FootType _footType;
    [SerializeField]
    private Transform _body;

    [Header("CentipedeHandler manages these")]
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
    private float _footSpacing;

    private Vector3 _oldPosition, _currentPosition, _newPosition;
    private Vector3 _oldNormal, _currentNormal, _newNormal;

    [SerializeField]
    private float _lerp;

    void Start()
    {
       
        _footSpacing = transform.localPosition.x;

        _currentPosition = _newPosition = _oldPosition = transform.position;
        _currentNormal = _newNormal = _oldNormal = transform.up;
        _lerp = 1;
    }

    void Update()
    {

        CheckFootPosition(_otherFootList);
        
        if(_lerp < 1)
            MoveLeg();
        else
        {
            _oldPosition = _newPosition;
            _oldNormal = _newNormal;         
        }
        transform.position = _currentPosition;
        transform.up = _currentNormal;
    }
    public void SetValues( float speed, float stepDistance, float steplength, float stepHeight, Vector3 footOffset)
    {
        _speed = speed;
        _stepDistance = stepDistance;
        _stepLength = steplength;
        _stepHeight = stepHeight;
        _footOffset = footOffset;
    }
    private bool CheckFeetList(List<IKFootSolver> footList)
    {
        bool result = false;
        for(int i = 0; i < footList.Count; i++)
        {
             result = footList[i].IsMoving();
        }
        return result;
    }
    //private void CheckFootPosition()
    //{
    //    //raycast to check where feed should be releative to the body
    //    Ray ray = new Ray(_body.position + (_body.right * _footSpacing), Vector3.down);

    //    if (Physics.Raycast(ray, out RaycastHit info, 10, _terrainLayer.value))
    //    {
    //        if (Vector3.Distance(_newPosition, info.point) > _stepDistance && !_otherFoot.IsMoving() && _lerp >= 1)
    //        {
    //            _lerp = 0;
    //            // transforming world space raycast position into local space. Returns if this is in front or behind
    //            int direction = _body.InverseTransformPoint(info.point).z > _body.InverseTransformPoint(_newPosition).z ? 1 : -1;
    //            // set new foot position
    //            _newPosition = info.point + (_body.forward * _stepLength * direction) + _footOffset;
    //            _newNormal = info.normal;
    //        }
    //    }
    //}
    private void CheckFootPosition(List<IKFootSolver> footList)
    {
        //raycast to check where feed should be releative to the body
        Ray ray = new Ray(_body.position + (_body.right * _footSpacing), Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, _terrainLayer.value))
        {
            if (Vector3.Distance(_newPosition, info.point) > _stepDistance && CheckFeetList(footList) == false && _lerp >= 1)
            {
                _lerp = 0;
                // transforming world space raycast position into local space. Returns if this is in front or behind
                int direction = _body.InverseTransformPoint(info.point).z > _body.InverseTransformPoint(_newPosition).z ? 1 : -1;
                // set new foot position
                _newPosition = info.point + (_body.forward * _stepLength * direction) + _footOffset;
                _newNormal = info.normal;
            }
        }
    }
    private void MoveLeg()
    {
        Vector3 tempPosition = Vector3.Lerp(_oldPosition, _newPosition, _lerp);
        // calculates an arc based on stepHeight. Step height is the amplitude.
        // Multiply by PI returns value between 0 - 1 instead of -1 - 1
        tempPosition.y += Mathf.Sin(_lerp * Mathf.PI) * _stepHeight;

        _currentPosition = tempPosition;
        // set normal facing direction
        _currentNormal = Vector3.Lerp(_oldNormal, _newNormal, _lerp);
        _lerp += Time.deltaTime * _speed;
    }
    public bool IsMoving() => _lerp < 1 ;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_newPosition, 0.5f);
    }
}
