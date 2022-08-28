using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingEnemyFlyingState : IState
{
    private ExplodingEnemy _explodingEnemy;
    public ExplodingEnemyFlyingState(ExplodingEnemy explodingEnemy)
    {
        _explodingEnemy = explodingEnemy;
    }
    public void OnEnter()
    {
        //Debug.Log("Now in flying state.");

        _explodingEnemy.Animator.SetBool(StringHolder.IsRollingAnimParameter,true);
        _explodingEnemy.StartCoroutine(Fly());

    }

    public void OnExit()
    {
        _explodingEnemy.Agent.enabled = true;
    }

    public void OnUpdate()
    {
        
    }


    private IEnumerator Fly()
    {
        _explodingEnemy.IsFlying = true;
        Vector3 source = _explodingEnemy.transform.position;
        Vector3 destination = _explodingEnemy.GetFlyingDestination();
        
        float currentTime = 0f;
        _explodingEnemy.Agent.enabled = false;
        
        Vector3 currentPosition = source;

        while(currentTime < 1.0f)
        {
            float offsetY = _explodingEnemy.FlyingCurve.Evaluate(currentTime);
            currentPosition = Vector3.Lerp(source,destination,currentTime) + Vector3.up * offsetY;
            _explodingEnemy.transform.position = currentPosition;
            currentTime += (_explodingEnemy.FlySpeed * Time.deltaTime)/_explodingEnemy.FlyTime;
            yield return null;
        }
        _explodingEnemy.transform.position = destination;
        _explodingEnemy.Agent.enabled = true;
        yield return null;
        _explodingEnemy.IsFlying = false;
    }
}