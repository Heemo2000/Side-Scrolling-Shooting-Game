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
        
        _explodingEnemy.Animator.SetBool(StringHolder.IsRollingAnimParameter,false);
        _explodingEnemy.Animator.SetFloat(StringHolder.MoveInputAnimParam,1.0f);
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        CheckFront();
    }

    private void CheckFront()
    {
        Ray ray = new Ray(_explodingEnemy.FrontCheck.position,_explodingEnemy.FrontCheck.forward);
        if(Physics.SphereCast(ray,0.15f,out RaycastHit hit,_explodingEnemy.MaxFrontCheckDistance,
                              ~((1 << _explodingEnemy.Target.gameObject.layer) | _explodingEnemy.RayCastIgnore.value)))
        {
            //Debug.Log("Detecting : " + hit.transform.name);
            _explodingEnemy.Agent.enabled = false;
            _explodingEnemy.Animator.SetFloat(StringHolder.MoveInputAnimParam,0.0f);
        }
        else
        {
            _explodingEnemy.Agent.enabled = true;
            _explodingEnemy.Agent.SetDestination(_explodingEnemy.Target.position);    
            _explodingEnemy.Animator.SetFloat(StringHolder.MoveInputAnimParam,1.0f);
        }
    }
}