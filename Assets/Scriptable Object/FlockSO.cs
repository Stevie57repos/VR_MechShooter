using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Flock")]
public class FlockSO : ScriptableObject
{
    public float TopSpeed; 
    public float TargetDistanceSlowDown; 
    public float DistanceSlowDown; 
    public float DesiredSeperationDistance;
}
