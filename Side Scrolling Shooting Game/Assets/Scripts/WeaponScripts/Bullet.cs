using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Min(0f)]
    [SerializeField]private float moveSpeed = 5f;
    [Min(0f)]
    [SerializeField]private float destroyTime = 2f;

    [SerializeField]private LayerMask ignoreMask;
    private Rigidbody _bulletRB;

    private Collider _bulletCollider;

    private float _currentTime = 0f;

    private void Awake() 
    {
        _bulletRB = GetComponent<Rigidbody>();
        _bulletCollider = GetComponent<Collider>();    
    }

    private void Update() {
        if(_currentTime >= destroyTime)
        {
            Destroy(gameObject);
            return;
        }

        _currentTime += Time.deltaTime;
    }
    private void FixedUpdate() 
    {
        _bulletRB.AddForce(transform.right * moveSpeed,ForceMode.Impulse);   
    }

    private void OnCollisionEnter(Collision other) 
    {
        if(gameObject.layer == ignoreMask.value)
        {
            Physics.IgnoreCollision(_bulletCollider,other.collider);
        }

        Destroy(gameObject);
    }
}