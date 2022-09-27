using UnityEngine;
using Cinemachine;
public class BulletPowerup : Powerup
{
    [SerializeField]private CommonBulletData bulletData;

    private void OnCollisionEnter(Collision other) 
    {
         Player player = other.gameObject.GetComponent<Player>();

         if(player != null)
         {
            player.WeaponManager.ChangeBulletData(bulletData);
            Destroy(gameObject);
         }
    }

}
