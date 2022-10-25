using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class ExplodingEnemyExplodeState : IState
{
    private ExplodingEnemy _explodingEnemy;

    private float _remainingTime = 0f;
    public ExplodingEnemyExplodeState(ExplodingEnemy explodingEnemy)
    {
        _explodingEnemy = explodingEnemy;
    }

    public void OnEnter()
    {   
        _explodingEnemy.Animator.SetBool(StringHolder.IsRollingAnimParameter,false);
        _explodingEnemy.Animator.SetFloat(StringHolder.MoveInputAnimParam,0.0f);
        
        _explodingEnemy.Agent.enabled = false;
        _remainingTime = _explodingEnemy.ExplodeTime;
        _explodingEnemy.ExplodeTimerUI.gameObject.SetActive(true);
    }

    public void OnExit()
    {
                
    }

    public void OnUpdate()
    {
        if(_remainingTime <= 0f)
        {
            var explosionEffect = Object.Instantiate(_explodingEnemy.ExplosionEffect,_explodingEnemy.transform.position,Quaternion.identity);
            explosionEffect.Play();
            DamageTarget();
            _explodingEnemy.EnemyHealth.OnDeath?.Invoke();
            return;
        }
        _explodingEnemy.ExplodeTimerUI.SetFillAmount(_remainingTime,_explodingEnemy.ExplodeTime);
        _remainingTime -= Time.deltaTime;
    }


    private void DamageTarget()
    {
        GameObject.Instantiate(_explodingEnemy.ExplosionEffect,_explodingEnemy.transform.position,Quaternion.identity);
        Health targetHealth = _explodingEnemy.Target.GetComponent<Health>();
        if(targetHealth == null)
        {
            return;
        }
        
        if(Utility.CheckDistance(_explodingEnemy.transform.position,_explodingEnemy.Target.position,_explodingEnemy.ExplodeDistance))
        {
            _explodingEnemy.ExplodeImpulse.GenerateImpulse();
            targetHealth.OnHealthDamaged?.Invoke(_explodingEnemy.Damage);
        }
        
        
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_explodingEnemy.ExplodeTime);
        
    }
}