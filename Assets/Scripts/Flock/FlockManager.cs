using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlockSystemType { simple, spatialRequest}

public class FlockManager : MonoBehaviour
{
    public static FlockManager instance;

    [SerializeField]
    private FlockSystemType _type;
    [Header("Flock Spawn Settings")]
    [SerializeField]
    private bool _isDebugTarget;
    [SerializeField]
    private Transform _debugTargetPos;
    [SerializeField]
    private Transform _VRTargetPos;
    private Transform _target;
    [SerializeField]
    private FlockController _flockPrefab;
    [SerializeField]
    private JobFlockController _jobFlockPrefab;
    [SerializeField]
    private int _spawnAmount;
    [SerializeField]
    protected List<FlockController> _flockList = new List<FlockController>();

    [Header("Flock Settings")]
    [SerializeField]
    private FlockSO _flockSO;

    [Header("RequestManager")]
    [SerializeField]
    private int _maxNumRequestPerFrame = 10;
    [SerializeField]
    private int _maxQueueCount = 50;
    private Queue<FlockRequest> _requestQueue = new Queue<FlockRequest>();
    [SerializeField]
    private SpatialPartitioner _spatialPartitioner;

    [Header("Jobs System")]
    [SerializeField]
    private bool _UsingJobs;
    [SerializeField]
    MovementJobSystem _movementJobSystem;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _target = _isDebugTarget ? _debugTargetPos : _VRTargetPos;
        SpawnFlock();
    }

    private void Update()
    {
        switch (_type)
        {
            case FlockSystemType.simple:            
                foreach (FlockController flock in _flockList)
                {
                    flock.SeekTarget(_target.position);
                    flock.Seperate(_flockList);
                }
                break;
                
            case FlockSystemType.spatialRequest:
                ProcessRequest();
                break;
        }       
    }

    private void SpawnFlock()
    {
        Vector3 offset = Vector3.zero;
        int UnitID;
        for (int i = 0; i < _spawnAmount; i++)
        {
            FlockController flock = Instantiate(_flockPrefab, _debugTargetPos.transform.position + offset, Quaternion.identity);
            UnitID = i;
            flock.Initialize(_flockSO, this, _UsingJobs, _movementJobSystem, UnitID) ;
            _flockList.Add(flock);
            offset.x++;
        }

        if (_UsingJobs == true)
            _movementJobSystem.GetFlockList(_flockList);
    }
    public void MakeRequest(FlockRequest request, FlockController flock)
    {
        if (_requestQueue.Count > _maxQueueCount)
        {
            flock.QueueFull();
            return;
        }
        _requestQueue.Enqueue(request);
    }
    private void ProcessRequest()
    {
        int processedRequestCount = 0;
        while(processedRequestCount < _maxNumRequestPerFrame && _requestQueue.Count != 0)
        {
            _spatialPartitioner.ProcessFlockRequest(_requestQueue.Dequeue(), _target.position);
            processedRequestCount++;
        }
    }
}
