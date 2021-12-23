using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelController : EnemyController
{
    [Header("Tentacle Target")]
    [SerializeField]
    private List<Transform> tentacleList = new List<Transform>();

    [SerializeField]
    private Animator _animator;

    protected override void Start()
    {
        SetState(State_Idle());
    }

    protected override IEnumerator State_Idle()
    {
        while(_target == null)
        {
            _animator.SetTrigger("SetIdle");
            yield return null;
        }
    }
}
