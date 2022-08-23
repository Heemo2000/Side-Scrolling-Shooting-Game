using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGun : BaseEnemy
{
    [SerializeField]private float lookOffSetY = 2.0f;
    [SerializeField]private Transform firePoint;   
    [Range(5f,100f)]
    [SerializeField]private float targetCheckDistance = 6f;

    [SerializeField]private LayerMask rayCastIgnoreLayerMask;
    [SerializeField]private Gun gun;
    private GenericAimHandler _aimHandler;

    

    protected override void Awake() {
        base.Awake();
        _aimHandler = GetComponent<GenericAimHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBehaviour();
    }

    protected override void HandleBehaviour()
    {
        _aimHandler.SetAimPosition(Target.position + Vector3.up * lookOffSetY);
        HandleShooting();
    }
    private void HandleShooting()
    {
        if(Target == null)
        {
            return;
        }   
        if(Physics.Raycast(firePoint.position,firePoint.right,out RaycastHit hit,targetCheckDistance,~rayCastIgnoreLayerMask.value))
        {
            Debug.DrawLine(firePoint.position,hit.point);
            if(hit.transform.gameObject.layer == Target.gameObject.layer)
            {
                gun.Fire();
            }
        }
    }
}
