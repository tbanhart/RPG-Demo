using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction
{
    public ActionType Type {get; set;}

    public GameObject Target {get; set;}

    public float Distance {get; set;}

    public float Effect {get; set;}

    float _currentProgress { get; set; }

    public float Progress { get => _currentProgress / Duration; }

    public float Duration { get; set; }

    public bool IsComplete {get; set;}

    public Interaction(ActionType type, GameObject target, float distance = 3f, float effect = 0f, float duration = 1f){
        Type = type;
        Target = target;
        Distance = distance;
        Effect = effect;
        _currentProgress = 0f;
        Duration = duration;

        IsComplete = false;
    }

    public Interaction(ActionType type, GameObject target, float distance){
        Type = type;
        Target = target;
        Distance = distance;
    }

    public float AddProgress(float progress)
    {
        _currentProgress = Mathf.Clamp(_currentProgress + progress, 0f, Duration);
        if (_currentProgress == Duration)
        {
            return 1f;
        }
        else return _currentProgress / Duration;
    }

    public float ResetProgress()
    {
        _currentProgress = 0f;
        return _currentProgress;
    }
}
