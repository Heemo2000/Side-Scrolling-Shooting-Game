using UnityEngine;
using Cinemachine;
public class BulletPowerup : MonoBehaviour
{
    [SerializeField]private Bullet bulletPrefab;

    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("Detected :" + other.gameObject.name);
         Player player = other.gameObject.GetComponent<Player>();

         if(player != null)
         {
            player.WeaponManager.ChangeBullet(bulletPrefab);
            Destroy(gameObject);
         }
    }

}
