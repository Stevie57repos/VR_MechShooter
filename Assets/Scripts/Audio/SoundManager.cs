using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType { EnemyTakeDamage, EnemyLaser};

public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private PoolableObject _soundEmitterPrefab;

    [SerializeField]
    private AudioClip _EnemyTakeDamage;

    [SerializeField]
    private Dictionary<SoundType, AudioClip> _SoundsDictionary;

    [SerializeField]
    private SoundEventChannelSO _soundEventChannel;

    private void Awake()
    {
        PoolSystem.CreatePool(_soundEmitterPrefab, 10);
        CreateDictionary();
    }

    private void OnEnable()
    {
        _soundEventChannel.SoundEvent += PositionAndPlaySoundEmitter;
    }

    private void OnDisable()
    {
        _soundEventChannel.SoundEvent -= PositionAndPlaySoundEmitter;
    }

    private void CreateDictionary()
    {
        _SoundsDictionary = new Dictionary<SoundType, AudioClip>();
        _SoundsDictionary.Add(SoundType.EnemyTakeDamage, _EnemyTakeDamage);
    }

    private void PositionAndPlaySoundEmitter(AudioClip soundClip, Transform spawnPoint)
    {
        SoundEmitter soundEmitter = PoolSystem.GetNext(_soundEmitterPrefab) as SoundEmitter;
        soundEmitter.gameObject.SetActive(true);
        soundEmitter.PlayOneShot(soundClip, spawnPoint);
    }

    private AudioClip GetSoundClipFromDictionary(SoundType soundType)
    {
        return _SoundsDictionary[soundType];
    }
}
