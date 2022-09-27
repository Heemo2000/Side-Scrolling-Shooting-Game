using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    private Rigidbody bulletRB;

    private Collider _bulletCollider;
    private float _currentTime = 0f;

    private CommonBulletData _bulletData;

    private Vector3 _destination;

    private Vector3 _firedDirection;
    public CommonBulletData BulletData
    {
        get => _bulletData;
        set => _bulletData = value;
    }
    protected Rigidbody BulletRB { get => bulletRB; }

    protected virtual void Awake() 
    {
        bulletRB = GetComponent<Rigidbody>();
        _bulletCollider = GetComponent<Collider>();    
    }

    protected virtual void Update() 
    {
        if(_currentTime >= _bulletData.destroyTime)
        {
            Destroy(gameObject);
            return;
        }

        _currentTime += Time.deltaTime;
    }
    protected virtual void OnTriggerEnter(Collider other) 
    {
        int colliderMaskValue = 1 << other.gameObject.layer;
        if((_bulletData.ignoreMask.value & colliderMaskValue) != 0)
        {
            Physics.IgnoreCollision(_bulletCollider,other);
            return;
        }

        Health health = other.gameObject.GetComponent<Health>();
        health?.OnHealthDamaged(_bulletData.damage);
        Destroy(gameObject);
    }
}
