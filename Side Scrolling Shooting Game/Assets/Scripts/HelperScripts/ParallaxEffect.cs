using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField]GameObject relativeObject;

    [SerializeField]private Vector2 parallaxSpeed;
    
    [Range(0f,1f)]
    [SerializeField]private float dampX = 0.5f;

    [Range(0f,1f)]
    [SerializeField]private float dampY = 0.5f;
    
    [Min(0f)]
    [SerializeField]private float checkDistance = 20f;
    private Vector3 _relativeObjPreviousPos;

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (relativeObject.transform.position - _relativeObjPreviousPos).normalized;

        transform.position += new Vector3(direction.x * parallaxSpeed.x * (1f - dampX),direction.y* parallaxSpeed.y * (1f - dampY)) * Time.deltaTime;
        
        float displacement = relativeObject.transform.position.x - transform.position.x;
        if(displacement >= checkDistance)
        {
            transform.position += Vector3.right * checkDistance;
        }
        else if(displacement <= -checkDistance)
        {
            transform.position -= Vector3.right * checkDistance;
        }

        _relativeObjPreviousPos = relativeObject.transform.position;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + Vector3.right * checkDistance);    
    }
}
