using System;
using UnityEngine;

[Serializable]
public class TAsset<T> : ScriptableObject
{
    public event Action<T> OnValueChanged;

    [SerializeField]
    private T _value;
    public T Value
    {
        get => _value;

        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value);
        }
    }
}

/// Software design Concepts
// Code Coupling
// Temporal Coupling
// Decouple Code
// Coupled Code
