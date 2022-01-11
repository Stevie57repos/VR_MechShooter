using System;
using UnityEngine;

[System.Serializable]
public class Timer
{
    private float _duration = 1f;
    private int _ticks = 4;
    private event Action _onTick;
    private event Action _onEnd;

    private float _tickDuration = 0f;
    private float _lastTickTime = 0f;
    private float _buffStartTime = float.MinValue;


    public Timer(float duration, float tickDuration, Action onTick, Action onEnd)
    {
        _duration = duration;
        _onTick = onTick;
        _onEnd = onEnd;

        _tickDuration = tickDuration;
        _lastTickTime = float.MinValue;
        _buffStartTime = Time.time;
    }

    public Timer(float duration, int ticks, Action onTick, Action onEnd)
    {
        _duration = duration;
        _ticks = ticks == 0 ? 1 : ticks;
        _onTick = onTick;
        _onEnd = onEnd;

        _tickDuration = _duration / (_ticks - 1);
        _lastTickTime = float.MinValue;
        _buffStartTime = Time.time;
    }

    public bool Tick()
    {
        if (_buffStartTime + _duration <= Time.time)
        {
            _onTick?.Invoke();
            _onEnd?.Invoke();
            return false;
        }

        if (_lastTickTime + _tickDuration < Time.time)
        {
            _onTick?.Invoke();
            _lastTickTime = Time.time;
        }

        return true;
    }
}
