using UnityEngine;
using Cinemachine;
public class BulletPowerup : Powerup
{
    [SerializeField]private Bullet bulletPrefab;

    private void OnCollisionEnter(Collision other) 
    {
         Player player = other.gameObject.GetComponent<Player>();

         if(player != null)
         {
            player.WeaponManager.ChangeBullet(bulletPrefab);
            Destroy(gameObject);
         }
    }

}
