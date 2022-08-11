using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class Player : MonoBehaviour
{
    private Health _playerHealth;

    [SerializeField]private CinemachineImpulseSource damageImpulse;

    private PlayerWeaponManager _weaponManager;

    public PlayerWeaponManager WeaponManager { get => _weaponManager; }

    private void Awake() 
    {
        _playerHealth = GetComponent<Health>();
        _weaponManager = GetComponent<PlayerWeaponManager>();    
    }

    public void TakeDamage(float damage)
    {
        _playerHealth.OnHealthDamaged(damage);
        damageImpulse.GenerateImpulse();
    }
    
}
