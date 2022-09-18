using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField]private Transform target;
    [SerializeField]private BarsUI healthBarPrefab;
    [SerializeField]private Transform healthBarFollowTransform;
    public Transform Target { get => target; set => target = value; }
    public Health EnemyHealth { get => _enemyHealth; }
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
    }

    protected void DestroyEnemy()
    {
        Destroy(_healthBar.gameObject);
        Destroy(gameObject);
    }

    protected abstract void HandleBehaviour();

    private void OnDestroy()
    {
        _enemyHealth.OnCurrentHealthSet -= _healthBar.SetFillAmount;
        _enemyHealth.OnDeath -= DestroyEnemy;
    }
}
