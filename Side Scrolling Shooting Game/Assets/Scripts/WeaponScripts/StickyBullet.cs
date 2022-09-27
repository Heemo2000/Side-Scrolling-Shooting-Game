using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBullet : NormalBullet
{
    [Min(0f)]
    [SerializeField]private float damageRadius = 3.0f;   
    [Min(0f)]
    [SerializeField]private float stickDuration = 1.0f;

    private Coroutine _stickAndExplodeCoroutine;
    private IEnumerator StickAndExplode(Collider other)
    {
        Stick(other);
        yield return new WaitForSeconds(stickDuration);
        DamageSurroundings();
        Destroy(gameObject);
    }

    private void Stick(Collider other)
    {
        base.BulletRB.velocity = Vector3.zero;
        base.BulletRB.isKinematic = true;
        base.BulletRB.detectCollisions = false;
        transform.parent = other.gameObject.transform;
    }

    private void DamageSurroundings()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position,damageRadius,~base.BulletData.ignoreMask.value);
        foreach(Collider collider in detectedColliders)
        {
            Health health = collider.gameObject.GetComponent<Health>();
            health?.OnHealthDamaged?.Invoke(base.BulletData.damage);
        }
    }
    protected override void OnTriggerEnter(Collider other) 
    {
        if(_stickAndExplodeCoroutine == null)
        {
          _stickAndExplodeCoroutine = StartCoroutine(StickAndExplode(other));  
        }

    }
}
