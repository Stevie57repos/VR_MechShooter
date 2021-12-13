using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FlockSystemType { simple, spatialRequest}

[RequireComponent(typeof(MovementJobSystem))]
[RequireComponent(typeof(SpatialPartitioner))]
public class FlockManager : MonoBehaviour
{
    [Header("Debugging Settings")]
    [SerializeField]
    private bool _isDebugTarget;
    [SerializeField]
    private Transform _debugTargetPos;

    // For testing different optimization set ups
    [SerializeField]
    private FlockSystemType _type;

    [Header("Flock Spawn Settings")]
    [SerializeField]
    private Transform _VRTargetPos;
    private Transform _target;
    [SerializeField]
    private FlockController _flockPrefab;
    [SerializeField]
    private int _spawnAmount;
    [SerializeField]
    protected List<FlockController> _flockList = new List<FlockController>();

    [Header("Flock Settings")]
    [SerializeField]
    private FlockSO _flockSO;

    [Header("Flock Movement Controller")]
    [SerializeField]
    FlockMovementRequestController _flockMovementRequestController;
    [SerializeField]
    private int _maxNumRequestPerFrame = 10;
    //[SerializeField]
    //private int _maxQueueCount = 50;

    [SerializeField]
    private SpatialPartitioner _spatialPartitioner;

    [Header("Jobs System")]
    [SerializeField]
    private bool _UsingJobs;
    [SerializeField]
    MovementJobSystem _movementJobSystem;

    private void Start()
    {
        SetTarget();
        SpawnFlock();
        SetupSpatialRequests();
        SetupJobSystem();

        // For debugging
        //DebuggingDisplaySettings();
    }

    private void Update()
    {
        switch (_type)
        {
            case FlockSystemType.simple:            
                foreach (FlockController flock in _flockList)
                {
                    flock.SimpleFlockSystemUpdate(_target.position,_flockList);
                }
                break;
                
            case FlockSystemType.spatialRequest:
                ProcessRequest();
                break;
        }       
    }
    private void SetTarget()
    {
        _target = _isDebugTarget ? _debugTargetPos : _VRTargetPos;
    }
    private void SpawnFlock()
    {
        Vector3 offset = Vector3.zero;
        int AssignedUnitID;
        for (int i = 0; i < _spawnAmount; i++)
        {
            FlockController flock = Instantiate(_flockPrefab, _debugTargetPos.transform.position + offset, Quaternion.identity);
            AssignedUnitID = i;
            flock.Initialize(_flockSO, _UsingJobs, _movementJobSystem, AssignedUnitID);
            _flockList.Add(flock);
            offset.x++;
        }
    }

    private void SetupJobSystem()
    {
        if (_UsingJobs == true)
            _movementJobSystem.GetFlockList(_flockList);
    }

    private void SetupSpatialRequests()
    {
        if (_type == FlockSystemType.spatialRequest)
        {
            _flockMovementRequestController.Initialize(_flockList, _maxNumRequestPerFrame);
            _spatialPartitioner.Initialize(_flockList);
        }
    }

    private void ProcessRequest()
    {
        int processedRequestCount = 0;

        while(processedRequestCount < _maxNumRequestPerFrame && _flockMovementRequestController.QueueCount != 0)
        {
            FlockRequest request = _flockMovementRequestController.RetrieveRequest();
            request.TargetPosition = _target.position;
            _spatialPartitioner.ProcessFlockRequest(request);
            processedRequestCount++;
        }
    }

    private void DebuggingDisplaySettings()
    {
        string display =
            $"spawn amount is {_spawnAmount}<br>" +
            $"jobsystem = {_UsingJobs}<br>" +
            $"request/frame {_maxNumRequestPerFrame}";
        DebugEditorScreen.Instance.DisplayValue(display);
    }
}
