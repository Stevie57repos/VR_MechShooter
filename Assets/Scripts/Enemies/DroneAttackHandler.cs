using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAttackHandler : MonoBehaviour, IEnemyAttackHandler
{
    private AttackStats _stats;
    private Transform _target;
    private IEnemyMovementHandler _movementHandler;
    private List<EnemyController> _enemiesInWave;
    private Coroutine _currentState = null;
    private float _attackTimerStart = float.MinValue;
    private float _attackTimerEnd;
    [SerializeField]
    private ParticleSystem _attackParticles;
    [SerializeField]
    private AudioClip _laserAttackSoundClip;
    [SerializeField]
    private SoundEventChannelSO _soundEventChannel;

    private void OnDisable()
    {
        _attackParticles.Stop();
    }

    public void HandleAttack(Transform target, IEnemyMovementHandler movementHandler, List<EnemyController> enemiesInWave)
    {
        _target = target;
        _movementHandler = movementHandler;
        _enemiesInWave = enemiesInWave;
        SetState(State_MoveTowardsTarget());
    }

    public void Setup(AttackStats stats)
    {
        _stats = stats;
    }

    private IEnumerator State_MoveTowardsTarget()
    {
        Debug.Log($"in moving state");
        _attackParticles.Stop();
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        // move towards target
        while (Vector3.Distance(transform.position, _target.position) > _stats.AttackDistance)
        {
            _movementHandler.AttackMovement(_target, _enemiesInWave);
            yield return null;    
        }
        _attackTimerStart = Time.time;
        _attackTimerEnd = Time.time + _stats.AttackChargeTime;    
        _attackParticles.Play();
        audioSource.PlayOneShot(_laserAttackSoundClip);
        SetState(State_Attack());        
    }

    private IEnumerator State_Attack()
    {
        Debug.Log($"in Attack State");
        float currentDistance = Vector3.Distance(transform.position, _target.position);
        if ( currentDistance > _stats.AttackDistance) SetState(State_MoveTowardsTarget());

        while(currentDistance < _stats.AttackDistance)
        {
            transform.LookAt(_target.position);
            _movementHandler.StopMovement();
            _attackTimerStart = Time.time;
            if (_attackTimerStart <= _attackTimerEnd)
            {
                yield return null;
            }
            else
            {
                Debug.Log($"timer started at {_attackTimerStart} and ended at {_attackTimerEnd}");
                Debug.Log($"Player takes damage !");
                PlayerController player = _target.GetComponent<PlayerController>();
                if (player.CheckPlayerHealthStatus())
                {
                    player.PlayerDamage(_stats.Damage);
                    if (player.CheckPlayerHealthStatus())
                    {
                        yield return new WaitForSeconds(3f);
                        _attackTimerStart = Time.time;
                        _attackTimerEnd = Time.time + _stats.AttackChargeTime;
                        _attackParticles.Stop();
                        _attackParticles.Play();
                        _soundEventChannel.RaiseEvent(_laserAttackSoundClip, this.transform);
                    }
                    else
                    {
                        StopAllCoroutines();
                    }
                }
                else
                {
                    StopAllCoroutines();
                }
            }
            yield return null;
        }
    }



    private void SetState(IEnumerator newState) 
    {
        if (_currentState != null)
        {
            StopCoroutine(_currentState);
        }

        _currentState = StartCoroutine(newState);
    }

    private IEnumerator State_EMPStun()
    {
        yield return new WaitForSeconds(3f);
        SetState(State_MoveTowardsTarget());
    }

    public void EMPStun()
    {
        SetState(State_EMPStun());       
    }
}
