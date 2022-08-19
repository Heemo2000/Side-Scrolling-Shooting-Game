using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField]private Transform target;

    public Transform Target { get => target; set => target = value; }
    public Health EnemyHealth { get => enemyHealth; }
    private Health enemyHealth;

    protected virtual void Awake()
    {
        enemyHealth = GetComponent<Health>();
    }

    protected virtual void Start()
    {
        enemyHealth.OnDeath += DestroyEnemy;
    }

    protected void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    protected abstract void HandleBehaviour();

    private void OnDestroy()
    {
        enemyHealth.OnDeath -= DestroyEnemy;
    }
}
