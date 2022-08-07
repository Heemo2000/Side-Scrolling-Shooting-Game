using UnityEngine;

public class BulletPowerup : MonoBehaviour
{
    [SerializeField]private Bullet bulletPrefab;

    public Bullet GetBulletPrefab()
    {
        return bulletPrefab;
    }
}
