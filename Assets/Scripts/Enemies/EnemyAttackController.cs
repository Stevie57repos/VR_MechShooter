using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour, IEnemyAttackHandler
{
    private AttackStats _stats;
    private float _attackTimerStart = float.MinValue;
    private float _attackTimerEnd;
    private Transform _target;
    private bool _attackStarted;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private ParticleSystem _attackParticles;
    [SerializeField]
    private AudioClip _laserAttackSoundClip;
    [SerializeField]
    private SoundEventChannelSO _soundEventChannel;

    private void OnEnable()
    {
        _attackStarted = false;
    }

    public void Setup(AttackStats stats)
    {
        _stats = stats;
    }

    public void AttackTarget(Transform target)
    {
        if (_attackStarted) return;

        _target = target;
        if (_animator != null ) _animator.SetTrigger("SetAttack");
        //_attackTimerStart = Time.time;
        _attackTimerEnd = Time.time + _stats.AttackChargeTime;
        _attackParticles.Play();
        _soundEventChannel.RaiseEvent(_laserAttackSoundClip, this.transform);
        _attackStarted = true;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (_attackStarted)
        {
            if (Time.time <= _attackTimerEnd) yield return null;
            else
            {
                PlayerController player = _target.GetComponent<PlayerController>();
                if (player.CheckPlayerHealthStatus())
                {
                    _target.GetComponent<PlayerController>().PlayerDamage(_stats.Damage);

                    // check if this attack killed the player
                    if (player.CheckPlayerHealthStatus())
                    {
                        // restart attack if player alive 
                        yield return new WaitForSeconds(3f);
                        //_attackTimerStart = Time.time;
                        _attackTimerEnd = Time.time + _stats.AttackChargeTime;
                        _attackParticles.Stop();
                        _attackParticles.Play();
                        _soundEventChannel.RaiseEvent(_laserAttackSoundClip, this.transform);
                    }
                    else
                    {
                        // stop attacking becuase player is dead
                        StopAllCoroutines();
                    }
                }
            }
        }
    }

    private void RestartAttack()
    {
        AttackTarget(_target);
    }

    public void EMPStun()
    {
        StopAttack();
    }

    public void HandleAttack(Transform target, IEnemyMovementHandler movementHandler, List<EnemyController> enemiesInWave)
    {
        throw new System.NotImplementedException();
    }

    public void StopAttack()
    {
        StopAllCoroutines();
        _attackStarted = false;
        _attackParticles.Stop();
    }
}
