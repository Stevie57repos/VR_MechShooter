using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyStat")]
public class StatsSO : ScriptableObject
{
    public float maxHealth;
    public float TopSpeed;
    public float TargetDistanceSlowDown;
    public float TargetDistanceLimit;
    public float DesiredSeperationDistance;
}
