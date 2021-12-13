using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockMovementRequestController : MonoBehaviour
{
    private List<FlockController> _flockList;
    [SerializeField]
    private int _currentIndex;
    private Queue<FlockRequest> _requestQueue = new Queue<FlockRequest>();
    public int QueueCount { get { return _requestQueue.Count; } }
    private bool _isUpdatingFlockMovement = false;
    private int _maxNumRequestPerFrame;

    public void Initialize(List<FlockController> flockList, int requestPerFrame)
    {
        _currentIndex = 0;
        _flockList = flockList;
        _isUpdatingFlockMovement = true;
        _maxNumRequestPerFrame = requestPerFrame;
        //StartUpdateCoroutine();
    }

    private void StartUpdateCoroutine()
    {
        StartCoroutine(FlockMovementRequest());
    }

    private IEnumerator FlockMovementRequest()
    {
        while (_isUpdatingFlockMovement)
        {
            int counter = 0;
            while (counter < 30)
            {
                FlockController flock = _flockList[_currentIndex];
                FlockRequest request = flock.MakeRequest();
                _requestQueue.Enqueue(request);
                IncrementIndex();
                yield return null;
            }
        }
    }

    private void IncrementIndex()
    {
        _currentIndex++;
        if(_currentIndex == _flockList.Count)
        {
            _currentIndex = 0;
        }
    }

    public FlockRequest RetrieveRequest()
    {
        return _requestQueue.Dequeue();
    }

    private void Update()
    {
        if(_isUpdatingFlockMovement)
        {
            int counter = 0;
            while(counter < _maxNumRequestPerFrame)
            {
                FlockController flock = _flockList[_currentIndex];
                FlockRequest request = flock.MakeRequest();
                _requestQueue.Enqueue(request);
                IncrementIndex();
                counter++;
            }       
        }
    }
}
