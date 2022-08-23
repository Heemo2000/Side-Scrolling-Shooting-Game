using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyChaseState : IState
{
    private ExplodingEnemy _explodingEnemy;

    public ExplodingEnemyChaseState(ExplodingEnemy explodingEnemy)
    {
        _explodingEnemy = explodingEnemy;
    }

    public void OnEnter()
    {
        _explodingEnemy.Agent.isStopped = false;
        _explodingEnemy.Agent.speed = _explodingEnemy.ChaseSpeed;
        //_explodingEnemy.Animator.SetBool("IsWalking",true);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        _explodingEnemy.Agent.SetDestination(_explodingEnemy.Target.position);
    }
}
