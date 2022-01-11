using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyStat")]
public class StatsSO : ScriptableObject
{
    public HealthStats HealthStats;
    public MovementStats MovementStats;
    public AttackStats AttackStats;
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
[System.Serializable]
public struct AttackStats
{
    public float Damage;
    public float AttackDistance;
    public float AttackChargeTime;
}