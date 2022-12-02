using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Attack 
{
    float _currentProgress;
    float _baseProgress { get; }
    public Attack(float completeTime)
    {
        _baseProgress = completeTime;
        _currentProgress = 0; 
    }

    public float AddProgress(float progress)
    {
        _currentProgress += progress;
        if (_currentProgress >= _baseProgress)
        {
            _currentProgress = 0;
            return 1f;
        }
        else return progress / _baseProgress;
    }
}
