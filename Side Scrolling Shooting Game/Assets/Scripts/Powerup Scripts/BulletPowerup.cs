using UnityEngine;
using Cinemachine;
public class BulletPowerup : MonoBehaviour
{
    [SerializeField]private Bullet bulletPrefab;

    public Bullet GetBulletPrefab()
    {
        return bulletPrefab;
    }

    private void ObtainBulletPrefab(out Bullet prefab)
    {
        prefab = bulletPrefab;
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision other) 
    {
        PlayerCollisionDetection detection = other.gameObject.GetComponent<PlayerCollisionDetection>();
        if(detection != null)
        {
            detection.GetPlayerReference().TakeDamage(1f);
            Destroy(gameObject);
        }
    }
}
