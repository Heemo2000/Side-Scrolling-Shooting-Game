using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanEnemyChaseState : IState
{
    private HumanEnemy _humanEnemy;

    private bool _jumpTrigger = false;
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
        if(_humanEnemy.Agent.isOnOffMeshLink && _jumpTrigger == false)
        {
            _jumpTrigger = true;
            _humanEnemy.HumanAnimator.SetTrigger(StringHolder.JumpTriggerAnimParameter);
            _humanEnemy.HumanAnimator.SetBool(StringHolder.IsFallingAnimParameter,true);
        }

        else if(_humanEnemy.Agent.isOnOffMeshLink == false && _jumpTrigger == true)
        {
            _jumpTrigger = false;
            _humanEnemy.HumanAnimator.SetBool(StringHolder.IsFallingAnimParameter,false);
        }
    }
}
