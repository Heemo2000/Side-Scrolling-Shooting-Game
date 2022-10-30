using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StickyBullet : NormalBullet
{
    [Min(0f)]
    [SerializeField]private float damageRadius = 3.0f;   
    [Min(0f)]
    [SerializeField]private float stickDuration = 1.0f;

    [SerializeField]private ParticleSystem explosionEffect;
    
    private Coroutine _stickAndExplodeCoroutine;
    private bool _isHit;
    private CinemachineImpulseSource _explosionImpulse;

    protected override void Awake()
    {
        base.Awake();
        _explosionImpulse = GetComponent<CinemachineImpulseSource>();
    }
    private IEnumerator StickAndExplode(Collider other)
    {
        Stick(other);
        yield return new WaitForSeconds(stickDuration);
        DamageSurroundings();
        Destroy(gameObject);
        yield return null;
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
        //Debug.Log("Damaging surroundings");
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position,damageRadius,~base.BulletData.ignoreMask.value);
        foreach(Collider collider in detectedColliders)
        {
            //Debug.Log("Detected " + collider.gameObject.name);
            Health health = collider.GetComponent<Health>();
            health?.OnHealthDamaged?.Invoke(base.BulletData.damage);
        }

        Instantiate(explosionEffect,transform.position,Quaternion.identity);
        _explosionImpulse.GenerateImpulse();
        SoundManager.Instance?.PlaySFXInstantly(SoundType.Explosion);
        
    }
    protected override void OnTriggerEnter(Collider other) 
    {
        //Debug.Log("On Trigger Enter called");
        int colliderMaskValue = 1 << other.gameObject.layer;
        if((base.BulletData.ignoreMask.value & colliderMaskValue) != 0)
        {
            //Debug.Log("Ignoring collision");
            Physics.IgnoreCollision(base.BulletCollider,other);
            return;
        }
        if(_isHit == false)
        {
            _stickAndExplodeCoroutine = StartCoroutine(StickAndExplode(other));
            _isHit = true;    
        }

    }
}
