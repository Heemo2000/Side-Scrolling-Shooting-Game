using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField]GameObject relativeObject;
    
    [Min(0f)]
    [SerializeField]private float checkDistance = 20f;
    private Vector3 _relativeObjPreviousPos;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.IsGameEnded || PauseController.Instance.IsGamePaused)
        {
            return;
        }

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
