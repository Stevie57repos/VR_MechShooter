using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyStat")]
public class StatsSO : ScriptableObject
{
    public float maxHealth;
    public float DistanceSlowDown;
    public float TopSpeed;
    public float TargetDistanceSlowDown;
    public float DesiredSeperationDistance;
}
