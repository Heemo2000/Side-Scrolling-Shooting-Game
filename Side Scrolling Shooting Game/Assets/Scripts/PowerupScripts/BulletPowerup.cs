using UnityEngine;
using Cinemachine;
public class BulletPowerup : MonoBehaviour
{
    [SerializeField]private Bullet bulletPrefab;

    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("Detected :" + other.gameObject.name);
         PlayerCollisionDetection playerCollision = other.gameObject.GetComponent<PlayerCollisionDetection>();

         if(playerCollision != null)
         {
            playerCollision.GetPlayerReference().WeaponManager.ChangeBullet(bulletPrefab);
            Destroy(gameObject);
         }
    }

}
