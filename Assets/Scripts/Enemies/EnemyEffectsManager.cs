using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffectsManager : MonoBehaviour
{
    [SerializeField]
    EnemyDeathEventSO _deathEventChannel;
    [SerializeField]
    PoolableObject _explosionEffectsPrefab;
    [SerializeField]
    PoolableObject _electricityEffectsPrefab;
    [SerializeField]
    private AudioClip _explosionSoundClip;
    [SerializeField]
    private SoundEventChannelSO _soundEventChannel;
    [SerializeField]
    private ElectricalEffectsChannelSO _electricalFXEventChannel;
    private void Awake()
    {
        PoolSystem.CreatePool(_explosionEffectsPrefab, 10);
        PoolSystem.CreatePool(_electricityEffectsPrefab, 10);
    }

    private void OnEnable()
    {
        _deathEventChannel.EnemyDeathEvent += Explode;
        _electricalFXEventChannel.ElectricalEffectsEvent += SpawnElectricalFX;
    }

    private void OnDisable()
    {
        _deathEventChannel.EnemyDeathEvent -= Explode;
    }

    private void Explode(EnemyController enemy)
    {
        PoolableObject explosion = PoolSystem.GetNext(_explosionEffectsPrefab);
        explosion.transform.position = enemy.transform.position;
        explosion.gameObject.SetActive(true);
        _soundEventChannel.RaiseEvent(_explosionSoundClip, enemy.transform);
    }

    private void SpawnElectricalFX(Transform spawnPosition)
    {
        PoolableObject electricalFX = PoolSystem.GetNext(_electricityEffectsPrefab);
        electricalFX.transform.position = spawnPosition.transform.position;
        electricalFX.gameObject.SetActive(true);
    }
}
