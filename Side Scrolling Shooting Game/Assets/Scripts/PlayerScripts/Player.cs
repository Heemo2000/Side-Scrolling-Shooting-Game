using UnityEngine;
using Cinemachine;


public class Player : MonoBehaviour
{
    [SerializeField]private CinemachineImpulseSource damageImpulse;
    [SerializeField]private BarsUI healthBarPrefab;
    [SerializeField]private Transform healthBarFollowTransform;
    private Health _playerHealth;
    private PlayerWeaponManager _weaponManager;

    private BarsUI _healthBar;
    public PlayerWeaponManager WeaponManager { get => _weaponManager; }
    private void Awake() 
    {
        _playerHealth = GetComponent<Health>();
        _weaponManager = GetComponent<PlayerWeaponManager>();    
    }

    private void Start() 
    {
        _healthBar = Instantiate(healthBarPrefab,transform.position,Quaternion.identity);
        _healthBar.FollowTarget = healthBarFollowTransform;
        _playerHealth.OnCurrentHealthSet += _healthBar.SetFillAmount;
        _playerHealth.OnHealthDamaged += OnHealthDamaged;
        _playerHealth.OnDeath += DestroyPlayer;
    }

    public void OnHealthDamaged(float damage)
    {
        //damageImpulse.GenerateImpulse();
    }

    private void DestroyPlayer()
    {
        Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("Detecting OnCollisionEnter from player.");
        ContactPoint contact = other.contacts[0];
        float angle = Vector3.Angle(Vector3.up,contact.normal);
        if(angle >= -15f && angle <= 15f)
        {
            transform.parent = other.transform;
        }    
    }

    private void OnCollisionExit(Collision other) 
    {
        Debug.Log("Detecting OnCollisionExit from player.");
        if(transform.parent != null)
        {
            transform.parent = null;
        }    
    }

    private void OnDestroy() 
    {
        _playerHealth.OnCurrentHealthSet -= _healthBar.SetFillAmount;
        _playerHealth.OnHealthDamaged -= OnHealthDamaged;
        _playerHealth.OnDeath -= DestroyPlayer;
    }
}
