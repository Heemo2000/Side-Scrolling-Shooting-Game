using System;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [Min(0f)]
    [SerializeField]private float fireInterval = 0.1f;
    [SerializeField]private Transform firePoint;

    [SerializeField]private CommonBulletData bulletData;

    [SerializeField]private AudioSource gunAudioSource;
    public UnityEvent OnBulletShoot;
    private float _nextTimeToFire = 0.0f;
    
    public void Fire()
    {
        if(_nextTimeToFire < Time.time)
        {
            OnBulletShoot?.Invoke();
            Quaternion initialRotation = Quaternion.LookRotation(firePoint.forward,firePoint.right);
            if(bulletData.tripleFire)
            {
                Quaternion firstBulletRotation = initialRotation * Quaternion.AngleAxis(30f,Vector3.up);
                Quaternion thirdBulletRotation = initialRotation * Quaternion.AngleAxis(-30f,Vector3.up);

                Bullet firstBullet = Instantiate(bulletData.bulletPrefab,firePoint.position,firstBulletRotation);
                firstBullet.BulletData = bulletData;

                Bullet secondBullet = Instantiate(bulletData.bulletPrefab,firePoint.position,initialRotation);
                secondBullet.BulletData = bulletData;

                Bullet thirdBullet = Instantiate(bulletData.bulletPrefab,firePoint.position,thirdBulletRotation);
                thirdBullet.BulletData = bulletData;
            }
            else
            {
                Bullet bullet = Instantiate(bulletData.bulletPrefab,firePoint.position,initialRotation);
                bullet.BulletData = bulletData;
            } 
            
            SoundManager.Instance?.PlaySFX(bulletData.bulletSound);
            _nextTimeToFire = Time.time + fireInterval;
        }
    }

    
    public void SetBulletData(CommonBulletData bulletData)
    {
        if(bulletData != null)
        {
            this.bulletData = bulletData;
        }
    }
    
}
