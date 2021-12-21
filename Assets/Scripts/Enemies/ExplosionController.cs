using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    EnemyDeathEventSO _deathEventChannel;
    [SerializeField]
    GameObject _explosion;
    [SerializeField]
    PoolableObject _explosionFX;
    private void Awake()
    {
        PoolSystem.CreatePool(_explosionFX, 10);
    }

    private void OnEnable()
    {
        _deathEventChannel.EnemyDeathEvent += Explode;
    }

    private void OnDisable()
    {
        _deathEventChannel.EnemyDeathEvent -= Explode;
    }

    private void Explode(EnemyController enemy)
    {
        PoolableObject explosion = PoolSystem.GetNext(_explosionFX);
        explosion.transform.position = enemy.transform.position;
        explosion.gameObject.SetActive(true);
    }


}
