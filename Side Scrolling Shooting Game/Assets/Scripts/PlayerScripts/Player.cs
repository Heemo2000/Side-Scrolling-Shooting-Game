using UnityEngine;
using Cinemachine;


public class Player : MonoBehaviour
{
    [SerializeField]private CinemachineImpulseSource damageImpulse;
    [SerializeField]private BarsUI healthBarPrefab;
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
        _healthBar.FollowTarget = transform;
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
        Destroy(gameObject);
        Destroy(_healthBar.gameObject);
    }
    private void OnDestroy() 
    {
        _playerHealth.OnCurrentHealthSet -= _healthBar.SetFillAmount;
        _playerHealth.OnHealthDamaged -= OnHealthDamaged;
        _playerHealth.OnDeath -= DestroyPlayer;
    }
}
