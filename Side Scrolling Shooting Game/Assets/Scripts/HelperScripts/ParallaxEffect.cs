using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField]GameObject relativeObject;

    [SerializeField]float parallaxEffect;

    [Min(0f)]
    [SerializeField]private float halfLength = 20f;
    private float _currentPosition;

    private Vector3 _relativeObjPreviousPos;
    void Start()
    {
        _currentPosition=transform.position.x;
        _relativeObjPreviousPos = relativeObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (relativeObject.transform.position - _relativeObjPreviousPos).normalized;
        _currentPosition += direction.x * parallaxEffect;
        float displacement = relativeObject.transform.position.x - _currentPosition;

        if(displacement >= halfLength)
        {
            _currentPosition -= halfLength;
        }
        else if(displacement < -halfLength)
        {
            _currentPosition += halfLength;
        } 


        transform.position = new Vector3(_currentPosition,
                                         transform.position.y,
                                         transform.position.z);

        _relativeObjPreviousPos = relativeObject.transform.position;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position + Vector3.right * halfLength);    
    }
}
