using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //_explodingEnemy.Animator.SetBool("IsWalking",false);
        //_explodingEnemy.Animator.SetTrigger("Death");
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
        Health targetHealth = _explodingEnemy.Target.GetComponent<Health>();
        if(targetHealth == null)
        {
            return;
        }
        
        float distanceToTarget = Vector3.Distance(_explodingEnemy.transform.position,_explodingEnemy.Target.position);
        float explodeImpulsePercent = 1f - Mathf.Clamp01(distanceToTarget/_explodingEnemy.ExplodeDistance);
        //Debug.Log("Explode Impulse Percent : " + explodeImpulsePercent);
        _explodingEnemy.ExplodeImpulse.GenerateImpulseWithForce(explodeImpulsePercent);
        targetHealth.OnHealthDamaged?.Invoke(_explodingEnemy.Damage);
        
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_explodingEnemy.ExplodeTime);
        
    }
}
