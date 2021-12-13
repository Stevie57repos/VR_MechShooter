using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
using math = Unity.Mathematics;
using random = Unity.Mathematics.Random;
using UnityEngine.Profiling;

public struct JobData
{
    public Vector3 CurrentPosition;
    public Vector3 TargetPosition;
    public Vector3 ResultDirection;
    public int FlockUnitID;

    public float DesiredSeperationDistance;
    public float TopSpeed;
    public float DeltaTime;

    public void Update()
    {
        Vector3 resultDirection = Vector3.zero;
        float distance = Vector3.Distance(CurrentPosition, TargetPosition);
        if (distance > 0 && distance < DesiredSeperationDistance)
        {
            Vector3 oppositeDirection = (CurrentPosition - TargetPosition).normalized;
            resultDirection = oppositeDirection * TopSpeed * DeltaTime * 2;
        }
        ResultDirection = resultDirection;
    }
}

[BurstCompile]
public struct FlockSeperateMovementJobParallel : IJobParallelFor
{
    public NativeArray<JobData> JobList;
    public void Execute(int i)
    {
        JobData flockJob = JobList[i];
        flockJob.Update();
        JobList[i] = flockJob;
    }
}

public class MovementJobSystem : MonoBehaviour
{
    List<FlockController> _flockList;
    JobHandle _flockSeperateMovementJobParallelJobHandler;
    FlockSeperateMovementJobParallel _flockSeperateMovementJobParallel;
    private Queue<JobData> _jobDataQueue = new Queue<JobData>();

    private void Update()
    {
        if (_jobDataQueue.Count == 0) return;
  
        NativeArray<JobData> _jobList = new NativeArray<JobData>(_jobDataQueue.Count, Allocator.TempJob);
        for (int i = 0; i < _jobDataQueue.Count; i++)
            _jobList[i] = _jobDataQueue.Dequeue();
        _flockSeperateMovementJobParallel = new FlockSeperateMovementJobParallel()
        {
            JobList = _jobList
        };

        //Profiler.BeginSample("DoingJobSystem");
        _flockSeperateMovementJobParallelJobHandler = _flockSeperateMovementJobParallel.Schedule(_jobList.Length, 64);
        _flockSeperateMovementJobParallelJobHandler.Complete();
        for (int i = 0; i < _flockSeperateMovementJobParallel.JobList.Length; i++)
        {
            int UnitID = _flockSeperateMovementJobParallel.JobList[i].FlockUnitID;
            Vector3 resultDirection = _flockSeperateMovementJobParallel.JobList[i].ResultDirection;
            _flockList[UnitID].JobSeperateResult(resultDirection);
        }
        _jobList.Dispose();
        //Profiler.EndSample();
    }

    public void SeperateMovementJob(List<FlockController> flockList, FlockController flock, FlockSO flockSO)
    {
        if (flockList.Count == 0) return;

        for(int i = 0; i < flockList.Count; i++)
        {
            if (flockList[i] == flock) return;

            JobData flockMovementJob = new JobData()
            {
                CurrentPosition = flock.transform.position,
                TargetPosition = flockList[i].transform.position,
                FlockUnitID = flock._flockID,
                DesiredSeperationDistance = flockSO.DesiredSeperationDistance,
                TopSpeed = flockSO.TopSpeed,
                DeltaTime = Time.deltaTime
            };
            _jobDataQueue.Enqueue(flockMovementJob);
        }
    }

    public void GetFlockList(List<FlockController> flocklist)
    {
        _flockList = flocklist;
    }
}
