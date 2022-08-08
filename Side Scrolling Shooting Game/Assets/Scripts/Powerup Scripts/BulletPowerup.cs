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
        Debug.Log("Collided object : " + other.gameObject.name);
        Player player = other.gameObject.GetComponent<Player>();
        if(player != null)
        {
            player.TakeDamage(1f);
            Destroy(gameObject);
        }
    }
}
