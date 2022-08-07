using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class PlayerAim : MonoBehaviour
{
    [SerializeField]private RigBuilder rigBuilder;
    [SerializeField]private MultiAimConstraint headIK;
    [SerializeField]private MultiAimConstraint spineIK;

    [SerializeField]private MultiAimConstraint rightHandIK;
    private PlayerMovement _movement;

    GameObject _mousePointerPrimitive;
    private void Awake() 
    {
        _movement = GetComponent<PlayerMovement>();

        _mousePointerPrimitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        _mousePointerPrimitive.GetComponent<Collider>().enabled = false;

        
        var headIKSourceObjects = headIK.data.sourceObjects;
        var spineIKSourceObjects = spineIK.data.sourceObjects;
        var rightHandIKSourceObjects = rightHandIK.data.sourceObjects;

        headIKSourceObjects.SetTransform(0,_mousePointerPrimitive.transform);
        spineIKSourceObjects.SetTransform(0,_mousePointerPrimitive.transform);
        rightHandIKSourceObjects.SetTransform(0,_mousePointerPrimitive.transform);

        headIK.data.sourceObjects = headIKSourceObjects;
        spineIK.data.sourceObjects = spineIKSourceObjects;
        rightHandIK.data.sourceObjects = rightHandIKSourceObjects;

        rigBuilder.Build();
    }


    void LateUpdate()
    {
        _mousePointerPrimitive.transform.position = _movement.MouseWorldPos;
    }
}
