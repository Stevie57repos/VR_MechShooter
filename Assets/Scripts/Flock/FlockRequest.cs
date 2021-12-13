using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct FlockRequest
{
    //public Action<FlockCallbackResult> ResultCallBack;
    //public Vector3 FlockPosition;
    //public FlockController Flock;    
    public int UnitID;
    public Vector3 TargetPosition;

    public void AddUnitID(int unitID)
    {
        UnitID = unitID;
    }

    //public FlockRequest(Action<FlockCallbackResult> callback, Vector3 flockPosition, FlockController flock, int unitID)
    //{
    //    // flock result handle method when data is returned
    //    ResultCallBack = callback;
    //    FlockPosition = flockPosition;
    //    Flock = flock;
    //    UnitID = unitID;
    //}
}

public struct FlockCallbackResult
{
    public Vector3 TargetPosition;
    public List<FlockController> FlockList;
    public bool InGridRange;
}

public struct JobFlockCallbackResult
{
    public Vector3 DesiredDirection;    
}