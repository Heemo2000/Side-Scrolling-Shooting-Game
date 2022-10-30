using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if(ScoreManager.Instance == null)
        {
            return;
        }
        DontDestroyOnLoad(this);
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

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(scene.name.StartsWith("Level"))
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }

    private void OnDestroy() 
    {
        OnScoreSet -= SetScore;
        OnScoreIncreased -= IncreaseScore;    
    }
}
