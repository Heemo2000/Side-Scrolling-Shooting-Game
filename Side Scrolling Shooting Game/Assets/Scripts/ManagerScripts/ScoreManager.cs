using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : GenericSingleton<ScoreManager>
{
    private float _currentScore = 0;
    
    public Action<float> OnScoreIncreased;
    public Action<float> OnScoreSet;

    public float CurrentScore { get => _currentScore; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start() 
    {
        OnScoreSet += SetScore;
        OnScoreIncreased += IncreaseScore;   
    }
    private void IncreaseScore(float increment)
    {
        OnScoreSet?.Invoke(_currentScore + increment);
    }

    private void SetScore(float score)
    {
        _currentScore = score;
    }
}
