using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderIKFootSolver : MonoBehaviour
{
    [SerializeField]
    private LayerMask _terrainLayer;
    [SerializeField]
    private List<SpiderIKFootSolver> _otherFootList = new List<SpiderIKFootSolver>();
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

    private float _footSpacing;
    private Vector3 _oldPosition, _currentPosition, _newPosition;
    private Vector3 _oldNormal, _currentNormal, _newNormal;
    private float _lerp;

    //debug 
    [SerializeField]
    private float _initialFootAngle;
    [SerializeField]
    private Vector3 _idealFootPosition;
    [SerializeField]
    private Vector3 _targetOffset;
    [SerializeField]
    private Vector3 _debugPosition;

    void Start()
    {
        SetInitialFeetSpacing();

        _currentPosition = _newPosition = _oldPosition = transform.position;
        _currentNormal = _newNormal = _oldNormal = transform.up;
        _lerp = 1;
    }

    public void SetValues(Transform body,float speed, float stepDistance, float stepLength, float stepHeight, Vector3 footOffset, Vector3 targetOffset)
    {
        _body = body;
        _speed = speed;
        _stepDistance = stepDistance;
        _stepLength = stepLength;
        _stepHeight = stepHeight;
        _footOffset = footOffset;
        _targetOffset = targetOffset;
    }

    private void SetInitialFeetSpacing()
    {
        // Initial feet Distance from Body
        _footSpacing = Vector3.Magnitude(transform.position - _body.position);
        // Angle of the feed from the Body
        _initialFootAngle = Mathf.Atan2(transform.localPosition.z, transform.localPosition.x);
        // Ideal foot position
        //_idealFootPosition = new Vector3(Mathf.Cos(_initialFootAngle), 0, Mathf.Sin(_initialFootAngle) ) * _footSpacing  + _targetOffset;      
        
    }

    void Update()
    {
        CheckFootPosition(_otherFootList);

        if (_lerp < 1)
            MoveLeg();
        else
        {
            _oldPosition = _newPosition;
            _oldNormal = _newNormal;
        }
        transform.position = _currentPosition;
        transform.up = _currentNormal;
    }

    private void CheckFootPosition(List<SpiderIKFootSolver> footList)
    {
        // update angle based on forward facing direction
        // take the angle from facing direction in unit circle. Subtract +/- from the facing direction to determine foot placement
        float facingAngle = Mathf.Atan2(_body.transform.forward.z, _body.transform.forward.x);
        float newAngle = facingAngle + _initialFootAngle - (float)( Mathf.Deg2Rad * 90f);
        // set new ideal foot position value
        _idealFootPosition = new Vector3(Mathf.Cos(newAngle), 0, Mathf.Sin(newAngle)) * _footSpacing + _targetOffset;

        //raycast to check where feed should be releative to the body
        Ray ray = new Ray(_body.position + _idealFootPosition, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit info, 10, _terrainLayer.value))
        {
            if (Vector3.Distance(_newPosition, info.point) > _stepDistance && CheckFeetList(footList) == false && _lerp >= 1)
            //if (Vector3.Distance(_newPosition, info.point) > _stepDistance && _lerp >= 1)
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

    private bool CheckFeetList(List<SpiderIKFootSolver> footList)
    {
        bool result = false;
        for (int i = 0; i < footList.Count; i++)
        {
            result = footList[i].IsMoving();
        }
        return result;
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

    public bool IsMoving() => _lerp < 1;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_newPosition, 0.5f);   
    }
}
