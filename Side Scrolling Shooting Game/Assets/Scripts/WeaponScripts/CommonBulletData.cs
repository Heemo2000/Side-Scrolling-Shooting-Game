using UnityEngine;


[CreateAssetMenu(menuName = "Scriptable Objects/Common Bullet Data",fileName = "Common Bullet Data")]
public class CommonBulletData : ScriptableObject
{
    public LayerMask ignoreMask;
    
    [Min(0f)]
    public float damage = 10f;
    
    [Min(0f)]
    public float moveSpeed = 5f;
    
    [Min(0f)]
    public float destroyTime = 2f;
    
    public bool tripleFire = false;
    
    public Bullet bulletPrefab;
    
    public SoundType bulletSound;

    [Min(0f)]
    public float fireInterval = 0.2f;

    [Min(0f)]
    public float timeToAllowSpawn = 1.0f; 
}
