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
    public Health PlayerHealth { get => _playerHealth; }

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
        _playerHealth.OnDeath += DisablePlayer;
    }

    public void OnHealthDamaged(float damage)
    {
        //damageImpulse.GenerateImpulse();
    }

    private void DisablePlayer()
    {
        GameManager.Instance.OnGameOver?.Invoke();
        _healthBar.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    
    private void OnDestroy() 
    {
        _playerHealth.OnCurrentHealthSet -= _healthBar.SetFillAmount;
        _playerHealth.OnHealthDamaged -= OnHealthDamaged;
        _playerHealth.OnDeath -= DisablePlayer;
    }
}
