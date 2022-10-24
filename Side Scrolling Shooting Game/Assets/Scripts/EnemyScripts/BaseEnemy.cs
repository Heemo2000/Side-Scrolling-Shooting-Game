using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField]private Transform target;
    [SerializeField]private BarsUI healthBarPrefab;
    [SerializeField]private Transform healthBarFollowTransform;
    [SerializeField]private Powerup[] powerupsToSpawn;
    [SerializeField]private Transform powerupSpawnPos;
    public Transform Target { get => target; set => target = value; }
    public Health EnemyHealth { get => _enemyHealth; }
    public Powerup[] PowerupsToSpawn { get => powerupsToSpawn; set => powerupsToSpawn = value; }

    private Health _enemyHealth;
    private BarsUI _healthBar;
    protected virtual void Awake()
    {
        _enemyHealth = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        _healthBar = Instantiate(healthBarPrefab,transform.position,Quaternion.identity);
        _healthBar.FollowTarget = healthBarFollowTransform;
        _enemyHealth.OnHealthDamaged += ScoreManager.Instance.OnScoreIncreased;
        _enemyHealth.OnCurrentHealthSet += _healthBar.SetFillAmount;
        _enemyHealth.OnDeath += DestroyEnemy;
        _enemyHealth.OnDeath += SpawnPowerups;
    }

    protected void DestroyEnemy()
    {
        Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    protected abstract void HandleBehaviour();

    private void SpawnPowerups()
    {
        if(powerupsToSpawn == null)
        {
            return;
        }
        if(Random.value >= 0.6f)
        {
            int powerupIndex = Random.Range(0,powerupsToSpawn.Length);
            Instantiate(powerupsToSpawn[powerupIndex],powerupSpawnPos.position,Quaternion.identity);
        }
        
    }

    private void OnDestroy()
    {
        _enemyHealth.OnCurrentHealthSet -= _healthBar.SetFillAmount;
        _enemyHealth.OnDeath -= DestroyEnemy;
        _enemyHealth.OnDeath -= SpawnPowerups;
    }
}
