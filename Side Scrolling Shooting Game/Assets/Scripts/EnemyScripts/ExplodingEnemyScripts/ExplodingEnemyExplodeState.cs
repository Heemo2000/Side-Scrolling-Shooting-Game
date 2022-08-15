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
        _explodingEnemy.Animator.SetBool("IsWalking",false);
        _explodingEnemy.Animator.SetTrigger("Death");
        _explodingEnemy.Agent.enabled = false;
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
        _explodingEnemy.Health.OnDeath?.Invoke();
    }
}
