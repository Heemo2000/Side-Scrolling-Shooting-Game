using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyPatrolState : IState
{
    private ExplodingEnemy _explodingEnemy;

    private int _destinationIndex;

    private Coroutine _patrolCoroutine;

    public ExplodingEnemyPatrolState(ExplodingEnemy explodingEnemy)
    {
        _explodingEnemy = explodingEnemy;

    }
    public void OnEnter()
    {
        _explodingEnemy.Agent.isStopped = false;
        _explodingEnemy.Agent.speed = _explodingEnemy.PatrolSpeed;
        _explodingEnemy.Agent.stoppingDistance = _explodingEnemy.DestCheckDistance;
        _destinationIndex = 0;
        _patrolCoroutine = _explodingEnemy.StartCoroutine(PatrolCoroutine());
    }

    public void OnExit()
    {
        _explodingEnemy.StopCoroutine(_patrolCoroutine);
    }

    public void OnUpdate()
    {

    }

    private IEnumerator PatrolCoroutine()
    {
        while(_explodingEnemy.enabled)
        {
            
            if(_explodingEnemy.CheckDistance(_explodingEnemy.transform.position,
               _explodingEnemy.PatrolDestinations[_destinationIndex].position,
               _explodingEnemy.DestCheckDistance))
            {
                _explodingEnemy.Agent.isStopped = true;
                _explodingEnemy.Animator.SetBool("IsWalking",false);

                yield return new WaitForSeconds(_explodingEnemy.PatrolInterval);
                
                _destinationIndex = (_destinationIndex + 1) % _explodingEnemy.PatrolDestinations.Length;
                _explodingEnemy.Agent.isStopped = false;
                _explodingEnemy.Animator.SetBool("IsWalking",true);
            }

            _explodingEnemy.Agent.SetDestination(_explodingEnemy.PatrolDestinations[_destinationIndex].position);
            
            yield return null;
        }
        yield break;
    }
}
