using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyExplodeState : IState
{
    private ExplodingEnemy _explodingEnemy;
    public ExplodingEnemyExplodeState(ExplodingEnemy explodingEnemy)
    {
        _explodingEnemy = explodingEnemy;
    }

    public void OnEnter()
    {
        Debug.Log("Now inside exploding state.");
        _explodingEnemy.Agent.isStopped = true;
        _explodingEnemy.Agent.speed = 0f;

        _explodingEnemy.Animator.SetBool("IsWalking",false);
        
        _explodingEnemy.Agent.stoppingDistance = _explodingEnemy.DestCheckDistance;
        _explodingEnemy.Agent.acceleration = 0f;
        _explodingEnemy.StartCoroutine(ExplodeCoroutine());
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_explodingEnemy.ExplodeTime);
        var explosionEffect = Object.Instantiate(_explodingEnemy.ExplosionEffect,_explodingEnemy.transform.position,Quaternion.identity);
        explosionEffect.Play();
        Object.Destroy(_explodingEnemy.gameObject);
    }
}
