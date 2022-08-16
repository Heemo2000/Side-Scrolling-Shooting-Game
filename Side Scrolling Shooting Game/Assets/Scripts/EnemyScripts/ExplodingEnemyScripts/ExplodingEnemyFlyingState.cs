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
        Debug.Log("Now in flying state.");
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
        Vector3 destination = _explodingEnemy.Target.position;
        Vector3 displacement = destination - _explodingEnemy.transform.position;

        float launchAngle = 15f;//Mathf.Atan(displacement.y/displacement.x);
        float horizontalVelocity = _explodingEnemy.FlySpeed * Mathf.Cos(launchAngle * Mathf.Deg2Rad);
        float verticalVelocity = _explodingEnemy.FlySpeed * Mathf.Sin(launchAngle * Mathf.Deg2Rad);        
        float currentTime = 0f;
        _explodingEnemy.Agent.enabled = false;
        
        Vector3 currentPosition = _explodingEnemy.transform.position;

        while(currentTime < _explodingEnemy.FlyTime)
        {
            currentPosition.x += horizontalVelocity * currentTime;
            currentPosition.y -= Physics.gravity.y * verticalVelocity * currentTime;    
            currentTime += Time.deltaTime;
            _explodingEnemy.transform.position = currentPosition;

            yield return null;
        }
        _explodingEnemy.transform.position = destination;
        _explodingEnemy.Agent.enabled = true;
        yield return null;
    }
}
