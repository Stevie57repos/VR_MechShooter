using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyStat")]
public class StatsSO : ScriptableObject
{
    //public float maxHealth;
    public HealthStats HealthStats;
    //public float TopSpeed;
    //public float TargetDistanceSlowDown;
    //public float TargetDistanceLimit;
    //public float DesiredSeperationDistance;
    public MovementStats MovementStats;
}

[System.Serializable]
public struct HealthStats
{
    public float maxHealth;
}

[System.Serializable]
public struct MovementStats
{
    public float TopSpeed;
    public float TargetDistanceSlowDown;
    public float TargetDistanceLimit;
    public float DesiredSeperationDistance;
}