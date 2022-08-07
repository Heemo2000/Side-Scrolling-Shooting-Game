using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Min(0f)]
    [SerializeField]private float fireInterval = 0.1f;
    [SerializeField]private Transform firePoint;

    [SerializeField]private Bullet bulletPrefab;

    private float _nextTimeToFire = 0.0f;

    public void Fire()
    {
        if(_nextTimeToFire < Time.time)
        {
            Bullet bullet = Instantiate(bulletPrefab,firePoint.position,Quaternion.identity);
            bullet.transform.right = firePoint.right;
            _nextTimeToFire = Time.time + fireInterval;
        }
    }

    public void SetBulletPrefab(Bullet bulletPrefab)
    {
        if(bulletPrefab != null)
        {
            this.bulletPrefab = bulletPrefab;
        }
    }
}
