using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Cooldown
{
    public Action OnCooldownEnd;
    public string name;
    public float duration;
    private float timeLeft;
    public bool Finished { get { return timeLeft <= 0; } }

    public Cooldown(string _name, float _duration)
    {
        name = _name;
        duration = _duration;
        timeLeft = duration;
    }

    public void Update(float _deltaTime)
    {
        timeLeft -= _deltaTime;

        if (timeLeft <= 0)
            OnCooldownEnd?.Invoke();
    }

    public void SetName(string _name)
    {
        name = _name;
    }

    public void SetDuration(float _duration)
    {
        duration = _duration;
        timeLeft = duration;
    }

    public Cooldown GetCopy()
    {
        Cooldown copy = new Cooldown(name, duration);
        return copy;
    }

    public Cooldown GetCopy(string _name)
    {
        Cooldown copy = new Cooldown(_name, duration);
        return copy;
    }
}
