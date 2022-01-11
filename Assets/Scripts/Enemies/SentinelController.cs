using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentinelController : EnemyController
{
    [SerializeField]
    private Animator _animator;

    private void Start()
    {
        _animator.SetTrigger("SetIdle");
    }
}
