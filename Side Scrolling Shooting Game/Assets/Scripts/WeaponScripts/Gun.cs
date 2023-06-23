using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    [Min(0f)]
    [SerializeField]private float fireInterval = 0.1f;
    [SerializeField]private Transform firePoint;

    [SerializeField]private CommonBulletData defaultBulletData;
    [SerializeField]private AudioSource gunAudioSource;
    [SerializeField]private Bullet[] bulletPrefabs;
    public UnityEvent OnBulletShoot;
    private float _nextTimeToFire = 0.0f;

    private CommonBulletData _currentBulletData;
    private void Start() 
    {
        _currentBulletData = defaultBulletData;
    }

    public void Fire()
    {

        fireInterval = defaultBulletData.fireInterval;
        if(_nextTimeToFire < Time.time)
        {
            OnBulletShoot?.Invoke();
            Quaternion initialRotation = Quaternion.LookRotation(firePoint.forward,firePoint.right);
            
            if(_currentBulletData.tripleFire)
            {
                Quaternion firstBulletRotation = initialRotation * Quaternion.AngleAxis(30f,Vector3.up);
                Quaternion thirdBulletRotation = initialRotation * Quaternion.AngleAxis(-30f,Vector3.up);

                Bullet firstBullet = ObjectPoolManager.Instance.ReuseObject(_currentBulletData.bulletPrefab.gameObject,
                                                                            firePoint.position,
                                                                            firstBulletRotation) as Bullet;
                firstBullet.BulletData = _currentBulletData;

                Bullet secondBullet = ObjectPoolManager.Instance.ReuseObject(_currentBulletData.bulletPrefab.gameObject,
                                                                             firePoint.position,
                                                                             initialRotation) as Bullet;
                secondBullet.BulletData = _currentBulletData;

                Bullet thirdBullet = ObjectPoolManager.Instance.ReuseObject(_currentBulletData.bulletPrefab.gameObject,
                                                                            firePoint.position,
                                                                            thirdBulletRotation) as Bullet;
                thirdBullet.BulletData = _currentBulletData;
            }
            else
            {
                Bullet bullet = ObjectPoolManager.Instance.ReuseObject(_currentBulletData.bulletPrefab.gameObject,
                                                                       firePoint.position,
                                                                       initialRotation) as Bullet;
                bullet.BulletData = _currentBulletData;
            } 
            
            SoundManager.Instance?.PlaySFXInstantly(_currentBulletData.bulletSound);
            _nextTimeToFire = Time.time + fireInterval;
        }
    }

    
    public void SetBulletData(CommonBulletData bulletData)
    {
        if(bulletData != null)
        {
            _currentBulletData = bulletData;
        }
    }

    public IEnumerator SetBulletDataCoroutine(CommonBulletData bulletData)
    {
        SetBulletData(bulletData);
        yield return new WaitForSeconds(bulletData.timeToAllowSpawn);
        SetBulletData(defaultBulletData);
    }
    
}
