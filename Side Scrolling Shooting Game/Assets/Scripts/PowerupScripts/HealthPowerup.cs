using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : Powerup
{
    [Min(0f)]
    [SerializeField]private float healthRecharge;

    private void OnTriggerEnter(Collider other) 
    {
         Player player = other.gameObject.GetComponent<Player>();

         if(player != null)
         {
            player.PlayerHealth.OnHealthHealed?.Invoke(healthRecharge);
            Destroy(gameObject);
         }
    }
}
