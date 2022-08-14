using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemyStopState : IState
{
    private HumanEnemy _humanEnemy;
    public HumanEnemyStopState(HumanEnemy humanEnemy)
    {
        _humanEnemy = humanEnemy;
    }
    public void OnEnter()
    {
        _humanEnemy.Agent.enabled = false;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
