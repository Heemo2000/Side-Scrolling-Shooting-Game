using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemyChaseState : IState
{
    private HumanEnemy _humanEnemy;
    public HumanEnemyChaseState(HumanEnemy humanEnemy)
    {
        _humanEnemy = humanEnemy;
    }
    public void OnEnter()
    {
        _humanEnemy.Agent.enabled = true;
        _humanEnemy.Agent.speed = _humanEnemy.MoveSpeed;
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        _humanEnemy.Agent.SetDestination(_humanEnemy.Target.position);   
    }
}
